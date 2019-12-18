using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using EVCMonoGame.src.states;
using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.scenes.train
{
    public class Scene_Train_Room4 : Scene
    {
        private Shadow[] shadows;

        public Scene_Train_Room4(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes1_train/room4.tm.txt");

            sora.WorldPosition = new Vector2(175, 615);
            riku.WorldPosition = new Vector2(179, 2042);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(400, 670),
                new Vector2(1000, 700),
                new Vector2(1830, 677),
                new Vector2(2467, 663),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(300, 2075),
                new Vector2(1012, 2115),
                new Vector2(1921, 2123),
                new Vector2(2465, 2025),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[4];
            for (int i = 0; i < shadows.Length; ++i)
            {
                // Spawn on left side.
                if (i % 2 == 0)
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane(), GameplayState.Lane.LaneOne);
                }

                // Spawn on right side.
                else
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationRightLane(), GameplayState.Lane.LaneTwo);
                }

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            if (!isSynthTrainPlaying)
            {
                isSynthTrainPlaying = true;
                MediaPlayer.Play(AssetManager.GetSong(ESong.SYNTH_TRAIN));
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
