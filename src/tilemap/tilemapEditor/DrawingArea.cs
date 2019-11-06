
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
        private List<Tile> tiles                         = new List<Tile>();
        private Vector2 currentMousePosition             = Vector2.Zero;
        private bool drawTileSelectionCurrentTileOnMouse = false;
        private Rectangle destinationRectangle           = Rectangle.Empty;
        private Tile tileHoveredByMouse                  = null;
        private TileSelection tileSelection;

        private bool moveTileHoveredByMouse              = false;

        private Vector2 mouseTravel                      = Vector2.Zero;

        private SpriteFont font;
        private String currentTileInfo                   = String.Empty;

        private Tile currentTile                         = null;

        private bool drawHoveredTileMarker = false;
        private bool drawCurrentTileMarker = false;

        private List<Tile> rectangleSelection = new List<Tile>();
        private Rectangle selectionRectangle = Rectangle.Empty;
        private Vector2 selectionRectangleStartPoint = Vector2.Zero;
        private bool selectionRectangleHasStartPoint = false;
        private Rectangle selectionBoundingBox = Rectangle.Empty;

        #endregion
        #region Properties

        public List<Tile> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        #endregion

        public DrawingArea(Rectangle bounds, TileSelection tileSelection)
        {
            this.bounds = bounds;
            this.tileSelection = tileSelection;
        }

        public void UpdateTilePlacing()
        {
            bool tileSelectionHasCurrentTile = tileSelection.CurrentTile != null;

            // Check for drawing the currentTile of the TileSelection on the Mouse and
            // placing the currentTile of TileSelection in the DrawingArea.
            if (tileSelectionHasCurrentTile)
            {
                if (!tileSelection.IsHoveredByMouse)
                {
                    drawTileSelectionCurrentTileOnMouse = true;

                    if (InputManager.OnLeftMouseButtonClicked())
                    {
                        Tile tsCurrentTile = tileSelection.CurrentTile;
                        Tile newTile = new Tile(tsCurrentTile.name, tsCurrentTile.textureBounds,
                        new Rectangle(currentMousePosition.ToPoint(), tsCurrentTile.screenBounds.Size));

                        tiles.Add(newTile);
                    }


                }
                else
                {
                    drawTileSelectionCurrentTileOnMouse = false;
                }
            }
        }

        public void UpdateKeyInput()
        {
            // Move and improve this code.
            if (currentTile != null)
            {
                if (InputManager.OnKeyPressed(Keys.Right))
                {
                    drawCurrentTileMarker = false;
                    drawHoveredTileMarker = false;

                    currentTile.screenBounds.Location += new Point(1, 0);
                    currentTileInfo = currentTile.ToString();
                }
                else if (InputManager.OnKeyPressed(Keys.Left))
                {
                    drawCurrentTileMarker = false;
                    drawHoveredTileMarker = false;

                    currentTile.screenBounds.Location += new Point(-1, 0);
                    currentTileInfo = currentTile.ToString();

                }
                else if (InputManager.OnKeyPressed(Keys.Up))
                {
                    drawCurrentTileMarker = false;
                    drawHoveredTileMarker = false;

                    currentTile.screenBounds.Location += new Point(0, -1);
                    currentTileInfo = currentTile.ToString();

                }
                else if (InputManager.OnKeyPressed(Keys.Down))
                {
                    drawCurrentTileMarker = false;
                    drawHoveredTileMarker = false;

                    currentTile.screenBounds.Location += new Point(0, 1);
                    currentTileInfo = currentTile.ToString();

                }
                else if (InputManager.OnKeyPressed(Keys.Delete))
                {
                    tiles.Remove(currentTile);
                    currentTile = null;
                }
                else if (InputManager.AreAllKeysPressed(Keys.LeftControl, Keys.C))
                {
                    // TODO: Store currently selected Tiles in copyTilesBuffer(Override previous buffer contents).
                }
                else if (InputManager.AreAllKeysPressed(Keys.LeftControl, Keys.V))
                {
                    // TODO: Place Tiles at Mouse Position that are currently in copyTilesBuffer.
                    //       Every Tile needs to be offset with it's previous position from the 
                    //       Mouse Position. Otherwise all Tiles are put to the same position.
                }
                else if (InputManager.AreAllKeysPressed(Keys.LeftControl, Keys.Z))
                {
                    // TODO: Create a notion of a previous State and restore that state here.
                }
            }
        }

        public void UpdateHoveringAndMovingTilesWithMouse()
        {
            // Check for hovering and moving Tiles with Mouse.
            if (!tileSelection.IsHoveredByMouse &&
                !drawTileSelectionCurrentTileOnMouse &&
                InputManager.HasMouseMoved)
            {
                if (!moveTileHoveredByMouse)
                {
                    // Check for Mouse-Hover on one of the already placed Tiles in the DrawingArea and
                    // and detecting if the hovered Tile needs to be moved around.
                    foreach (Tile tile in tiles)
                    {
                        tileHoveredByMouse = null;

                        if (tile.screenBounds.Contains(currentMousePosition))
                        {
                            drawHoveredTileMarker = true;
                            tileHoveredByMouse = tile;

                            break;
                        }
                    }
                }

                // Hide the tileHoveredByMouseMarker when no Tile is hovered.
                if (tileHoveredByMouse == null)
                {
                    drawHoveredTileMarker = false;
                }

                // During moving the hovered Tile.
                //if (moveTileHoveredByMouse)
                //{
                //    // TODO: Move Tile in broader steps.

                //    tileHoveredByMouse.screenBounds.Location += mouseTravel.ToPoint();
                //    currentTileInfo = currentTile.ToString();
                //}
            }

            if ((tileHoveredByMouse != null) && InputManager.OnLeftMouseButtonClicked())
            {
                currentTile = tileHoveredByMouse;

                drawCurrentTileMarker = !drawCurrentTileMarker;
                drawHoveredTileMarker = false;
                moveTileHoveredByMouse = true;
                currentTileInfo = currentTile.ToString();
            }
            else if (moveTileHoveredByMouse && InputManager.OnLeftMouseButtonReleased())
            {
                moveTileHoveredByMouse = false;

                // Only restore the currentTileMarker if the Tile was actually moved.
                if (mouseTravel != Vector2.Zero)
                {
                    drawCurrentTileMarker = true;
                }
            }
        }

        public void UpdateRectangleSelection()
        {
            if (tileSelection.IsHoveredByMouse)
                return;

            // Is there a rectangleSelection that we can manipulate ?
            if (selectionRectangle.Contains(currentMousePosition) && InputManager.IsLeftMouseButtonDown())
            {
                // Move all Tiles in rectangleSelection by mouseTravel.
                foreach (Tile tile in rectangleSelection)
                {
                    tile.screenBounds.Location += mouseTravel.ToPoint();
                }
            }

            // Begin drawing selectionRectangle
            else if (InputManager.OnLeftMouseButtonClicked())
            {
                selectionRectangleStartPoint = currentMousePosition;
                selectionRectangleHasStartPoint = true;
            }

            // End drawing selectionRectangle and find out which Tiles were selected.
            else if (InputManager.OnLeftMouseButtonReleased())
            {
                // Points for selectionBoundingBox.
                Vector2 topLeft = new Vector2(float.MaxValue, float.MaxValue);
                Vector2 bottomRight = new Vector2(float.MinValue, float.MinValue);

                // Clear old selection and check which Tiles on DrawingArea are in selectionRectangle
                rectangleSelection.Clear();
                foreach (Tile tile in tiles)
                {
                    if (selectionRectangle.Contains(tile.screenBounds))
                    {
                        rectangleSelection.Add(tile);
                    }

                    // Find out selectionBoundingBox.
                    Vector2 temp = tile.screenBounds.Location.ToVector2();

                    if (temp.X < topLeft.X)
                    {
                        topLeft.X = temp.X;
                    }
                    if (temp.Y < topLeft.Y)
                    {
                        topLeft.Y = temp.Y;
                    }

                    if (temp.X > bottomRight.X)
                    {
                        bottomRight.X = temp.X;
                    }
                    if (temp.Y > bottomRight.Y)
                    {
                        bottomRight.Y = temp.Y;
                    }
                }

                selectionBoundingBox.Location = topLeft.ToPoint();
                selectionBoundingBox.Size = (bottomRight - topLeft).ToPoint();

                //selectionRectangleStartPoint = Vector2.Zero;
                //selectionRectangleHasStartPoint = false;
            }

            // While holding LeftMouseButton, calculate the selectionRectangle from the initial
            // Point where the LeftMouseButton was pushed down to the currentMousePosition.
            if (selectionRectangleHasStartPoint && InputManager.IsLeftMouseButtonDown())
            {

                // TODO: Verstehen.
                selectionRectangle.Width = (int)Math.Abs(currentMousePosition.X - selectionRectangleStartPoint.X);
                selectionRectangle.Height = (int)Math.Abs(currentMousePosition.Y - selectionRectangleStartPoint.Y);
                selectionRectangle.X = (int)Math.Min(selectionRectangleStartPoint.X, currentMousePosition.X);
                selectionRectangle.Y = (int)Math.Min(selectionRectangleStartPoint.Y, currentMousePosition.Y);
            }
        }

        public void Update(GameTime gameTime)
        {
            currentMousePosition = InputManager.CurrentMousePosition();
            mouseTravel = currentMousePosition - InputManager.PreviousMousePosition();

            // Check for everything that only needs to happen when the Mouse is inside
            // the DrawingArea's bounds.
            if (bounds.Contains(currentMousePosition))
            {
                UpdateTilePlacing();
                UpdateHoveringAndMovingTilesWithMouse();
                UpdateKeyInput();
                UpdateRectangleSelection();
            }

            // When the Mouse is not inside the DrawingArea's bounds, 
            // we don't even want to look if the TileSelection has a
            // currentTile that we could draw at the Mouse's position.
            else
            {
                drawTileSelectionCurrentTileOnMouse = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Primitives2D.DrawRectangle(spriteBatch, bounds, Color.DarkRed);

            foreach (Tile tile in tiles)
            {
                destinationRectangle.Location = tile.screenBounds.Location;
                destinationRectangle.Size = tile.screenBounds.Size;

                spriteBatch.Draw(tileSelection.TileSet, destinationRectangle, tile.textureBounds, Color.White);
            }

            if (drawHoveredTileMarker && tileHoveredByMouse != null)
            {
                Primitives2D.DrawRectangle(spriteBatch, tileHoveredByMouse.screenBounds, Color.AliceBlue, 5);
            }

            if (drawCurrentTileMarker && currentTile != null)
            {
                Primitives2D.DrawRectangle(spriteBatch, currentTile.screenBounds, Color.DarkRed, 5);
            }

            if (drawTileSelectionCurrentTileOnMouse)
            {
                spriteBatch.Draw(tileSelection.TileSet,
                new Rectangle(currentMousePosition.ToPoint(), tileSelection.CurrentTile.screenBounds.Size),
                tileSelection.CurrentTile.textureBounds, Color.White);
            }

            // Draw currentTileInfo
            if (currentTile != null)
            {
                spriteBatch.DrawString(font, currentTileInfo, new Vector2((bounds.X + bounds.Width) * 0.25f, bounds.Top) , Color.White);
            }

            // Mark all Tiles in rectangleSelection.
            if (rectangleSelection.Count != 0)
            {
                //foreach (Tile tile in rectangleSelection)
                //{
                //    Primitives2D.DrawRectangle(spriteBatch, tile.screenBounds, Color.DarkRed, 5);
                //}

                Primitives2D.DrawRectangle(spriteBatch, selectionBoundingBox, Color.DarkRed, 5);
            }

            // Draw selectionRectangle
            if (selectionRectangleHasStartPoint)
            {
                Color darkRed = Color.Blue;
                darkRed.A = 10;

                Primitives2D.FillRectangle(spriteBatch, selectionRectangle, darkRed);
            }

            spriteBatch.End();
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
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
