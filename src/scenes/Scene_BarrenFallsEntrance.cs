using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.input;

namespace EVCMonoGame.src.scenes
{
    public class Scene_BarrenFallsEntrance : Scene
    {
        private Rectangle doorArea = new Rectangle(800, 370, 1110, 650);

        public Scene_BarrenFallsEntrance(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/tilemaps/other/barrenFallsEntrance.tm.txt");

            sora.WorldPosition = new Vector2(820, 1020);
            riku.WorldPosition = new Vector2(1080, 1020);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Go to Barren Falls
            if ((CollisionManager.IsPlayerInArea(PlayerIndex.One, doorArea) && InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) ||
                CollisionManager.IsPlayerInArea(PlayerIndex.Two, doorArea) && InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two)))
            {
                sceneManager.SceneTransition(EScene.BARREN_FALLS);
            }
        }
    }
}
