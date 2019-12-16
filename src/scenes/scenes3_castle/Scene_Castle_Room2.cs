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
    public class Scene_Castle_Room2 : Scene
    {
        private Defender[] defenders;
        private Gargoyle[] gargoyles;

        public Scene_Castle_Room2(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room2.tm.txt");

            sora.WorldPosition = new Vector2(2789, 2770);
            riku.WorldPosition = new Vector2(4090, 2770);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(2957, 1202),
                new Vector2(2239, 1566),
                new Vector2(1239, 1841),
                new Vector2(635, 1534),
                new Vector2(1015, 1215),
                new Vector2(869, 2131),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(3897, 1041),
                new Vector2(4256, 1643),
                new Vector2(4200, 1884),
                new Vector2(5000, 1568),
                new Vector2(6040, 1301),
                new Vector2(6064, 1873),
            });
            RandomizeEnemySpawnLocations();

            defenders = new Defender[4];
            for (int i = 0; i < defenders.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    defenders[i] = new Defender(NextEnemySpawnLocationLeftLane());
                }
                else
                {
                    defenders[i] = new Defender(NextEnemySpawnLocationRightLane());
                }

                updateables.Add(defenders[i]);
                drawables.Add(defenders[i]);
            }

            gargoyles = new Gargoyle[4];
            for (int i = 0; i < gargoyles.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    gargoyles[i] = new Gargoyle(NextEnemySpawnLocationLeftLane());
                }
                else
                {
                    gargoyles[i] = new Gargoyle(NextEnemySpawnLocationRightLane());
                }

                updateables.Add(gargoyles[i]);
                drawables.Add(gargoyles[i]);
            }

            doorPlayerOne = new Door(new Vector2(3071, 381));
            doorPlayerTwo = new Door(new Vector2(3831, 386));
			
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

            if (defenders.All(d => !d.IsAlive && gargoyles.All(g => !g.IsAlive)) &&
                doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
