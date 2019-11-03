using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.input;
using EVCMonoGame.src.gui;

namespace EVCMonoGame.src.tilemap.tilemapEditor
{
    // TODO: Load TileSet and possibly other content in TilemapEditor class.
    // TODO: Properly implement Button.

    // TODO: Tile being a class instead of a struct could create problems. 
    //       Struct value type might be smarter if we need to pass Tiles around.

    // TODO: Create FileFormat that can store the information created in the TilemapEditor.
    //       This format also has to be readable by a Tilemap instance.

    // TODO: Create SaveAs Button that opens a FileDialog on being pressed.
    //       After choosing a name and location for the file that shall contain
    //       the current information of the TilemapEditor, the TilemapEditor creates
    //       that file in the previously mentioned FileFormat.


    public class Tile
    {
        public String name;
        public Rectangle textureBounds;
        public Rectangle screenBounds;

        public Tile(String name, Rectangle textureBounds, Rectangle screenBounds)
        {
            this.name = name;
            this.textureBounds = textureBounds;
            this.screenBounds = screenBounds;
        }

        public Tile(Tile otherTile)
        {
            this.name = otherTile.name;
            this.textureBounds = otherTile.textureBounds;
            this.screenBounds = otherTile.screenBounds;
        }

        public override string ToString()
        {
            return "TILE{" + name + ", " + Utility.RectangleToString(textureBounds) + ", " + 
                   Utility.RectangleToString(screenBounds) + "}"; 
        }
    }

    // (Moritz): Ich habe den TilemapEditor extra nicht von scenes.Updateable erben und scenes.IDrawable implementieren lassen,
    //           weil er im Hauptmenü durch den TilemapEditorState erreichbar sein soll. Die Klassen im states Ordner haben nichts
    //           mit den Klassen in unserem scenes Ordner zu tun.
    public class TilemapEditor
    {
        private TileSelection tileSelection;
        private DrawingArea drawingArea;

        public TilemapEditor()
        {
            tileSelection = new TileSelection(new Vector2(0, 0), new Vector2(100, 100), 3, new Vector2(1, 1),
                               "Content/rsrc/tilesets/configFiles/tilemapEditor_tileSelectionTiles.txt");
        }

        public void Update(GameTime gameTime)
        {
            tileSelection.Update(gameTime);
            drawingArea.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            drawingArea.Draw(gameTime, spriteBatch);
            tileSelection.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content, Viewport viewport)
        {
            drawingArea = new DrawingArea(viewport.Bounds, tileSelection);

            tileSelection.LoadContent(content);
            drawingArea.LoadContent(content);
        }
    }
}
