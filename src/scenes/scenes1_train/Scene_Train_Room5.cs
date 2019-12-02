using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.states;
using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.scenes.train
{
    public class Scene_Train_Room5 : Scene
    {
        private Shadow shadow1;
        private Shadow shadow2;

        public Scene_Train_Room5(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes1_train/room5.tm.txt");

            sora.WorldPosition = new Vector2(323, 896);
            riku.WorldPosition = new Vector2(330, 2600);

            // camera.DynamicZoomActivated = false;
            // camera.Zoom = camera.WideZoom;

            shadow1 = new Shadow(new Vector2(400, 896));
            shadow2 = new Shadow(new Vector2(400, 2600));

            updateables.AddRange(new IUpdateable[]
            {
                shadow1,
                shadow2,
            });

            drawables.AddRange(new IDrawable[]
            {
                shadow1,
                shadow2,
            });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!shadow1.IsAlive && !shadow2.IsAlive)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
