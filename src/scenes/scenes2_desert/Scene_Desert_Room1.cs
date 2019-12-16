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
    public class Scene_Desert_Room1 : Scene
    {
        private Gargoyle[] gargoyles;

        public Scene_Desert_Room1(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes2_desert/room1.tm.txt");

            sora.WorldPosition = new Vector2(700, 4100);
            riku.WorldPosition = new Vector2(2200, 4100);

            doorPlayerOne = new Door(new Vector2(959, 1280));
            doorPlayerTwo = new Door(new Vector2(2240, 1280));

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(544, 3524),
                new Vector2(1144, 3465),
                new Vector2(583, 2814),
                new Vector2(1210, 2290),
                new Vector2(865, 1705),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(2363, 3668),
                new Vector2(2046, 3216),
                new Vector2(2698, 2781),
                new Vector2(1989, 2063),
                new Vector2(2736, 1677),
            });
            RandomizeEnemySpawnLocations();

            gargoyles = new Gargoyle[6];
            for (int i = 0; i < gargoyles.Length; ++i)
            {
                // Spawn on left side.
                if (i % 2 == 0)
                {
                    gargoyles[i] = new Gargoyle(NextEnemySpawnLocationLeftLane());
                }

                // Spawn on right side.
                else
                {
                    gargoyles[i] = new Gargoyle(NextEnemySpawnLocationRightLane());
                }

                updateables.Add(gargoyles[i]);
                drawables.Add(gargoyles[i]);
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

            if (gargoyles.All(g => !g.IsAlive) && doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
