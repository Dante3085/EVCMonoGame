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
	public class CollisionDebugScene : Scene
	{
		private SpriteFont randomText;
		private Texture2D background;
		private GeometryBox geometryBox;

		public CollisionDebugScene(SceneManager sceneManager)
			: base(sceneManager)
		{
		}

		public override void LevelStartsEvent()
		{
			base.LevelStartsEvent();

			DummyEnemy dummyEnemy = new DummyEnemy(new Rectangle(100, 700, 50, 50));
			//geometryBox = new GeometryBox(new Rectangle(300, 300, 200, 200));

			//spawnCharacter(player, new Rectangle(750, 300, 100, 100));

			// Spawn Items
			//PickUpItem HealPotion = new PickUpItem(		new Rectangle(700, 500, 50, 50)) { stats = new PickUpItem.ItemStats() { heal = 10 } };
			//PickUpItem PosionPotion = new PickUpItem(	new Rectangle(800, 500, 50, 50)) { stats = new PickUpItem.ItemStats() { heal = -40 } };
			//PickUpItem SpeedPotion = new PickUpItem(	new Rectangle(750, 560, 50, 50)) { stats = new PickUpItem.ItemStats() { speed = 6 } };

			//new GeometryBox(new Rectangle(500, 299, 200, 200));
			//new GeometryBox(new Rectangle(100, 100, 200, 200));
			//new GeometryBox(new Rectangle(300, 100, 100, 100));
			new GeometryBox(new Rectangle(99, 200, 100, 100));
			new GeometryBox(new Rectangle(100, 300, 100, 100));
			new GeometryBox(new Rectangle(172, 400, 100, 100));

			new GeometryBox(new Rectangle(399, 200, 130, 100));
			new GeometryBox(new Rectangle(400, 300, 130, 100));
			new GeometryBox(new Rectangle(401, 400, 130, 100));

			new GeometryBox(new Rectangle(700, 200, 30, 100));
			new GeometryBox(new Rectangle(785, 300, 30, 100));
			new GeometryBox(new Rectangle(770, 400, 30, 100));

			new GeometryBox(new Rectangle(1099, 200, 101, 100));
			new GeometryBox(new Rectangle(1100, 300, 101, 100));
			new GeometryBox(new Rectangle(1101, 400, 101, 100));


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
                sceneManager.SceneTransition(EScene.DEBUG_2);
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
