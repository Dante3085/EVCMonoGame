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

namespace EVCMonoGame.src.scenes.desert
{
    public class Scene_Desert_Room4 : Scene
    {
        private Shadow[] shadows;
        private Defender[] defenders;
        private Gargoyle[] gargoyles;

        public Scene_Desert_Room4(SceneManager sceneManager)
           : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes2_desert/room4.tm.txt");

            sora.WorldPosition = new Vector2(1000, 5200);
            riku.WorldPosition = new Vector2(3000, 5200);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(633, 4890),
                new Vector2(1580, 4353),
                new Vector2(305, 3764),
                new Vector2(468, 2844),
                new Vector2(601, 2146),
                new Vector2(1195, 1555),
                new Vector2(1082, 894),
                new Vector2(420, 248),
                new Vector2(380, 2217),
                new Vector2(1116, 2121),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(3931, 4972),
                new Vector2(2865, 4376),
                new Vector2(3908, 3400),
                new Vector2(2721, 2777),
                new Vector2(3964, 2400),
                new Vector2(3202, 1674),
                new Vector2(3822, 996),
                new Vector2(2967, 280),
                new Vector2(2775, 2432),
                new Vector2(2773, 3231),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[4];
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

            defenders = new Defender[4];
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

            gargoyles = new Gargoyle[4];
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

            doorPlayerOne = new Door(new Vector2(1090, 0));
            doorPlayerTwo = new Door(new Vector2(3500, 0));

			LaneTeleporter teleporter_sora = new LaneTeleporter(new Vector2(820, 2250), sora, new Vector2(3331, 2372), 12000);
			LaneTeleporter teleporter_riku = new LaneTeleporter(new Vector2(3400, 3980), riku, new Vector2(850, 4400), 12000);

			updateables.AddRange(new IUpdateable[]
			{
				teleporter_sora,
				teleporter_riku
			});

			drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
				teleporter_sora,
				teleporter_riku
			});

            base.OnEnterScene();

            if (!isArabianDesertPlaying)
            {
                isArabianDesertPlaying = true;
                MediaPlayer.Play(AssetManager.GetSong(ESong.ARABIAN_DESERT));
            }
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
