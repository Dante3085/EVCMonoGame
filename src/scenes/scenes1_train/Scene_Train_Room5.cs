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
        private Shadow[] shadows;

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

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(361, 1230),
                new Vector2(1560, 664),
                new Vector2(1627, 1190),
                new Vector2(2200, 1160),
                new Vector2(2348, 787),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(1505, 2357),
                new Vector2(1635, 2729),
                new Vector2(1931, 2891),
                new Vector2(2207, 2865),
                new Vector2(2352, 2444),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[10];
            for (int i = 0; i < shadows.Length; ++i)
            {
                // Spawn on left side.
                if (i % 2 == 0)
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane());
                }

                // Spawn on right side.
                else
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationRightLane());
                }

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (shadows.All((s) => !s.IsAlive))
            {
                sceneManager.SceneTransitionNextRoom();
                // sceneManager.SceneTransition(EScene.TRAIN_ROOM_3);
            }
        }
    }
}
