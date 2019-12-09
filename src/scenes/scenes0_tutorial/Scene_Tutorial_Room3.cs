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

namespace EVCMonoGame.src.scenes.tutorial
{
    public class Scene_Tutorial_Room3 : Scene
    {
        private Shadow[] shadows;

        public Scene_Tutorial_Room3(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero, 
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room3.tm.txt");

            sora.WorldPosition = new Vector2(1356, 4265);
            riku.WorldPosition = new Vector2(2069, 4376);

            shadows = new Shadow[10];
            for (int i = 0; i < shadows.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    shadows[i] = new Shadow(new Vector2(1200, 2500 + 100*i));
                }
                else
                {
                    shadows[i] = new Shadow(new Vector2(2100, 2500 + 100 * i));
                }

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            doorPlayerOne = new Door(new Vector2(1550, 801));
            doorPlayerTwo = new Door(new Vector2(2000, 805));

            updateables.AddRange(new IUpdateable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            camera.Zoom = 0.7f;
            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (doorPlayerOne.Open && doorPlayerTwo.Open /* &&
            //    !shadow1.IsAlive && !shadow2.IsAlive */)
            //{
            //    sceneManager.SceneTransition(EScene.TUTORIAL_ROOM_4);
            //}

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
