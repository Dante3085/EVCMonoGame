using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using C3.MonoGame;

using EVCMonoGame.src.input;

namespace EVCMonoGame.src.tilemap.tilemapEditor
{
    // TODO: Tile being a class instead of a struct could create problems. Struct value type might be smarter if we need to pass Tiles around.
    // TODO: Solution for moving the TileSelection with Mouse being annoying: Lock/Unlock moving on leftMouseDoubleClick.

    public class TileSelection
    {
        class Tile
        {
            public String name;
            public Rectangle textureBounds;
            public Rectangle bounds;

            public Tile(String name, Rectangle textureBounds, Rectangle bounds)
            {
                this.name = name;
                this.textureBounds = textureBounds;
                this.bounds = bounds;
            }

            public override string ToString()
            {
                return "TILE{" + name + ", (" + textureBounds.X + ", " + textureBounds.Y + ", " +
                                              textureBounds.Width + ", " + textureBounds.Height + ")";
            }
        }

        #region Fields

        private Rectangle bounds;
        private Vector2 tileSize;
        private int numTilesPerRow;
        private Texture2D tileSet      = null;
        private String tileSetName     = String.Empty;
        private List<List<Tile>> tiles = new List<List<Tile>>();
        private Tile currentTile       = null;
        private Tile selectorTile      = null;                 // Reference to Tile that is selected by currentTileSelector.
        private Vector2 tileSpacing;

        private SpriteFont font;
        private Vector2 fontSize;
        private String text;

        // TODO: currentTileSelector und currentTileMarker sind keine eindeutigen Begriffe.
        private Rectangle currentTileSelector;                 // Shows which Tile can become the currentTile.
        private Rectangle currentTileMarker = Rectangle.Empty; // Shows which Tile is the currentTile.

        private bool leftMouseDown = false;

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

        #endregion

        public TileSelection(Vector2 position, Vector2 tileSize, int numTilesPerRow, Vector2 tileSpacing, String tileSelectionFile)
        {
            this.tileSize = tileSize;
            this.numTilesPerRow = numTilesPerRow;
            this.tileSpacing = tileSpacing;

            bounds = new Rectangle(position.ToPoint(), Point.Zero);
            tiles.Add(new List<Tile>());

            ReadTilesFromFile(tileSelectionFile);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 currentMousePosition = InputManager.CurrentMousePosition();

            bool breakOuterLoop = false;
            if (bounds.Contains(currentMousePosition))
            {
                if (InputManager.HasMouseMoved)
                {
                    // Check which Tile is hovered by mouse
                    foreach (List<Tile> rows in tiles)
                    {
                        foreach (Tile tile in rows)
                        {
                            if (tile.bounds.Contains(currentMousePosition))
                            {
                                currentTileSelector = tile.bounds;
                                selectorTile = tile;

                                // break both loops if we found a Tile that the mouse currently hovers.
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

                if (InputManager.OnLeftMouseButtonClicked())
                {
                    currentTileMarker = currentTileSelector;
                    currentTile = selectorTile;

                    leftMouseDown = true;
                }
                else if (InputManager.OnRightMouseButtonClicked())
                {
                    currentTileMarker = Rectangle.Empty;
                    currentTile = null;
                }
            }

            // This is for moving the TileSelection with the Mouse.
            // It still can be annoying, so it's commented out for now.
            if (leftMouseDown)
            {
                if (InputManager.OnLeftMouseButtonReleased())
                {
                    leftMouseDown = false;
                }

                Position += currentMousePosition - InputManager.PreviousMousePosition();
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
                    spriteBatch.Draw(tileSet, tile.bounds, tile.textureBounds, Color.White);
                }
            }

            // Draw other stuff.
            spriteBatch.DrawString(font, text, bounds.Location.ToVector2(), Color.White);
            // Primitives2D.DrawRectangle(spriteBatch, bounds, Color.Red, 1);
            Primitives2D.DrawRectangle(spriteBatch, currentTileSelector, Color.AliceBlue, 5);
            Primitives2D.DrawRectangle(spriteBatch, currentTileMarker, Color.DarkRed, 5);

            spriteBatch.End();
        }

        public void LoadContent(ContentManager content)
        {
            tileSet = content.Load<Texture2D>(tileSetName);

            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            text = "Tile-Selection";
            fontSize = font.MeasureString(text);

            // We call these things here because they are dependant on fontSize.
            UpdateBounds();
            selectorTile = tiles[0][0];
            UpdateTileBounds();
            currentTileSelector = tiles[0][0].bounds;
        }

        private void ReadTilesFromFile(String file)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(file);
            String line = String.Empty;

            int numTilesInLastRow = 0;

            // Variables for storing the information that will be read.
            String tileName = String.Empty;
            Rectangle tileBounds = Rectangle.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                // Find section
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // Determine specific section
                    if (line.Contains("TILE"))
                    {
                        line = Utility.ReplaceWhitespace(reader.ReadLine(), ""); // Remove Whitespace
                        tileName = line.Remove(0, 5); // Remove 'NAME='

                        line = Utility.ReplaceWhitespace(reader.ReadLine(), "");
                        line = line.Remove(0, 7); // Remove 'BOUNDS='
                        tileBounds = Utility.StringToRectangle(line);

                        if (numTilesInLastRow == numTilesPerRow)
                        {
                            tiles.Add(new List<Tile>());
                            numTilesInLastRow = 0;
                        }
                        tiles[tiles.Count - 1].Add(new Tile(tileName, tileBounds, Rectangle.Empty));
                        ++numTilesInLastRow;
                    }
                }
                else if (line.Contains("TILESET"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    tileSetName = line.Substring(8); // Read everything after 'TILESET='
                }
            }
            reader.Close();
        }

        private void UpdateBounds()
        {
            int widthFirstRow = (int)(tiles[0].Count * tileSize.X + (tiles[0].Count - 1) * tileSpacing.X);

            bounds.Width = fontSize.X > widthFirstRow ? (int)fontSize.X : widthFirstRow;
            bounds.Height = (int)(fontSize.Y + (tiles.Count * tileSize.Y + (tiles.Count - 1) * tileSpacing.Y));
        }

        private void UpdateTileBounds()
        {
            for (int i = 0; i < tiles.Count; ++i)
            {
                for (int j = 0; j < tiles[i].Count; ++j)
                {
                    tiles[i][j].bounds = new Rectangle((bounds.Location.ToVector2() + new Vector2(j * tileSize.X + j * tileSpacing.X,
                                                     i * tileSize.Y + i * tileSpacing.Y + fontSize.Y)).ToPoint(), tileSize.ToPoint());
                }
            }

            currentTileSelector = selectorTile.bounds;

            if (currentTile != null)
            {
                currentTileMarker = currentTile.bounds;
            }
        }
    }
}
