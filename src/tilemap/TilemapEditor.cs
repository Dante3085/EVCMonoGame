using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.input;

namespace EVCMonoGame.src.tilemap
{
    // (Moritz): Ich habe den TilemapEditor extra nicht von scenes.Updateable erben und scenes.IDrawable implementieren lassen,
    //           weil er im Hauptmenü durch den TilemapEditorState erreichbar sein soll. Die Klassen im states Ordner haben nichts
    //           mit den Klassen in unserem scenes Ordner zu tun.
    public class TilemapEditor
    {
        private TileSelection tileSelection;

        public TilemapEditor()
        {
            tileSelection = new TileSelection(new Vector2(0, 0), new Vector2(64, 64), 3, new Vector2(1, 1),
                               "Content/rsrc/tilesets/configFiles/tilemapEditor_tileSelectionTiles.txt");
        }

        public void Update(GameTime gameTime)
        {
            //if (InputManager.HasMouseMoved)
            //{
            //    tileSelection.Position = InputManager.CurrentMousePosition();
            //}

            tileSelection.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tileSelection.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            tileSelection.LoadContent(content);
        }
    }
}
