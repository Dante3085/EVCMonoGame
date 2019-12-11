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
        private Defender[] defenders;

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

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(715, 2345),
                new Vector2(1045, 2210),
                new Vector2(1380, 2116),
                new Vector2(1722, 2250),
                new Vector2(1920, 2030),
                new Vector2(2012, 2255),
                new Vector2(2275, 2100),
                new Vector2(2570, 2190),
                new Vector2(2862, 2106),
                new Vector2(3140, 2150),
                new Vector2(4293, 2190),
                new Vector2(3540, 2166),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[5];
            for (int i = 0; i < shadows.Length; ++i)
            {
                shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane());

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            defenders = new Defender[3];
            for (int i = 0; i < defenders.Length; ++i)
            {
                defenders[i] = new Defender(NextEnemySpawnLocationLeftLane());

                updateables.Add(defenders[i]);
                drawables.Add(defenders[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (sora.WorldPosition.X > ? && riku.WorldPosition > ?)
            //{
            //    sceneManager.SceneTransitionNextRoom();
            //}

            if(shadows.All((s) => !s.IsAlive) && defenders.All(d => !d.IsAlive))
            {
                sceneManager.SceneTransitionNextRoom();
                // sceneManager.SceneTransition(EScene.TRAIN_ROOM_2);
            }
        }
    }
}
