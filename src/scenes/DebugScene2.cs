﻿using System;
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

		public DebugScreen2(SceneManager sceneManager)
            : base(sceneManager)
        {
            player = new Player(new Rectangle(750, 300, 100, 100), new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left });
			Enemy dummyEnemy = new DummyEnemy(new Rectangle(800, 100, 100, 100));
			geometryBox = new GeometryBox(new Rectangle(300, 300, 200, 200));

			//spawnCharacter(player, new Rectangle(750, 300, 100, 100));



			collisionManager = new CollisionManager();
			collisionManager.AddCollidables(new Collidable[]
			{
				dummyEnemy,
				player,
				geometryBox,
				new GeometryBox(new Rectangle(500, 299, 200, 200)),
				new GeometryBox(new Rectangle(100, 100, 200, 200)),
				new GeometryBox(new Rectangle(300, 100, 100, 100)),
				new GeometryBox(new Rectangle(100, 300, 110, 100)),
			});

            updateables.AddRange(new Updateable[]
            {
                player,
				dummyEnemy,
				collisionManager,
			});

            drawables.AddRange(new IDrawable[]
            {
                collisionManager,
                player,
				dummyEnemy,
			});


        }

		public void spawnCharacter(Character character, Rectangle bounds)
		{
			character.Position = new Vector2(bounds.X, bounds.Y);
			character.Bounds = bounds;
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
