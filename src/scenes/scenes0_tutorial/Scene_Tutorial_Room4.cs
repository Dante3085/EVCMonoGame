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
    public class Scene_Tutorial_Room4 : Scene
    {
        private Defender defender;

        public Scene_Tutorial_Room4(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room4.tm.txt");

            sora.WorldPosition = new Vector2(474, 1430);
            riku.WorldPosition = new Vector2(1682, 1422);

            doorPlayerOne = new Door(new Vector2(494, 100));
            doorPlayerTwo = new Door(new Vector2(1712, 104));

            defender = new Defender(new Vector2(1712, 900));

            updateables.AddRange(new IUpdateable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
                defender,
            });

            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
                defender,
            });

            camera.Zoom = 0.7f;
            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (doorPlayerOne.Open && doorPlayerTwo.Open && !defender.IsAlive)
            //{
            //    sceneManager.SceneTransition(EScene.TUTORIAL_ROOM_5);
            //}

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
