
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.utility;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.tilemap
{
    // TODO: Maybe rethink Tilemap to work better with TilemapEditor.
    // TODO: Make Tilemap able to load the 'tilemap.ts.txt' FileFormat.

    public class Tilemap : scenes.IDrawable
    {
        private List<Tile> tiles;
        private Texture2D tileSet = null;
        private String tileSetPath = String.Empty;
        private Vector2 position;
        List<GeometryBox> collisionBoxes = new List<GeometryBox>();

        public List<GeometryBox> CollisionBoxes
        {
            get { return collisionBoxes; }
        }

        public Tilemap(Vector2 position, String tilemapFile)
        {
            this.position = position;
            ReadTilemapFile(tilemapFile);
        }

        public Tilemap(Vector2 position)
        {
            this.position = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tileSet, tile.screenBounds, tile.textureBounds, Color.White);
            }
        }

        public void LoadContent(ContentManager content)
        {
            tileSet = content.Load<Texture2D>(tileSetPath);
        }

        public void ReadTilemapFile(String path)
        {
            if (!path.EndsWith(".tm.txt"))
            {
                throw new ArgumentException("Given file '" + path + "' is not an tm(Tilemap)File.\n" +
                    "Provide a file that ends with '.tm.txt'.");
            }

            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            String line = String.Empty;

            // Variables for things that will be read.
            String tileSetPath = String.Empty;
            String tileName = String.Empty;
            Rectangle textureBounds = Rectangle.Empty;
            Rectangle screenBounds = Rectangle.Empty;
            List<Tile> tiles = new List<Tile>();
            List<GeometryBox> collisionBoxes = new List<GeometryBox>();

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
                        line = line.Remove(0, 15); // Remove 'TEXTURE_BOUNDS='
                        textureBounds = Utility.StringToRectangle(line);

                        line = Utility.ReplaceWhitespace(reader.ReadLine(), "");
                        line = line.Remove(0, 14); // Remove 'SCREEN_BOUDNDS='
                        screenBounds = Utility.StringToRectangle(line);

                        tiles.Add(new Tile(tileName, textureBounds, screenBounds));
                    }
                    else if(line.Contains("COLLISION_BOX"))
                    {
                        line = Utility.ReplaceWhitespace(reader.ReadLine(), ""); // Remove Whitespace
                        line = line.Remove(0, 17); // Remove 'COLLISION_BOUNDS='
                        collisionBoxes.Add(new GeometryBox(Utility.StringToRectangle(line)));
                    }
                }
                else if (line.Contains("TILESET"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    tileSetPath = line.Substring(8); // Read everything after 'TILESET='
                }
            }

            this.tiles = tiles;
            this.collisionBoxes = collisionBoxes;
            this.tileSetPath = tileSetPath;

            reader.Close();
        }
    }
}
