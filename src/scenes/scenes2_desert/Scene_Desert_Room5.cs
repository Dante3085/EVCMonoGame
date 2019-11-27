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
        private PlayerOne playerOne = GameplayState.PlayerOne;
        private PlayerTwo playerTwo = GameplayState.PlayerTwo;

        private Rectangle bigDoor1InteractionArea = new Rectangle(3616, 4517, 515, 257);
        private Rectangle bigDoor2InteractionArea = new Rectangle(3724, 1517, 407, 259);

        public Scene_Desert_Room5(SceneManager sceneManager)
            : base(sceneManager)
        {
        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes2_desert/room5.tm.txt");

            playerOne.WorldPosition = new Vector2(3500, 6800);
            playerOne.Sprite.SetAnimation("IDLE_RIGHT");

            playerTwo.WorldPosition = new Vector2(4000, 6800);
            playerTwo.Sprite.SetAnimation("IDLE_RIGHT");

            doorPlayerOne = new Door(new Vector2(5300, 7));
            doorPlayerTwo = new Door(new Vector2(6847, 3));

            updateables.AddRange(new IUpdateable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            base.OnEnterScene();

            // camera.Zoom = 0.8f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // bigDoor1
            if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) && 
                CollisionManager.IsPlayerInArea(PlayerIndex.One, bigDoor1InteractionArea))
            {
                playerOne.WorldPosition = new Vector2(3781, 3257);
            }
            else if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two) &&
                CollisionManager.IsPlayerInArea(PlayerIndex.Two, bigDoor1InteractionArea))
            {
                playerTwo.WorldPosition = new Vector2(3996, 3265);
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

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            tilemap.LoadContent(contentManager);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            tilemap.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
