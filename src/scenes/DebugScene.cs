using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.scenes
{
    class DebugScene : Scene
    {
        private Player player;
        private Player player2;
        private Player player3;
        private SpriteFont randomText;
        private Texture2D background;
        private GeometryBox geometryBox;

        private CollisionManager collisionManager;

        public DebugScene(SceneManager sceneManager)
            : base(sceneManager)
        {
            player = new Player(sceneManager.GetViewportCenter(), 
                new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left });

            player2 = new Player(new Vector2(200, 500), new Keys[] { Keys.W, Keys.S, Keys.D, Keys.A });
            player3 = new Player(new Vector2(300, 700), new Keys[] { Keys.I, Keys.K, Keys.L, Keys.J });
            geometryBox = new GeometryBox(new Rectangle(500, 400, 50, 1000));

            collisionManager = new CollisionManager();
            collisionManager.AddCollidables(new Collidable[]
            {
                player.Sprite,
                player2.Sprite,
                player3.Sprite,
                geometryBox,
            });

            updateables.AddRange(new Updateable[] 
            { 
                player,
                player2,
                player3,
                collisionManager,
            });

            drawables.AddRange(new IDrawable[] 
            { 
                player,
                player2,
                player3,
                collisionManager,
            });
        }

        public override void LoadContent(ContentManager content)
        {
            randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            background = content.Load<Texture2D>("rsrc/backgrounds/cyberpunk-street");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.OnKeyPressed(Keys.Space))
            {
                sceneManager.SceneTransition(EScene.DEBUG_2);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
