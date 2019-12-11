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
    public class Scene_Desert_Room5 : Scene
    {
        private Rectangle bigDoor1InteractionArea = new Rectangle(3616, 4517, 515, 257);
        private Rectangle bigDoor2InteractionArea = new Rectangle(3724, 1517, 407, 259);

        private Shadow[] shadows;
        private Defender[] defenders;
        private Gargoyle[] gargoyles;

        public Scene_Desert_Room5(SceneManager sceneManager)
            : base(sceneManager)
        {
        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes2_desert/room5.tm.txt");

            sora.WorldPosition = new Vector2(3500, 6800);
            sora.Sprite.SetAnimation("IDLE_RIGHT");

            riku.WorldPosition = new Vector2(4000, 6800);
            riku.Sprite.SetAnimation("IDLE_RIGHT");

            enemySpawnLocationsLeftLane.AddRange(new Vector2[]
            {
                new Vector2(1600, 6175),
                new Vector2(851, 5416),
                new Vector2(1730, 4218),
                new Vector2(2951, 4312),
                new Vector2(2334, 3749),
                new Vector2(363, 3521),
                new Vector2(594, 1215),
                new Vector2(1697, 562),
                new Vector2(2797, 948),
                new Vector2(4990, 803),
                new Vector2(5370, 1389),
                new Vector2(6871, 652),
                new Vector2(7530, 1385), 
                new Vector2(8305, 2379),
                new Vector2(6868, 2958),
                new Vector2(7389, 3540),
                new Vector2(5833, 3987),
                new Vector2(7334, 5363),
                new Vector2(4781, 4434),
                new Vector2(6283, 6431),
                new Vector2(3849, 5331),
            });
            RandomizeEnemySpawnLocations();

            shadows = new Shadow[4];
            for (int i = 0; i < shadows.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane());
                }
                else
                {
                    shadows[i] = new Shadow(NextEnemySpawnLocationLeftLane());
                }

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            defenders = new Defender[4];
            for (int i = 0; i < defenders.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    defenders[i] = new Defender(NextEnemySpawnLocationLeftLane());
                }
                else
                {
                    defenders[i] = new Defender(NextEnemySpawnLocationLeftLane());
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
                    gargoyles[i] = new Gargoyle(NextEnemySpawnLocationLeftLane());
                }

                updateables.Add(gargoyles[i]);
                drawables.Add(gargoyles[i]);
            }

            base.OnEnterScene();

            // camera.Zoom = 0.8f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // bigDoor1
            if (shadows.All(s => !s.IsAlive) && defenders.All(d => !d.IsAlive) && gargoyles.All(g => !g.IsAlive))
            {
                if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) &&
                    CollisionManager.IsPlayerInArea(PlayerIndex.One, bigDoor1InteractionArea))
                {
                    sora.WorldPosition = new Vector2(3781, 3257);
                }
                else if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two) &&
                    CollisionManager.IsPlayerInArea(PlayerIndex.Two, bigDoor1InteractionArea))
                {
                    riku.WorldPosition = new Vector2(3996, 3265);
                }
            }

            // bigDoor2
            if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) &&
                CollisionManager.IsPlayerInArea(PlayerIndex.One, bigDoor2InteractionArea))
            {
                sceneManager.SceneTransition(EScene.CASTLE_ROOM_1);
            }
            else if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two) &&
                CollisionManager.IsPlayerInArea(PlayerIndex.Two, bigDoor2InteractionArea))
            {
                sceneManager.SceneTransition(EScene.CASTLE_ROOM_1);
            }
        }
    }
}
