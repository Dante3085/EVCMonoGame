using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using C3.MonoGame;
using EVCMonoGame.src.events;
using EVCMonoGame.src.Items;

namespace EVCMonoGame.src
{
    public class Player : Character
    {

        private AnimatedSprite playerSprite;
		private ItemFinder itemFinder;
		private Inventory inventory;

		private Keys[] controls;

		#region Porperties
		public AnimatedSprite Sprite
        {
            get { return playerSprite; }
        }

		public Inventory PlayerInventory
		{
			get { return inventory; }
			set { inventory = value; }
		}

		public float PlayerSpeed
		{
			get { return movementSpeed; }
			set { movementSpeed = value; }
		}

		private Vector2 PlayerDirection
		{
			get { return movementDirection; }
			set { movementDirection = value; }
		}
		#endregion


        public Player(Rectangle bounds, Keys[] controls) : base(bounds)
        {
			inventory = new Inventory(this);
			itemFinder = new ItemFinder(this);
			
            //playerSprite = new AnimatedSprite("rsrc/spritesheets/CronoTransparentBackground", position, 6.0f);

            // Frames sind leicht falsch(Abgeschnittene Ecken).
            //playerSprite.AddAnimation("IDLE", new Rectangle[]
            //{
            //    new Rectangle(59, 14, 15, 34), new Rectangle(79, 14, 15, 34), new Rectangle(99, 14, 15, 34)
            //}, 0.8f);
            //playerSprite.AddAnimation("WALK_UP", new Rectangle[]
            //{
            //    new Rectangle(130, 59, 17, 32), new Rectangle(152, 60, 17, 31), new Rectangle(174, 57, 15, 34),
            //    new Rectangle(193, 57, 15, 34), new Rectangle(213, 60, 17, 31), new Rectangle(235, 59, 17, 32),
            //}, 0.15f);
            //playerSprite.AddAnimation("WALK_LEFT", new Rectangle[]
            //{
            //    new Rectangle(34, 683, 14, 33), new Rectangle(56, 684, 13, 32), new Rectangle(75, 685, 21, 31),
            //    new Rectangle(103, 683, 13, 33), new Rectangle(125, 684, 14, 32), new Rectangle(145, 685, 20, 32)
            //}, 0.15f);
            //playerSprite.AddAnimation("WALK_DOWN", new Rectangle[]
            //{
            //    new Rectangle(130, 15, 15, 33), new Rectangle(150, 17, 16, 31), new Rectangle(171, 14, 17, 34),
            //    new Rectangle(193, 15, 15, 33), new Rectangle(213, 17, 16, 31),
            //}, 0.15f);
            //playerSprite.AddAnimation("WALK_RIGHT", new Rectangle[]
            //{
            //    new Rectangle(126, 100, 19, 31), new Rectangle(151, 99, 14, 32), new Rectangle(174, 98, 13, 33),
            //    new Rectangle(194, 100, 21, 31), new Rectangle(221, 99, 13, 32), new Rectangle(242, 98, 14, 33),
            //}, 0.15f);

            //playerSprite = new AnimatedSprite(bounds.Location.ToVector2(), 6.0f);
            //playerSprite.LoadFromFile("Content/rsrc/spritesheets/configFiles/sora.txt");
            //playerSprite.SetAnimation("IDLE");
			
            PlayerSpeed = 8;

            if (controls.Length != 4)
            {
                throw new ArgumentException("Nur 4 Bewegungstasten");
            }
            this.controls = controls;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
			base.Draw(gameTime, spriteBatch);
			//playerSprite.Draw(gameTime, spriteBatch);
			PlayerInventory.Draw(gameTime, spriteBatch);
            itemFinder.Draw(gameTime, spriteBatch);

			//Debug
			PlayerDirection.Normalize();
			Primitives2D.DrawLine(spriteBatch, CollisionBox.Center.ToVector2(), CollisionBox.Center.ToVector2() + PlayerDirection*50, Color.Black);
        }

        public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);
			//playerSprite.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
			// TODO: playerSprite steuern(Animationen ändern und bewegen)
			base.Update(gameTime);

			ProcessInput(gameTime);
            itemFinder.Update(gameTime);

            //playerSprite.Update(gameTime);
        }

		public void ProcessInput(GameTime gameTime)
		{
			int[] anzahl = { 0, 0 };

			//Vector2 currentPosition = playerSprite.Position;

			PlayerDirection = Vector2.Zero;

			// Debug and dirty codeplacement
			if (InputManager.IsKeyPressed(Keys.Q))
			{
				PlayerInventory.NavigateItems(gameTime, Inventory.Direction.LEFT);
			}
			if (InputManager.IsKeyPressed(Keys.R))
			{
				PlayerInventory.NavigateItems(gameTime, Inventory.Direction.RIGHT);
			}
		


			if (InputManager.IsAnyKeyPressed(controls))
			{

				PreviousWorldPosition = WorldPosition;

				if (InputManager.IsKeyPressed(controls[0]))
				{
					++anzahl[0];
					//playerSprite.SetAnimation("WALK_UP");
					PlayerDirection += new Vector2(0, -1);
				}
				if (InputManager.IsKeyPressed(controls[1]))
				{
					++anzahl[0];
					//playerSprite.SetAnimation("WALK_DOWN");
					PlayerDirection += new Vector2(0, 1);
				}
				if (InputManager.IsKeyPressed(controls[2]))
				{
					++anzahl[1];
					//playerSprite.SetAnimation("WALK_RIGHT");
					PlayerDirection += new Vector2(1, 0);
				}
				if (InputManager.IsKeyPressed(controls[3]))
				{
					++anzahl[1];
					//playerSprite.SetAnimation("WALK_LEFT");
					PlayerDirection += new Vector2(-1, 0);
				}

				// Richtungsvektor Normalizieren
				if (PlayerDirection != Vector2.Zero)
					PlayerDirection.Normalize();

				//Snap to Grid
				WorldPosition +=  PlayerDirection * PlayerSpeed;

				// Funktion fixt unsere Position
				if (CollisionManager.IsCollisionAfterMove(this, true, true))
				{
                    List<Collision> intersects = CollisionManager.GetCollidablesOnCollision(this);
                    foreach (Collision item in intersects)
                    {
                        Console.WriteLine(item);
                        if (item is Item)
                        {
							((Item)item).PickUp(this);
                            
                        }
                    }
                }



                if ((anzahl[0] > 1) || (anzahl[1] > 1) || ((anzahl[0] == 0) && (anzahl[1] == 0)))
				{
					//playerSprite.Position = currentPosition;
					//playerSprite.SetAnimation("IDLE");
				}

				// Update Healthbar etc. was sich auf die neue Position bezieht
				OnMove();
			}

		}

		public override void OnGeometryCollision(IGeometryCollision collider)
		{
			base.OnGeometryCollision(collider);
			//Console.WriteLine("Player Collison Feedback");
		}
	}
}
