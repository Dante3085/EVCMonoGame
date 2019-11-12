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
    // TODO: Make it impossible for the TileSelection to go outside of the screen.
    // TODO: Clear tileHoveredByMouseMarker if no Tile is marked(Like in DrawingArea).

    // TODO: When moving the TileSelection around, disable the tileHoveredByMouseMarker so that 
    //       that it doesn't flicker when the Mouse moves violently.

    // TODO: Make it possible to be able to resize the TileSelection during runtime by scaling the bounds rectangle.
    //       This would increase/decrease the number of Tiles on one row.

    public class TileSelection
    {
        #region Fields

        private Rectangle bounds;
        private Vector2 tileSize;
        private int numTilesPerRow;
        private Texture2D tileSet       = null;
        private String tileSetName      = String.Empty;
        private List<List<Tile>> tiles  = new List<List<Tile>>();
        private Tile currentTile        = null;
        private Tile tileHoveredByMouse = null;                 
        private Vector2 tileSpacing;

        private SpriteFont font;
        private Vector2 fontSize;
        private String text;

        private Rectangle tileHoveredByMouseMarker;
        private Rectangle currentTileMarker = Rectangle.Empty;

        private bool moveTileSelection           = false;
        private bool movementLocked              = false;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return bounds.Location.ToVector2(); }
            set 
            {
                bounds.Location = value.ToPoint();
                UpdateTileBounds();
            }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
        }

        public Tile CurrentTile
        {
            get { return currentTile; }
        }

        public Texture2D TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }

        public List<List<Tile>> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        public bool IsHoveredByMouse
        {
            get; private set;
        }

        #endregion

        public TileSelection
        (
            Vector2 position, 
            Vector2 tileSize, 
            int numTilesPerRow, 
            Vector2 tileSpacing
        )
        {
            this.tileSize = tileSize;
            this.numTilesPerRow = numTilesPerRow;
            this.tileSpacing = tileSpacing;

            bounds = new Rectangle(position.ToPoint(), Point.Zero);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 currentMousePosition = InputManager.CurrentMousePosition();

            bool breakOuterLoop = false;
            if (bounds.Contains(currentMousePosition))
            {
                IsHoveredByMouse = true;

                if (InputManager.OnKeyPressed(Keys.M))
                {
                    movementLocked = movementLocked ? false : true;
                    text = "Tile-Selection\nMovementLocked: " + movementLocked.ToString();
                }

                // Only check for which Tile is hovered by Mouse when
                // the Mouse is inside the TileSelection's bounds and
                // it has moved.
                if (InputManager.HasMouseMoved)
                {
                    foreach (List<Tile> rows in tiles)
                    {
                        foreach (Tile tile in rows)
                        {
                            if (tile.screenBounds.Contains(currentMousePosition))
                            {
                                tileHoveredByMouseMarker = tile.screenBounds;
                                tileHoveredByMouse = tile;

                                // Break both loops if we found a Tile that the mouse currently hovers.
                                breakOuterLoop = true;
                                break;
                            }
                        }

                        if (breakOuterLoop)
                        {
                            break;
                        }
                    }
                }

                // Check for tileHoveredByMouse becoming the currentTile.
                if (InputManager.OnLeftMouseButtonClicked())
                {
                    currentTileMarker = tileHoveredByMouseMarker;
                    currentTile = tileHoveredByMouse;

                    // This is for moving the TileSelection with the Mouse.
                    moveTileSelection = movementLocked ? false : true;
                }

                // Check for the currentTile being discarded.
                else if (InputManager.OnRightMouseButtonClicked())
                {
                    currentTileMarker = Rectangle.Empty;
                    currentTile = null;
                }
            }
            else
            {
                IsHoveredByMouse = false;
            }

            // Check for moving the TileSelection around.
            // We do this when the LeftMouseButton was once
            // down inside the TileSelection's bounds and is 
            // still down.
            // We only stop moving the TileSelection if the 
            // LeftMouseButton is released again.
            if (!movementLocked && moveTileSelection)
            {
                if (InputManager.OnLeftMouseButtonReleased())
                {
                    moveTileSelection = false;
                }
                else
                {
                    Position += currentMousePosition - InputManager.PreviousMousePosition();
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO: Converting from Point to Vector2 and vice versa all the time is kinda stupid :|

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Draw all Tiles
            foreach (List<Tile> rows in tiles)
            {
                foreach (Tile tile in rows)
                {
                    spriteBatch.Draw(tileSet, tile.screenBounds, tile.textureBounds, Color.White);
                }
            }

            // Draw TileSelection String, TileSelection's bounds, tileHoveredByMouseMarker and currentTileMarker.
            spriteBatch.DrawString(font, text, bounds.Location.ToVector2(), Color.White);
            Primitives2D.DrawRectangle(spriteBatch, bounds, Color.Green, 5);
            Primitives2D.DrawRectangle(spriteBatch, tileHoveredByMouseMarker, Color.AliceBlue, 5);
            Primitives2D.DrawRectangle(spriteBatch, currentTileMarker, Color.DarkRed, 5);

            spriteBatch.End();
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            text = "Tile-Selection\nMovementLocked: false";
            fontSize = font.MeasureString(text);

            // We call these things here because they are dependant on fontSize.
            UpdateBounds();

            tileHoveredByMouse = tiles[0][0];
            UpdateTileBounds();
            tileHoveredByMouseMarker = tiles[0][0].screenBounds;
        }

        public void ReadTilesFromFile(String file, ContentManager content)
        {
            TileSelectionData tileSelectionData = ConfigFileUtility.ReadTileSelectionFile(file, numTilesPerRow);
            tiles = tileSelectionData.tiles;
            tileSet = content.Load<Texture2D>(tileSelectionData.tileSetName);
        }

        private void UpdateBounds()
        {
            int widthFirstRow = (int)(tiles[0].Count * tileSize.X + (tiles[0].Count - 1) * tileSpacing.X);

            bounds.Width = fontSize.X > widthFirstRow ? (int)fontSize.X : widthFirstRow;
            bounds.Height = (int)(fontSize.Y + (tiles.Count * tileSize.Y + (tiles.Count - 1) * tileSpacing.Y));

            // width and height of bounds without text.
            //bounds.Width  = widthFirstRow;
            //bounds.Height = (int)(tiles.Count * tileSize.Y + (tiles.Count - 1) * tileSpacing.Y); 
        }

        private void UpdateTileBounds()
        {
            for (int i = 0; i < tiles.Count; ++i)
            {
                for (int j = 0; j < tiles[i].Count; ++j)
                {
                    tiles[i][j].screenBounds = new Rectangle((bounds.Location.ToVector2() + new Vector2(j * tileSize.X + j * tileSpacing.X,
                                                     i * tileSize.Y + i * tileSpacing.Y + fontSize.Y)).ToPoint(), tileSize.ToPoint());
                }
            }

            tileHoveredByMouseMarker = tileHoveredByMouse.screenBounds;

            if (currentTile != null)
            {
                currentTileMarker = currentTile.screenBounds;
            }
        }
    }
}
