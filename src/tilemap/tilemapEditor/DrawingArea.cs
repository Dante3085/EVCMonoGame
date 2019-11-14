
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using C3.MonoGame;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.input;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.tilemap.tilemapEditor
{
    // TODO: Vernünftig kommentieren...
    // TODO: Maybe make DrawingArea Mouse Tile hovering more performant by using Spacial-Partitioning(Grid, BSPTree).
    // TODO: Besides hoveredTile, create the notion of currentTile in DrawingArea(Red Marker).
    // TODO: Display number of Tiles in DrawingArea.
    // TODO: Rectangle selection of many Tiles and move them around as a unit.
    // TODO: Copy content of Rectangle selection with STRG+C and place it at mouse position with STRG+V.
    // TODO: Make lining up Tiles when moving a Tile easy. Select a Tile and move 1 unit with arrow keys.
    // TODO: Make deleting Tiles in DrawingArea possible.

    // TODO: Make it possible to scale the currentTile of DrawingArea by placing Mouse at bottom right corner, 
    //       pressing LeftMouseButton and moving the Mouse around.

    // TODO: Make moving of DrawingArea possible so that a DrawingArea bigger than the Screen is still accessible.
    //       Make it impossible that the whole DrawingArea goes outside the screen.
    //       An alternative is using the Camera class, but that maybe presents other problems.

    // TODO: Make it possible to draw multiple Tiles in quick succession.

    // TODO: InfoText ergänzen. Grid und TileSelection Hide show.

    public class DrawingArea
    {
        #region Fields

        private Rectangle bounds;
        private List<Tile> tiles = new List<Tile>();
        private Vector2 currentMousePosition = Vector2.Zero;
        private bool drawTileSelectionCurrentTileOnMouse = false;
        private TileSelection tileSelection;


        private Vector2 mouseTravel = Vector2.Zero;

        private float zoom = 1;
        private Vector2 position;

        private Tile hoveredTile = null;
        private List<Tile> copyBuffer = new List<Tile>();
        private List<Tile> undoBuffer = new List<Tile>();
        private List<Tile> selection = new List<Tile>();
        private Rectangle selectionBox = Rectangle.Empty;
        private Vector2 selectionBoxStartPoint = Vector2.Zero;
        private bool selectionBoxHasStartPoint = false;
        private bool drawMultipleTilesAtOnce = false;
        private Rectangle minimalBoundingBox = Rectangle.Empty;
        private bool movingSelectionWithMouse = false;
        private bool movingSelectionWithKeys = false;
        private bool scalingSelection = false;

        private const float holdDelay = 500;
        private float rightHoldElapsed = 0;
        private float leftHoldElapsed = 0;
        private float upHoldElapsed = 0;
        private float downHoldElapsed = 0;

        private int gridCellSize = 100;
        private bool gridActivated = false;

        #endregion
        #region Properties

        public List<Tile> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        public int NumTilesSelection
        {
            get { return selection.Count; }
        }

        public int NumTilesCopyBuffer
        {
            get { return copyBuffer.Count; }
        }

        public String SelectionInfo
        {
            get
            {
                if (selection.Count == 1)
                {
                    return selection[0].ToString();
                }
                else if (selection.Count > 1)
                {
                    return "minimalBoundingBox: " + minimalBoundingBox.ToString();
                }
                else
                {
                    return "No Tile selected";
                }
            }
        }
        public int GridCellSize
        {
            get { return gridCellSize; }
            set { gridCellSize = value; }
        }

        #endregion

        public DrawingArea(Rectangle bounds, TileSelection tileSelection)
        {
            this.bounds = bounds;
            this.tileSelection = tileSelection;
            this.gridCellSize = (int)tileSelection.TileSize.X;
        }

        public void Update(GameTime gameTime)
        {
            // Correct for zooming.
            currentMousePosition = InputManager.CurrentMousePosition();
            currentMousePosition -= position;
            currentMousePosition /= zoom;
            Vector2 previousMousePosition = InputManager.PreviousMousePosition();
            previousMousePosition -= position;
            previousMousePosition /= zoom;
            mouseTravel = currentMousePosition - previousMousePosition;

            // Update dragging.
            if (InputManager.IsMiddleMouseButtonDown())
            {
                Vector2 positionTemp = position;
                position += (InputManager.CurrentMousePosition() - InputManager.PreviousMousePosition());
                if (position.X > 0) position.X = positionTemp.X;
                if (position.Y > 0) position.Y = positionTemp.Y;
            }

            //toggle Grid
            if (InputManager.OnKeyCombinationPressed(Keys.LeftControl, Keys.G))
            {
                gridActivated = !gridActivated;
            }
            //Snap all Tiles
            if (gridActivated && InputManager.OnKeyCombinationPressed(Keys.LeftControl, Keys.LeftAlt, Keys.S))
            {
                SnapAllTilesToGrid();
            }

            // Update all DrawingArea components.
            UpdateTileDrawing();
            UpdateHoveredTile();
            UpdateDetectingSelection();
            UpdateMovingSelection(gameTime);
            UpdateSelectionCopyCutPasteDelete();
            UpdateScalingSelection();
        }

        #region UpdateHelper


        private void UpdateTileDrawing()
        {
            Tile tsct = tileSelection.CurrentTile;

            if ((!tileSelection.IsHoveredByMouse ||
                  tileSelection.Hidden) &&
                tsct != null)
            {
                drawTileSelectionCurrentTileOnMouse = true;

                if (InputManager.OnLeftMouseButtonClicked())
                {
                    DrawTileSelectionCurrentTile();
                    drawMultipleTilesAtOnce = true;
                }
                else if (InputManager.OnLeftMouseButtonReleased())
                {
                    drawMultipleTilesAtOnce = false;
                }

                if (drawMultipleTilesAtOnce)
                {
                    // If last added Tile and tileSelection's currentTile at currentMousePosition overlap, 
                    // draw tileSelection's currentTile again.
                    if (!tiles[tiles.Count - 1].screenBounds.Intersects(new Rectangle(currentMousePosition.ToPoint(), tsct.screenBounds.Size)))
                    {
                        DrawTileSelectionCurrentTile();
                    }
                }
            }
            else
            {
                drawTileSelectionCurrentTileOnMouse = false;
            }
        }

        private void DrawTileSelectionCurrentTile()
        {
            Tile tsct = tileSelection.CurrentTile;

            Tile newTile = new Tile(tsct.name, tsct.textureBounds,
                        new Rectangle(currentMousePosition.ToPoint(), tsct.screenBounds.Size));
            Vector2 insideCellPosition = new Vector2((int)(newTile.screenBounds.Location.X) % gridCellSize, (int)(newTile.screenBounds.Location.Y) % gridCellSize);
            if (insideCellPosition != Vector2.Zero && gridActivated)
            {
                Vector2 snappingVector = Vector2.Zero;
                if ((insideCellPosition.X) < gridCellSize / 2)//näher an linker Kante 
                {
                    snappingVector.X -= insideCellPosition.X;
                }
                else//näher an Rechter Kante
                {
                    snappingVector.X += (gridCellSize - insideCellPosition.X);
                }
                if ((insideCellPosition.Y) < gridCellSize / 2)//näher an oberer Kante 
                {
                    snappingVector.Y -= insideCellPosition.Y;
                }
                else//näher an unterer Kante
                {
                    snappingVector.Y += (gridCellSize - insideCellPosition.Y);
                }
                newTile.screenBounds.Location += snappingVector.ToPoint();
            }
            tiles.Add(newTile);
        }

        private void SnapAllTilesToGrid()
        {
            foreach (Tile tile in tiles)
            {
                Vector2 insideCellPosition = new Vector2((int)(tile.screenBounds.Location.X) % gridCellSize, (int)(tile.screenBounds.Location.Y) % gridCellSize);
                if (insideCellPosition != Vector2.Zero && gridActivated)
                {
                    Vector2 snappingVector = Vector2.Zero;
                    if ((insideCellPosition.X) < gridCellSize / 2)//näher an linker Kante 
                    {
                        snappingVector.X -= insideCellPosition.X;
                    }
                    else//näher an Rechter Kante
                    {
                        snappingVector.X += (gridCellSize - insideCellPosition.X);
                    }
                    if ((insideCellPosition.Y) < gridCellSize / 2)//näher an oberer Kante 
                    {
                        snappingVector.Y -= insideCellPosition.Y;
                    }
                    else//näher an unterer Kante
                    {
                        snappingVector.Y += (gridCellSize - insideCellPosition.Y);
                    }
                    tile.screenBounds.Location += snappingVector.ToPoint();
                }
            }
        }

        private void UpdateHoveredTile()
        {
            if (!tileSelection.Hidden &&
                (tileSelection.IsHoveredByMouse ||
                drawTileSelectionCurrentTileOnMouse ||
                selectionBoxHasStartPoint))
            {
                hoveredTile = null;
                return;
            }

            // If there is no hovered Tile we don't want to keep marking the previously hovered Tile,
            // so we set it to null here and if there actually is a hovered Tile this will be overriden
            // by the actual hovered Tile.
            hoveredTile = null;
            for (int i = tiles.Count - 1; i >= 0; --i)
            {
                Tile tile = tiles[i];
                if (tile.screenBounds.Contains(currentMousePosition))
                {
                    hoveredTile = tile;
                    return;
                }
            }
        }

        private void UpdateDetectingSelection()
        {
            if (!tileSelection.Hidden &&
                (tileSelection.IsHoveredByMouse ||
                drawTileSelectionCurrentTileOnMouse ||
                movingSelectionWithMouse))
                return;

            // Select all Tiles with STRG+A
            // Select all Tiles with STRG+A
            if (InputManager.OnKeyCombinationPressed(Keys.LeftControl, Keys.A))
            {
                selection.Clear();
                selection.AddRange(tiles);

                CalcSelectionMinimalBoundingBox();
            }

            // One Tile selected.
            if (hoveredTile != null &&
                !minimalBoundingBox.Contains(currentMousePosition) &&
                InputManager.OnLeftMouseButtonClicked())
            {
                // If we are clicking the already selected Tile again, just throw it out
                // of the selection.
                if (selection.Count == 1 &&
                    selection[0] == hoveredTile)
                {
                    selection.Clear();

                    minimalBoundingBox = Rectangle.Empty;
                }
                else
                {
                    selection.Clear();
                    selection.Add(hoveredTile);

                    minimalBoundingBox = hoveredTile.screenBounds;
                }
            }

            // Multiple Tiles selected with RectangleSelection.
            // We return on minimalBoundingBox.Contains(currentMousePosition) because
            // we don't want to cancel the selection(search for a new one) when the user
            // is trying to move it.
            if (/*hoveredTile != null ||*/
                minimalBoundingBox.Contains(currentMousePosition))
                return;

            if (InputManager.OnRightMouseButtonClicked())
            {
                selectionBoxStartPoint = currentMousePosition;
                selectionBoxHasStartPoint = true;
                minimalBoundingBox = Rectangle.Empty;
            }
            else if (InputManager.OnRightMouseButtonReleased())
            {
                selectionBoxHasStartPoint = false;

                // Find out which Tiles were selected.
                // We want to know the minimumBoundingBox of all selected Tiles as well.
                Vector2 topLeft = new Vector2(float.MaxValue, float.MaxValue);
                Vector2 bottomRight = new Vector2(float.MinValue, float.MinValue);

                // We don't use DetectSelectionMinimalBoundingBox() because we already have to iterate
                // over all Tiles to figure out which actually are inside the selection.
                // So while we are iterating over all Tiles we can simultaneously figure out the dimensions
                // of the minimalBoundingBox.
                selection.Clear();
                foreach (Tile tile in tiles)
                {
                    if (selectionBox.Contains(tile.screenBounds))
                    {
                        selection.Add(tile);

                        // Find out topLeft Point of minimumBoundingBox.
                        if (tile.screenBounds.Left < topLeft.X)
                            topLeft.X = tile.screenBounds.Left;
                        if (tile.screenBounds.Top < topLeft.Y)
                            topLeft.Y = tile.screenBounds.Top;

                        // Find bottomRight Point of minimumBoundingBox.
                        if (tile.screenBounds.Right > bottomRight.X)
                            bottomRight.X = tile.screenBounds.Right;
                        if (tile.screenBounds.Bottom > bottomRight.Y)
                            bottomRight.Y = tile.screenBounds.Bottom;
                    }
                }
                minimalBoundingBox = new Rectangle(topLeft.ToPoint(), (bottomRight - topLeft).ToPoint());
            }
            if (selectionBoxHasStartPoint && InputManager.IsRightMouseButtonDown())
            {
                selectionBox.Width = (int)Math.Abs(currentMousePosition.X - selectionBoxStartPoint.X);
                selectionBox.Height = (int)Math.Abs(currentMousePosition.Y - selectionBoxStartPoint.Y);
                selectionBox.X = (int)Math.Min(selectionBoxStartPoint.X, currentMousePosition.X);
                selectionBox.Y = (int)Math.Min(selectionBoxStartPoint.Y, currentMousePosition.Y);
            }
        }

        private void CalcSelectionMinimalBoundingBox()
        {
            Vector2 topLeft = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 bottomRight = new Vector2(float.MinValue, float.MinValue);

            foreach (Tile tile in selection)
            {
                // Find out topLeft Point of minimumBoundingBox.
                if (tile.screenBounds.Left < topLeft.X)
                    topLeft.X = tile.screenBounds.Left;
                if (tile.screenBounds.Top < topLeft.Y)
                    topLeft.Y = tile.screenBounds.Top;

                // Find bottomRight Point of minimumBoundingBox.
                if (tile.screenBounds.Right > bottomRight.X)
                    bottomRight.X = tile.screenBounds.Right;
                if (tile.screenBounds.Bottom > bottomRight.Y)
                    bottomRight.Y = tile.screenBounds.Bottom;
            }
            minimalBoundingBox = new Rectangle(topLeft.ToPoint(), (bottomRight - topLeft).ToPoint());
        }

        private void UpdateMovingSelection(GameTime gameTime)
        {
            if (!tileSelection.Hidden &&
                (tileSelection.IsHoveredByMouse ||
                selection.Count == 0 ||
                selectionBoxHasStartPoint ||
                scalingSelection))
                return;

            // Move selection with Mouse.
            if (minimalBoundingBox.Contains(currentMousePosition) &&
                InputManager.OnLeftMouseButtonClicked())
            {
                movingSelectionWithMouse = true;
            }
            else if (InputManager.OnLeftMouseButtonReleased())
            {
                movingSelectionWithMouse = false;
            }

            if (movingSelectionWithMouse &&
                mouseTravel != Vector2.Zero)
            {
                foreach (Tile tile in selection)
                {
                    tile.screenBounds.Location += mouseTravel.ToPoint();
                }
                minimalBoundingBox.Location += mouseTravel.ToPoint();

            }
            //Grid Snapping
            if (gridActivated && InputManager.OnLeftMouseButtonReleased())
            {
                Vector2 insideCellPosition = new Vector2((int)(minimalBoundingBox.Location.X) % gridCellSize, (int)(minimalBoundingBox.Location.Y) % gridCellSize);
                if (insideCellPosition != Vector2.Zero)
                {
                    Vector2 snappingVector = Vector2.Zero;
                    if ((insideCellPosition.X) < gridCellSize / 2)//näher an linker Kante 
                    {
                        snappingVector.X -= insideCellPosition.X;
                    }
                    else//näher an Rechter Kante
                    {
                        snappingVector.X += (gridCellSize - insideCellPosition.X);
                    }
                    if ((insideCellPosition.Y) < gridCellSize / 2)//näher an oberer Kante 
                    {
                        snappingVector.Y -= insideCellPosition.Y;
                    }
                    else//näher an unterer Kante
                    {
                        snappingVector.Y += (gridCellSize - insideCellPosition.Y);
                    }
                    minimalBoundingBox.Location += snappingVector.ToPoint();
                    Vector2 correctionVector = Vector2.Zero;
                    if (minimalBoundingBox.Location.X < 0)
                    {
                        correctionVector.X -= minimalBoundingBox.Location.X;
                    }
                    if (minimalBoundingBox.Location.Y < 0)
                    {
                        correctionVector.Y -= minimalBoundingBox.Location.Y;
                    }
                    minimalBoundingBox.Location += correctionVector.ToPoint();
                    foreach (Tile tile in selection)
                    {
                        tile.screenBounds.Location += snappingVector.ToPoint() + correctionVector.ToPoint();
                    }
                }
            }

            UpdateMovingSelectionWithKeys(gameTime);
        }

        private void UpdateMovingSelectionWithKeys(GameTime gameTime)
        {
            movingSelectionWithKeys = false;

            // Move Right
            if (InputManager.OnKeyPressed(Keys.Right))
            {
                // Move every Tile and the minimumBoundingBox one step to the right.
                foreach (Tile tile in selection)
                {
                    tile.screenBounds.Location += new Point(1, 0);
                }
                minimalBoundingBox.Location += new Point(1, 0);
            }
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                movingSelectionWithKeys = true;

                if (rightHoldElapsed >= holdDelay)
                {
                    // Move every Tile and the minimumBoundingBox one step to the right.
                    foreach (Tile tile in selection)
                    {
                        tile.screenBounds.Location += new Point(1, 0);
                    }
                    minimalBoundingBox.Location += new Point(1, 0);

                    rightHoldElapsed *= 0.90f;
                }
                else
                {
                    rightHoldElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else
            {
                rightHoldElapsed = 0;
            }

            // Move Left
            if (InputManager.OnKeyPressed(Keys.Left))
            {
                // Move every Tile and the minimumBoundingBox one step to the left.
                foreach (Tile tile in selection)
                {
                    tile.screenBounds.Location += new Point(-1, 0);
                }
                minimalBoundingBox.Location += new Point(-1, 0);
            }
            if (InputManager.IsKeyPressed(Keys.Left))
            {
                movingSelectionWithKeys = true;

                if (leftHoldElapsed >= holdDelay)
                {
                    // Move every Tile and the minimumBoundingBox one step to the left.
                    foreach (Tile tile in selection)
                    {
                        tile.screenBounds.Location += new Point(-1, 0);
                    }
                    minimalBoundingBox.Location += new Point(-1, 0);

                    leftHoldElapsed *= 0.90f;
                }
                else
                {
                    leftHoldElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else
            {
                leftHoldElapsed = 0;
            }

            // Move Up 
            if (InputManager.OnKeyPressed(Keys.Up))
            {
                // Move every Tile and the minimumBoundingBox one step to the top.
                foreach (Tile tile in selection)
                {
                    tile.screenBounds.Location += new Point(0, -1);
                }
                minimalBoundingBox.Location += new Point(0, -1);
            }
            if (InputManager.IsKeyPressed(Keys.Up))
            {
                movingSelectionWithKeys = true;

                if (upHoldElapsed >= holdDelay)
                {
                    // Move every Tile and the minimumBoundingBox one step to the top.
                    foreach (Tile tile in selection)
                    {
                        tile.screenBounds.Location += new Point(0, -1);
                    }
                    minimalBoundingBox.Location += new Point(0, -1);

                    upHoldElapsed *= 0.90f;
                }
                else
                {
                    upHoldElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else
            {
                upHoldElapsed = 0;
            }

            // Move Down.
            if (InputManager.OnKeyPressed(Keys.Down))
            {
                // Move every Tile and the minimumBoundingBox one step to bottom.
                foreach (Tile tile in selection)
                {
                    tile.screenBounds.Location += new Point(0, 1);
                }
                minimalBoundingBox.Location += new Point(0, 1);
            }
            if (InputManager.IsKeyPressed(Keys.Down))
            {
                movingSelectionWithKeys = true;

                if (downHoldElapsed >= holdDelay)
                {
                    // Move every Tile and the minimumBoundingBox one step to bottom.
                    foreach (Tile tile in selection)
                    {
                        tile.screenBounds.Location += new Point(0, 1);
                    }
                    minimalBoundingBox.Location += new Point(0, 1);

                    downHoldElapsed *= 0.90f;
                }
                else
                {
                    downHoldElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else
            {
                downHoldElapsed = 0;
            }
        }

        private void UpdateSelectionCopyCutPasteDelete()
        {
            if (!tileSelection.Hidden &&
                tileSelection.IsHoveredByMouse)
                return;

            if (selection.Count != 0 &&
                InputManager.OnKeyPressed(Keys.Delete))
            {
                tiles.RemoveAll((tile) =>
                {
                    return selection.Contains(tile);
                });
                selection.Clear();
                minimalBoundingBox = Rectangle.Empty;
            }

            // TODO: Hier möchte ich eigentlich OnAllKeysPressed() vom InputManager benutzen, um
            // die üblichen Tastenkombinationen STRG+C, STRG+X, STRG+V benutzen zu können.
            // Ich habe bis jetzt aber noch keine schöne bzw. überhaupt keine Lösung für OnAllKeysPressed()
            // gefunden.

            // Copy
            else if (selection.Count != 0 &&
                     InputManager.OnKeyCombinationPressed(Keys.LeftControl, Keys.C))
            {
                copyBuffer.Clear();
                copyBuffer.AddRange(selection);
            }

            // Cut
            else if (selection.Count != 0 &&
                     InputManager.OnKeyCombinationPressed(Keys.LeftControl, Keys.X))
            {
                copyBuffer.Clear();
                copyBuffer.AddRange(selection);

                // Remove all Tiles that have been cut from DrawingArea.
                tiles.RemoveAll((tile) =>
                {
                    return selection.Contains(tile);
                });

                minimalBoundingBox = Rectangle.Empty;
            }


            // Paste
            else if (copyBuffer.Count != 0 &&
                     InputManager.OnKeyCombinationPressed(Keys.LeftControl, Keys.V))
            {
                Vector2 shiftVector = currentMousePosition - (copyBuffer[0].screenBounds.Location.ToVector2());

                foreach (Tile tile in copyBuffer)
                {
                    Vector2 newTilePosition = tile.screenBounds.Location.ToVector2() + shiftVector;

                    Tile newTile = new Tile(tile.name, tile.textureBounds,
                        new Rectangle(newTilePosition.ToPoint(), tile.screenBounds.Size));

                    tiles.Add(newTile);
                }
            }
        }

        private void UpdateScalingSelection()
        {
            if (!tileSelection.Hidden &&
                (tileSelection.IsHoveredByMouse ||
                selection.Count == 0 ||
                selectionBoxHasStartPoint))
                return;

            scalingSelection = false;

            if (InputManager.IsKeyPressed(Keys.LeftAlt))
            {
                scalingSelection = true;
                foreach (Tile tile in selection)
                {
                    tile.screenBounds.Size += mouseTravel.ToPoint();
                }
            }
        }

        private Matrix CalcZoomMatrix()
        {
            if (InputManager.CurrentScrollWheel() < InputManager.PreviousScrollWheel() && zoom > 0.027f)
            {
                zoom -= 0.01f + (0.04f * zoom);
            }
            else if (InputManager.CurrentScrollWheel() > InputManager.PreviousScrollWheel())
            {
                zoom += 0.01f + (0.1f * zoom);
            }

            return new Matrix(
                   new Vector4(zoom, 0, 0, 0),
                   new Vector4(0, zoom, 0, 0),
                   new Vector4(0, 0, 1, 0),
                   new Vector4(/*point.X - */position.X, /*point.Y - */position.Y, 0, 1));

        }

        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: CalcZoomMatrix());

            // Draw all Tiles on DrawingArea.
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tileSelection.TileSet, tile.screenBounds, tile.textureBounds, Color.White);
            }

            // Draw tileSeleciton's currentTile.
            if (drawTileSelectionCurrentTileOnMouse)
            {
                Game1.MouseVisible = false;
                spriteBatch.Draw(tileSelection.TileSet, new Rectangle(currentMousePosition.ToPoint(), tileSelection.CurrentTile.screenBounds.Size),
                                 tileSelection.CurrentTile.textureBounds, Color.White);
            }
            else
            {
                Game1.MouseVisible = true;
            }

            // Mark hovered Tile.
            if (hoveredTile != null &&
                !movingSelectionWithMouse &&
                !movingSelectionWithKeys)
            {
                Primitives2D.DrawRectangle(spriteBatch, hoveredTile.screenBounds, Color.AliceBlue, 5);
            }

            // Draw selectionBox
            if (selectionBoxHasStartPoint)
            {
                Color color = Color.Blue;
                color.A = 15;

                Primitives2D.FillRectangle(spriteBatch, selectionBox, color);
            }

            // Mark selection.
            if (selection.Count != 0 /* &&
                !movingSelectionWithMouse &&
                !movingSelectionWithKeys*/)
            {
                Color boxColor = Color.DarkRed;
                boxColor.A = 50;

                // Mark selection with one Tile.
                if (selection.Count == 1)
                {
                    Primitives2D.FillRectangle(spriteBatch, selection[0].screenBounds, boxColor);
                }

                // Mark selection with multiple Tiles.
                else
                {
                    Primitives2D.FillRectangle(spriteBatch, minimalBoundingBox, boxColor);
                }
            }

            // draw Grid
            if (gridActivated)
            {
                float currentDisplayingWidth = bounds.Width / zoom;
                float currentDisplayingHeight = bounds.Height / zoom;
                for (float i = 0; i < -position.X / zoom + currentDisplayingWidth + gridCellSize; i += gridCellSize)
                {
                    float y = -position.Y / zoom + currentDisplayingHeight + gridCellSize;
                    Vector2 start = new Vector2(i, 0);
                    Vector2 end = new Vector2(i, y);
                    Primitives2D.DrawLine(spriteBatch, start, end, Color.DarkCyan, 2 / zoom);
                }
                for (float i = 0; i < -position.Y / zoom + currentDisplayingHeight + gridCellSize; i += gridCellSize)
                {
                    float x = -position.X / zoom + currentDisplayingWidth + gridCellSize;
                    Vector2 start = new Vector2(0, i);
                    Vector2 end = new Vector2(x, i);
                    Primitives2D.DrawLine(spriteBatch, start, end, Color.DarkCyan, 2 / zoom);
                }
            }


            spriteBatch.End();
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void SaveToFile(String path)
        {
            //if (System.IO.File.Exists(path))
            //{
            //    throw new ArgumentException("The file '" + path + "' already exists.\n" +
            //        "Delete it manually if you want to overwrite it!");
            //}

            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);

            // Write tileSet to file
            writer.WriteLine("TILESET = " + tileSelection.TileSet);

            // Write each Tile to file
            foreach (Tile tile in tiles)
            {
                // Separate every Tile by an empty line
                writer.WriteLine("");

                // Write Tile information to file
                writer.WriteLine("[TILE]");
                writer.WriteLine("NAME           = " + tile.name);
                writer.WriteLine("TEXTURE_BOUNDS = " + Utility.RectangleToString(tile.textureBounds));
                writer.WriteLine("SCREEN_BOUNDS  = " + Utility.RectangleToString(tile.screenBounds));
            }

            writer.Close();
        }
    }
}
