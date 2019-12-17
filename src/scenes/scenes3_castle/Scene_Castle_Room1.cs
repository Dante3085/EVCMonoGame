using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.scenes.castle
{
    public class Scene_Castle_Room1 : Scene
    {
        private Shadow[] shadows;

        public Scene_Castle_Room1(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room1.tm.txt");

            sora.WorldPosition = new Vector2(1540, 3315);
            riku.WorldPosition = new Vector2(2278, 3256);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(1394, 2245),
                new Vector2(943, 2502),
                new Vector2(389, 2724),
                new Vector2(467, 1891),
                new Vector2(1392, 1026),
                new Vector2(411, 732),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(2410, 2340),
                new Vector2(2823, 2716),
                new Vector2(3319, 2661),
                new Vector2(3394, 1334),
                new Vector2(2251, 1060),
                new Vector2(3443, 3521)
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[6];
            for (int i = 0; i < shadows.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane(), GameplayState.Lane.LaneOne);
                }
                else
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationRightLane(), GameplayState.Lane.LaneTwo);
                }

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            doorPlayerOne = new Door(new Vector2(384, 256));
            doorPlayerTwo = new Door(new Vector2(3456, 256));


            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });
            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (shadows.All(s => !s.IsAlive) && 
                doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }

    }
}
