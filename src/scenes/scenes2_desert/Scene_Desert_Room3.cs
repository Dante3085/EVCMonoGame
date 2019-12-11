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
    public class Scene_Desert_Room3 : Scene
    {

        private Defender[] defenders;
        private Gargoyle[] gargoyles;

        public Scene_Desert_Room3(SceneManager sceneManager)
           : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes2_desert/room3.tm.txt");

            sora.WorldPosition = new Vector2(900, 2000);
            riku.WorldPosition = new Vector2(2500, 2000);

            doorPlayerOne = new Door(new Vector2(765, 0));
            doorPlayerTwo = new Door(new Vector2(2431, 0));

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(1437, 1272),
                new Vector2(913, 740),
                new Vector2(1212, 279),
                new Vector2(426, 238),
            });

            enemySpawnLocationsRightLane.AddRange(new Vector2[]
            {
                new Vector2(2200, 1878),
                new Vector2(2867, 1485),
                new Vector2(2205, 830),
                new Vector2(2213, 253),
            });
            RandomizeEnemySpawnLocations();

            defenders = new Defender[2];
            defenders[0] = new Defender(NextEnemySpawnLocationLeftLane());
            defenders[1] = new Defender(NextEnemySpawnLocationRightLane());

            gargoyles = new Gargoyle[2];
            gargoyles[0] = new Gargoyle(NextEnemySpawnLocationLeftLane());
            gargoyles[1] = new Gargoyle(NextEnemySpawnLocationRightLane());

            AddUpdateables
            (
                defenders[0],
                defenders[1],

                gargoyles[0],
                gargoyles[1],

                doorPlayerOne,
                doorPlayerTwo
            );

            AddDrawables
            (
                defenders[0],
                defenders[1],

                gargoyles[0],
                gargoyles[1],

                doorPlayerOne,
                doorPlayerTwo
            );

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
