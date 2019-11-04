
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
        private Rectangle tileHoveredByMouseMarker       = Rectangle.Empty;
        private TileSelection tileSelection;

        private bool isAnyTileHoveredByMouse             = false;
        private bool moveTileHoveredByMouse              = false;

        private Vector2 mouseTravel                      = Vector2.Zero;

        private SpriteFont font;
        private String currentTileInfo                   = String.Empty;

        private Tile currentTile                         = null;
        private Rectangle currentTileMarker              = Rectangle.Empty;

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

        public void Update(GameTime gameTime)
        {
            currentMousePosition = InputManager.CurrentMousePosition();

            bool tileSelectionHasCurrentTile = tileSelection.CurrentTile != null;
            bool tileSelectionIsHoveredByMouse = tileSelection.IsHoveredByMouse;

            // Move and improve this code.
            if (currentTile != null)
            {

                if (InputManager.OnKeyPressed(Keys.Right))
                {
                    currentTileMarker = Rectangle.Empty;
                    currentTile.screenBounds.Location += new Point(1, 0);

                    currentTileInfo = currentTile.ToString();
                }
                else if (InputManager.OnKeyPressed(Keys.Left))
                {
                    currentTileMarker = Rectangle.Empty;
                    currentTile.screenBounds.Location += new Point(-1, 0);

                    currentTileInfo = currentTile.ToString();

                }
                else if (InputManager.OnKeyPressed(Keys.Up))
                {
                    currentTileMarker = Rectangle.Empty;
                    currentTile.screenBounds.Location += new Point(0, -1);

                    currentTileInfo = currentTile.ToString();

                }
                else if (InputManager.OnKeyPressed(Keys.Down))
                {
                    currentTileMarker = Rectangle.Empty;
                    currentTile.screenBounds.Location += new Point(0, 1);

                    currentTileInfo = currentTile.ToString();

                }
            }

            // Check for everything that only needs to happen when the Mouse is inside
            // the DrawingArea's bounds.
            if (bounds.Contains(currentMousePosition))
            {
                // Check for drawing the currentTile of the TileSelection on the Mouse and
                // placing the currentTile of TileSelection in the DrawingArea.
                if (tileSelectionHasCurrentTile)
                {
                    if (!tileSelectionIsHoveredByMouse)
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

                // Check for hovering and moving Tiles with Mouse.
                if (!tileSelectionIsHoveredByMouse && 
                    !drawTileSelectionCurrentTileOnMouse &&
                    InputManager.HasMouseMoved)
                {
                    if (!moveTileHoveredByMouse)
                    {
                        // Check for Mouse-Hover on one of the already placed Tiles in the DrawingArea and
                        // and detecting if the hovered Tile needs to be moved around.
                        foreach (Tile tile in tiles)
                        {
                            isAnyTileHoveredByMouse = false;
                            tileHoveredByMouse = null;

                            if (tile.screenBounds.Contains(currentMousePosition))
                            {
                                isAnyTileHoveredByMouse = true;
                                tileHoveredByMouseMarker = tile.screenBounds;
                                tileHoveredByMouse = tile;

                                break;
                            }
                        }
                    }

                    // Hide the tileHoveredByMouseMarker when no Tile is hovered.
                    if (!isAnyTileHoveredByMouse)
                    {
                        tileHoveredByMouseMarker = Rectangle.Empty;
                    }

                    // During moving the hovered Tile.
                    if (moveTileHoveredByMouse)
                    {
                        mouseTravel = currentMousePosition - InputManager.PreviousMousePosition();

                        //if (mouseTravel.Length() > 50)
                        //{
                        //    tileHoveredByMouse.screenBounds.Location += (mouseTravel).ToPoint();
                        //    mouseTravel = Vector2.Zero;
                        //}

                        tileHoveredByMouse.screenBounds.Location += mouseTravel.ToPoint();
                        currentTileMarker.Location += mouseTravel.ToPoint();

                        currentTileInfo = currentTile.ToString();
                    }
                }

                // Start moving the hovered Tile.
                if (isAnyTileHoveredByMouse && InputManager.OnLeftMouseButtonClicked())
                {
                    moveTileHoveredByMouse = true;
                    tileHoveredByMouseMarker = Rectangle.Empty;

                    currentTile = tileHoveredByMouse;
                    currentTileMarker = currentTile.screenBounds;

                    currentTileInfo = currentTile.ToString();
                }

                // Stop moving the hovered Tile.
                else if (moveTileHoveredByMouse && InputManager.OnLeftMouseButtonReleased())
                {
                    moveTileHoveredByMouse = false;
                    tileHoveredByMouseMarker = tileHoveredByMouse.screenBounds;
                }
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

            Primitives2D.DrawRectangle(spriteBatch, tileHoveredByMouseMarker, Color.AliceBlue, 5);
            Primitives2D.DrawRectangle(spriteBatch, currentTileMarker, Color.DarkRed, 5);

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
