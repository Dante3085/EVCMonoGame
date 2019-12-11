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
    class Scene_Train_Room3 : Scene
    {
        private Shadow[] shadows;

        public Scene_Train_Room3(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes1_train/room3.tm.txt");

            sora.WorldPosition = new Vector2(255, 1095);
            riku.WorldPosition = new Vector2(312, 394);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(600, 436),
                new Vector2(880, 463),
                new Vector2(1305, 336),
                new Vector2(1485, 478),
                new Vector2(1746, 458),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(430, 1200),
                new Vector2(630, 1200),
                new Vector2(1130, 1200),
                new Vector2(1400, 1087),
                new Vector2(1752, 1253),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[6];
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

            if (shadows.All(s => !s.IsAlive))
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
