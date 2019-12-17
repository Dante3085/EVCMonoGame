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

namespace EVCMonoGame.src.scenes.desert
{
    public class Scene_Desert_Room2 : Scene
    {
        private Shadow[] shadows;
        
        public Scene_Desert_Room2(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes2_desert/room2.tm.txt");

            sora.WorldPosition = new Vector2(2000, 5200);
            riku.WorldPosition = new Vector2(5000, 3700);

            doorPlayerOne = new Door(new Vector2(1794, 3000));
            doorPlayerTwo = new Door(new Vector2(4415, 2177));

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(1785, 4760),
                new Vector2(2282, 4598),
                new Vector2(2172, 4030),
                new Vector2(1441, 3927),
                new Vector2(1961, 3472),
                new Vector2(1764, 3332),
                new Vector2(2217, 3669),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(4302, 3508),
                new Vector2(5976, 3305),
                new Vector2(5110, 3103),
                new Vector2(3908, 3047),
                new Vector2(4575, 2539),
                new Vector2(5336, 1940),
                new Vector2(5852, 1697)
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[10];
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
