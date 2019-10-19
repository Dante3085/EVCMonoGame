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
    public class DebugScreen2 : Scene
    {
        private Player player;
        private SpriteFont randomText;
        private Texture2D background;
        private CollisionManager collisionManager;
        private GeometryBox geometryBox;
		private GeometryBox geometryBox2;
		private GeometryBox geometryBox3;
		private GeometryBox geometryBox4;
		private GeometryBox geometryBox5;

		public DebugScreen2(SceneManager sceneManager)
            : base(sceneManager)
        {
            player = new Player(Vector2.Zero, new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left });
            geometryBox = new GeometryBox(new Rectangle(300, 300, 200, 200));
			geometryBox2 = new GeometryBox(new Rectangle(500, 299, 200, 200));
			geometryBox3 = new GeometryBox(new Rectangle(100, 100, 200, 200));
			geometryBox4 = new GeometryBox(new Rectangle(300, 100, 100, 100));
			geometryBox5 = new GeometryBox(new Rectangle(100, 300, 110, 100));

			collisionManager = new CollisionManager();
            collisionManager.AddCollidables(new Collidable[]
			{
				player,
				geometryBox,
				geometryBox2,
				geometryBox3,
				geometryBox4,
				geometryBox5
			});

            updateables.AddRange(new Updateable[]
            {
                player,
                collisionManager,
            });

            drawables.AddRange(new IDrawable[]
            {
                collisionManager,
                player,
            });
        }

        public override void LoadContent(ContentManager content)
        {
            randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            background = content.Load<Texture2D>("rsrc/backgrounds/background");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.OnKeyPressed(Keys.Space))
            {
                sceneManager.SceneTransition(EScene.DEBUG);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            //spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.", new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
