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
    // TODO: Nach vorgeschriebener Zeit in 2.te Szene mit Zug.

    public class Scene_Train_Room1 : Scene
    {
        private Shadow[] shadows;

        public Scene_Train_Room1(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes1_train/room1.tm.txt");

            sora.WorldPosition = new Vector2(5900, 2200);
            riku.WorldPosition = new Vector2(6250, 2200);

            shadows = new Shadow[10];
            for (int i = 0; i < shadows.Length; ++i)
            {
                shadows[i] = new Shadow(new Vector2(4000 + i * 100, 2200));

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (sora.WorldPosition.X > ? && riku.WorldPosition > ?)
            //{
            //    sceneManager.SceneTransitionNextRoom();
            //}

            if(shadows.All((s) => !s.IsAlive))
            {
                sceneManager.SceneTransitionNextRoom();
                // sceneManager.SceneTransition(EScene.TRAIN_ROOM_2);
            }
        }
    }
}
