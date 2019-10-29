using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EVCMonoGame.src
{
	public class DummyEnemy : Enemy
	{
		public DummyEnemy(Rectangle bounds)
		{
			CollisionBox = bounds;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			
		}

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);
		}


		public override void Update(GameTime gameTime)
		{
			// TODO: playerSprite steuern(Animationen ändern und bewegen)
			base.Update(gameTime);

			processInput();
		}


		public void processInput()
		{			

			if (InputManager.IsAnyKeyPressed(Keys.W, Keys.D, Keys.S, Keys.A))
			{
				Vector2 playerDirection = new Vector2();

				PreviousWorldPosition = WorldPosition;

				if (InputManager.IsKeyPressed(Keys.W))
				{
					Console.WriteLine("jaa");
					playerDirection += new Vector2(0, -1);
				}
				if (InputManager.IsKeyPressed(Keys.S))
				{
					playerDirection += new Vector2(0, 1);
				}
				if (InputManager.IsKeyPressed(Keys.D))
				{
					playerDirection += new Vector2(1, 0);
				}
				if (InputManager.IsKeyPressed(Keys.A))
				{
					playerDirection += new Vector2(-1, 0);
				}

				// Richtungsvektor Normalizieren
				if (playerDirection != Vector2.Zero)
					playerDirection.Normalize();

				//Snap to Grid
				WorldPosition += playerDirection * 7;

				if (CollisionManager.IsCollisionAfterMove(this, true, true))
				{
					//Position = PreviousPosition;
				}


				// Update Healthbar etc. was sich auf die neue Position bezieht
				OnMove();
			}

		}
	}
}
