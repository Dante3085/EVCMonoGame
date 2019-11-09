
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

        private Tile hoveredTile = null;
        private List<Tile> copyBuffer = new List<Tile>();
        private List<Tile> selection = new List<Tile>();
        private Rectangle selectionBox = Rectangle.Empty;
        private Vector2 selectionBoxStartPoint = Vector2.Zero;
        private bool selectionBoxHasStartPoint = false;
        private bool drawMultipleTilesAtOnce = false;
        private Rectangle minimalBoundingBox = Rectangle.Empty;
        private bool movingSelection = false;

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

        public String CurrentTileInfo
        {
            get
            {
                if (selection.Count == 1)
                {
                    return selection[0].ToString();
                }
                else if (selection.Count > 1)
                {
                    return "More than one Tile selected.";
                }
                else
                {
                    return "No Tile selected";
                }
            }
        }

        #endregion

        public DrawingArea(Rectangle bounds, TileSelection tileSelection)
        {
            this.bounds = bounds;
            this.tileSelection = tileSelection;
        }

        public void Update(GameTime gameTime)
        {
            currentMousePosition = InputManager.CurrentMousePosition();
            mouseTravel = currentMousePosition - InputManager.PreviousMousePosition();

            UpdateTileDrawing();
            UpdateHoveredTile();
            UpdateDetectingSelection();
            UpdateMovingSelection();
            UpdateSelectionCopyCutPasteDelete();
        }

        #region UpdateHelper

        private void UpdateTileDrawing()
        {
            Tile tsct = tileSelection.CurrentTile;

            if (!tileSelection.IsHoveredByMouse &&
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

            tiles.Add(newTile);
        }

        private void UpdateHoveredTile()
        {
            if (tileSelection.IsHoveredByMouse ||
                drawTileSelectionCurrentTileOnMouse ||
                selectionBoxHasStartPoint)
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
            if (tileSelection.IsHoveredByMouse ||
                drawTileSelectionCurrentTileOnMouse ||
                movingSelection)
                return;

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
            if (hoveredTile != null ||
                minimalBoundingBox.Contains(currentMousePosition))
                return;

            if (InputManager.OnLeftMouseButtonClicked())
            {
                selectionBoxStartPoint = currentMousePosition;
                selectionBoxHasStartPoint = true;
                minimalBoundingBox = Rectangle.Empty;
            }
            else if (InputManager.OnLeftMouseButtonReleased())
            {
                selectionBoxHasStartPoint = false;

                // Find out which Tiles were selected.
                // We want to know the minimumBoundingBox of all selected Tiles as well.
                Vector2 topLeft = new Vector2(float.MaxValue, float.MaxValue);
                Vector2 bottomRight = new Vector2(float.MinValue, float.MinValue);

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
            if (selectionBoxHasStartPoint && InputManager.IsLeftMouseButtonDown())
            {
                selectionBox.Width = (int)Math.Abs(currentMousePosition.X - selectionBoxStartPoint.X);
                selectionBox.Height = (int)Math.Abs(currentMousePosition.Y - selectionBoxStartPoint.Y);
                selectionBox.X = (int)Math.Min(selectionBoxStartPoint.X, currentMousePosition.X);
                selectionBox.Y = (int)Math.Min(selectionBoxStartPoint.Y, currentMousePosition.Y);
            }
        }

        private void UpdateMovingSelection()
        {
            if (tileSelection.IsHoveredByMouse ||
                selection.Count == 0 ||
                selectionBoxHasStartPoint)
                return;

            if (minimalBoundingBox.Contains(currentMousePosition) &&
                InputManager.OnLeftMouseButtonClicked())
            {
                movingSelection = true;
            }
            else if (InputManager.OnLeftMouseButtonReleased())
            {
                movingSelection = false;
            }

            if (movingSelection &&
                mouseTravel != Vector2.Zero)
            {
                foreach (Tile tile in selection)
                {
                    tile.screenBounds.Location += mouseTravel.ToPoint();
                }
                minimalBoundingBox.Location += mouseTravel.ToPoint();
            }
        }

        private void UpdateSelectionCopyCutPasteDelete()
        {
            if (tileSelection.IsHoveredByMouse)
                return;

            if (selection.Count != 0 &&
                InputManager.IsKeyPressed(Keys.Delete))
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
                     InputManager.OnKeyPressed(Keys.C))
            {
                copyBuffer.Clear();
                copyBuffer.AddRange(selection);
            }

            // Cut
            else if (selection.Count != 0 &&
                     InputManager.OnKeyPressed(Keys.X))
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
                     InputManager.OnKeyPressed(Keys.V))
            {
                Vector2 shiftVector = currentMousePosition - copyBuffer[0].screenBounds.Location.ToVector2();

                foreach (Tile tile in copyBuffer)
                {
                    Vector2 newTilePosition = tile.screenBounds.Location.ToVector2() + shiftVector;

                    Tile newTile = new Tile(tile.name, tile.textureBounds,
                        new Rectangle(newTilePosition.ToPoint(), tile.screenBounds.Size));

                    tiles.Add(newTile);
                }
            }
        }

        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Draw all Tiles on DrawingArea.
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tileSelection.TileSet, tile.screenBounds, tile.textureBounds, Color.White);
            }

            // Draw tileSeleciton's currentTile.
            if (drawTileSelectionCurrentTileOnMouse)
            {
                spriteBatch.Draw(tileSelection.TileSet, new Rectangle(currentMousePosition.ToPoint(), tileSelection.CurrentTile.screenBounds.Size),
                                 tileSelection.CurrentTile.textureBounds, Color.White);
            }

            // Mark hovered Tile.
            if (hoveredTile != null)
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
            if (selection.Count != 0)
            {
                // Mark selection with one Tile.
                if (selection.Count == 1)
                {
                    Primitives2D.DrawRectangle(spriteBatch, selection[0].screenBounds, Color.DarkRed, 5);
                }

                // Mark selection with multiple Tiles.
                else
                {
                    Primitives2D.DrawRectangle(spriteBatch, minimalBoundingBox, Color.DarkRed, 5);
                }
            }

            spriteBatch.End();
        }

        private Matrix getZoom()
        {
            if (InputManager.CurrentScrollWheel() != InputManager.PreviousScrollWheel())
            {
                Console.WriteLine(InputManager.CurrentScrollWheel());
            }
            if (InputManager.CurrentScrollWheel() < InputManager.PreviousScrollWheel() && zoom > 0.001f)
            {
                zoom -= 0.01f + (0.04f * zoom);
            }
            else if (InputManager.CurrentScrollWheel() > InputManager.PreviousScrollWheel())
            {
                zoom += 0.01f + (0.1f * zoom);
            }

            Vector2 point = (-zoom) * new Vector2(bounds.Width * 0.5f, bounds.Height * 0.5f) + new Vector2(bounds.Width * 0.5f, bounds.Height * 0.5f);
            return new Matrix(
                     new Vector4(zoom, 0, 0, 0),
                     new Vector4(0, zoom, 0, 0),
                     new Vector4(0, 0, 1, 0),
                     new Vector4(point.X, point.Y, 0, 1));
            //return Matrix.Identity;
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void SaveToFile(String path)
        {
            if (System.IO.File.Exists(path))
            {
                throw new ArgumentException("The file '" + path + "' already exists.\n" +
                    "Delete it manually if you want to overwrite it!");
            }

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
