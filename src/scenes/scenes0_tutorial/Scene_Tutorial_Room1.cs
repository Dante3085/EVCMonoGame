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
    public class Scene_Tutorial_Room1 : Scene
    {
        private Tilemap tilemap;

        private Shadow shadow = new Shadow(new Vector2(4000, 1300));

        private Lever lever = new Lever(new Vector2(3000, 1300));

        public Scene_Tutorial_Room1(SceneManager sceneManager)
            : base(sceneManager)
        {
            tilemap = new Tilemap(Vector2.Zero, 
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room1.tm.txt");
        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.WorldPosition = new Vector2(2300, 1300);
            playerOne.Sprite.SetAnimation("IDLE_RIGHT");

            playerTwo.WorldPosition = new Vector2(2300, 1900);
            playerTwo.Sprite.SetAnimation("IDLE_RIGHT");

            camera.Zoom = 0.55f;

            updateables.AddRange(new IUpdateable[]
            {
                shadow,
                lever,
            });

            drawables.AddRange(new IDrawable[]
            {
                shadow,
                lever,
            });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) &&
                CollisionManager.IsPlayerInArea(PlayerIndex.One, new Rectangle(5200, 67, 400, 213)))
            {
                if (CollisionManager.IsPlayerInArea(PlayerIndex.Two, new Rectangle(6700, 75, 400, 215)))
                {
                    sceneManager.SceneTransition(EScene.GAME_OVER);
                }
            }
            else if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two) &&
                CollisionManager.IsPlayerInArea(PlayerIndex.Two, new Rectangle(6700, 75, 400, 215)))
            {
                if (CollisionManager.IsPlayerInArea(PlayerIndex.One, new Rectangle(5200, 67, 400, 213)))
                {
                    sceneManager.SceneTransition(EScene.GAME_OVER);
                }
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            tilemap.LoadContent(contentManager);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred ,samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            tilemap.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
