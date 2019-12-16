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
    public class Scene_Castle_Room4 : Scene
    {
        private Shadow[] shadows;

        public Scene_Castle_Room4(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
               "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room4.tm.txt");

            sora.WorldPosition = new Vector2(565, 6400);
            riku.WorldPosition = new Vector2(1900, 6400);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(258, 5700),
                new Vector2(887, 5661),
                new Vector2(275, 3372),
                new Vector2(892, 3407),
                new Vector2(249, 989),
                new Vector2(879, 951),
                new Vector2(576, 2454),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(1569, 1001),
                new Vector2(1600, 3418),
                new Vector2(2204, 3395),
                new Vector2(1600, 5704),
                new Vector2(2279, 5761),
                new Vector2(1937, 4859),
                new Vector2(1938, 1997),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[12];
            for (int i = 0; i < shadows.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane());
                }
                else
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationRightLane());
                }

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            doorPlayerOne = new Door(new Vector2(577, 384));
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
