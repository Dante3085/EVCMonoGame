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
using EVCMonoGame.src.Traps;

namespace EVCMonoGame.src.scenes.castle
{
    public class Scene_Castle_Room3 : Scene
    {
        private Defender[] defenders;
        private Gargoyle[] gargoyles;

        public Scene_Castle_Room3(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {

			SpikeTrap spikeTrap_1 = new SpikeTrap(new Vector2(760, 2600));
			SpikeTrap spikeTrap_2 = new SpikeTrap(new Vector2(960, 2600));
			SpikeTrap spikeTrap_3 = new SpikeTrap(new Vector2(1160, 2600));

			SpikeTrap spikeTrap_4 = new SpikeTrap(new Vector2(1500, 2600));
			SpikeTrap spikeTrap_5 = new SpikeTrap(new Vector2(1700, 2600));
			SpikeTrap spikeTrap_6 = new SpikeTrap(new Vector2(1900, 2600));


			SpikeTrap spikeTrap_7 = new SpikeTrap(new Vector2(260, 1600));
			SpikeTrap spikeTrap_8 = new SpikeTrap(new Vector2(460, 1600));
			SpikeTrap spikeTrap_9 = new SpikeTrap(new Vector2(660, 1600));

			SpikeTrap spikeTrap_10 = new SpikeTrap(new Vector2(1900, 1600));
			SpikeTrap spikeTrap_11 = new SpikeTrap(new Vector2(2100, 1600));
			SpikeTrap spikeTrap_12 = new SpikeTrap(new Vector2(2300, 1600));

			updateables.AddRange(new IUpdateable[]
			{
				spikeTrap_1,
				spikeTrap_2,
				spikeTrap_3,
				spikeTrap_4,
				spikeTrap_5,
				spikeTrap_6,
				spikeTrap_7,
				spikeTrap_8,
				spikeTrap_9,
				spikeTrap_10,
				spikeTrap_11,
				spikeTrap_12
			});

			drawables.AddRange(new IDrawable[]
			{
				spikeTrap_1,
				spikeTrap_2,
				spikeTrap_3,
				spikeTrap_4,
				spikeTrap_5,
				spikeTrap_6,
				spikeTrap_7,
				spikeTrap_8,
				spikeTrap_9,
				spikeTrap_10,
				spikeTrap_11,
				spikeTrap_12
			});
			tilemap = new Tilemap(Vector2.Zero,
               "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room3.tm.txt");

            sora.WorldPosition = new Vector2(950, 2900);
            riku.WorldPosition = new Vector2(1700, 3000);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(912, 2700),
                new Vector2(462, 1575),
                new Vector2(757, 1000),
                new Vector2(765, 1463),
                new Vector2(486, 1850),
                new Vector2(317, 1456),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(1745, 2666),
                new Vector2(1983, 2018),
                new Vector2(2309, 1741),
                new Vector2(2346, 1389),
                new Vector2(2015, 1132),
                new Vector2(2065, 725),
            });
            RandomizeEnemySpawnLocations();

            defenders = new Defender[2];
            for (int i = 0; i < defenders.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    defenders[i] = new Defender(NextEnemySpawnLocationLeftLane(), GameplayState.Lane.LaneOne);
                }
                else
                {
                    defenders[i] = new Defender(NextEnemySpawnLocationRightLane(), GameplayState.Lane.LaneTwo);
                }

                updateables.Add(defenders[i]);
                drawables.Add(defenders[i]);
            }

            gargoyles = new Gargoyle[2];
            for (int i = 0; i < gargoyles.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    gargoyles[i] = new Gargoyle(NextEnemySpawnLocationLeftLane(), GameplayState.Lane.LaneOne);
                }
                else
                {
                    gargoyles[i] = new Gargoyle(NextEnemySpawnLocationRightLane(), GameplayState.Lane.LaneTwo);
                }

                updateables.Add(gargoyles[i]);
                drawables.Add(gargoyles[i]);
            }

            doorPlayerOne = new Door(new Vector2(767, 383));
            doorPlayerTwo = new Door(new Vector2(1920, 383));


            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            camera.FollowPlayers();

            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
