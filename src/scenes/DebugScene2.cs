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
using EVCMonoGame.src.Items;

namespace EVCMonoGame.src.scenes
{
	public class DebugScreen2 : Scene
	{
		private Player player;
		private SpriteFont randomText;
		private Texture2D background;
		private GeometryBox geometryBox;

		public DebugScreen2(SceneManager sceneManager)
			: base(sceneManager)
		{
		}

		public override void LevelStartsEvent()
		{
			base.LevelStartsEvent();

			player = new Player(new Rectangle(750, 300, 100, 100), new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left});
			Enemy dummyEnemy = new DummyEnemy(new Rectangle(800, 100, 100, 100));
			geometryBox = new GeometryBox(new Rectangle(300, 300, 200, 200));

			//spawnCharacter(player, new Rectangle(750, 300, 100, 100));

			// Spawn Items
			PickUpItem.Stats HealPotionStats = new PickUpItem.Stats();
			HealPotionStats.heal = 10;

			PickUpItem HealPotion = new PickUpItem(new Rectangle(700, 500, 50, 50), HealPotionStats);


			CollisionManager.AddCollidables(new Collision[]
			{
				dummyEnemy,
				player,
				HealPotion,
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
				HealPotion,
			});

			drawables.AddRange(new IDrawable[]
			{
				player,
				dummyEnemy,
				HealPotion,
			});
		}
		
		

		public void spawnCharacter(Character character, Rectangle bounds)
		{
			character.WorldPosition = new Vector2(bounds.X, bounds.Y);
			character.CollisionBox = bounds;
		}

		public override void LoadContent(ContentManager content)
        {
            //randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            //background = content.Load<Texture2D>("rsrc/backgrounds/background");

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
