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

namespace EVCMonoGame.src.scenes.tutorial
{
    public class Scene_Tutorial_Room3 : Scene
    {
        private Shadow[] shadows;

        public Scene_Tutorial_Room3(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero, 
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room3.tm.txt");

            sora.WorldPosition = new Vector2(1356, 4265);
            riku.WorldPosition = new Vector2(2069, 4376);

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(1350, 1320),
                new Vector2(660, 1877),
                new Vector2(1214, 2007), 
                new Vector2(440, 2500),
                new Vector2(1249, 2500),
                new Vector2(1140, 3636),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(2030, 1500),
                new Vector2(2800, 1920),
                new Vector2(2500, 2174),
                new Vector2(3180, 2342),
                new Vector2(2451, 2930),
                new Vector2(2368, 3430),
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

            doorPlayerOne = new Door(new Vector2(1550, 801));
            doorPlayerTwo = new Door(new Vector2(2000, 805));
			
            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            camera.Zoom = 0.7f;
            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (doorPlayerOne.Open && doorPlayerTwo.Open /* &&
            //    !shadow1.IsAlive && !shadow2.IsAlive */)
            //{
            //    sceneManager.SceneTransition(EScene.TUTORIAL_ROOM_4);
            //}

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
