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
    public class Scene_Train_Room2 : Scene
    {
        private Shadow[] shadows;
        private Defender[] defenders;

        public Scene_Train_Room2(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes1_train/room2.tm.txt");

            sora.WorldPosition = new Vector2(13000, 2650);
            riku.WorldPosition = new Vector2(12000, 2650);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(14480, 2543),
                new Vector2(13761, 2597),
                new Vector2(14910, 2676),
                new Vector2(15785, 2670),
                new Vector2(15965, 2706),
                new Vector2(17110, 2584),
                new Vector2(17424, 2795),
                new Vector2(13859, 2565),
                new Vector2(13907, 2790),
                new Vector2(14464, 2752),
                new Vector2(12832, 2608),
                new Vector2(13020, 2767),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[5];
            for (int i = 0; i < shadows.Length; ++i)
            {
                shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane(), GameplayState.Lane.LaneBoth);

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            defenders = new Defender[3];
            for (int i = 0; i < defenders.Length; ++i)
            {
                defenders[i] = new Defender(NextEnemySpawnLocationLeftLane(), GameplayState.Lane.LaneBoth);

                updateables.Add(defenders[i]);
                drawables.Add(defenders[i]);
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
