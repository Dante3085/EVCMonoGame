using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;


namespace EVCMonoGame.src.scenes.castle
{
    public class Scene_Castle_Room4 : Scene
    {

        public Scene_Castle_Room4(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
               "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room4.tm.txt");

            sora.WorldPosition = new Vector2(565, 6400);
            riku.WorldPosition = new Vector2(1900, 6400);
        }
    }
}
