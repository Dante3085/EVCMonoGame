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
	public class ItemDebugScene : Scene
	{
		private SpriteFont randomText;
		private Texture2D background;
		private GeometryBox geometryBox;

		public ItemDebugScene(SceneManager sceneManager)
			: base(sceneManager)
		{
		}

		public override void LevelStartsEvent()
		{
			base.LevelStartsEvent();

			DummyEnemy dummyEnemy = new DummyEnemy(new Rectangle(100, 700, 50, 50));

			
			// Spawn Items
			PickUpItem HealPotion = new PickUpItem(new Rectangle(700, 500, 50, 50)) { stats = new PickUpItem.ItemStats() { heal = 10 } };
			PickUpItem PosionPotion = new PickUpItem(new Rectangle(800, 500, 50, 50)) { stats = new PickUpItem.ItemStats() { heal = -40 } };
			PickUpItem SpeedPotion = new PickUpItem(new Rectangle(750, 560, 50, 50)) { stats = new PickUpItem.ItemStats() { speed = 6 } };

			new GeometryBox(new Rectangle(500, 299, 200, 200));
			new GeometryBox(new Rectangle(100, 100, 200, 200));
			new GeometryBox(new Rectangle(300, 100, 100, 100));


			updateables.AddRange(new Updateable[]
			{
				dummyEnemy,
				//HealPotion,
				//PosionPotion,
				//SpeedPotion,
			});

			drawables.AddRange(new IDrawable[]
			{
				dummyEnemy,
				//HealPotion,
				//PosionPotion,
				//SpeedPotion,
			});
		}
		
		

		public void spawnCharacter(Character character, Rectangle bounds)
		{
			character.WorldPosition = new Vector2(bounds.X, bounds.Y);
			character.CollisionBox = bounds;
		}

		public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
			randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
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
            spriteBatch.DrawString(randomText, "Press X for NavMesh.", new Vector2(30, 40), Color.White);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
