using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using C3.MonoGame;
using EVCMonoGame.src.Items;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.states;
using EVCMonoGame.src.animation;

namespace EVCMonoGame.src
{
	public class Inventory : scenes.IDrawable
	{
		private Player owner;

		// Inventory Allgemeine Settings
		private int navigateTimeThreshold = 160; //in milliseconds
		private double lastNavigateTime;
		private int inputUseThreshold = 160; //in milliseconds
		private double lastUseTime;

		private int navigateTimeThresholdWeapons = 160; //in milliseconds
		private double lastNavigateTimeWeapons;
		private int inputUseThresholdWeapons = 160; //in milliseconds
		private double lastUseTimeWeapons;

		public enum Direction
		{
			UP = -1,
			DOWN = 1,
			LEFT = -1,
			RIGHT = 1,
		}

		private int gold;

        // Weapons Inventory
        public List<Weapon> weapons;
		protected Weapon activeWeapon;

		// Items Inventory
		protected List<UsableItem> usableItems;
		protected UsableItem activeUsableItem;

		// GUI
		protected Vector2 screenPosition;
		protected Point itemSize;
		protected int itemSpacing = 0;
		protected Vector2 usableItemAmmountDrawOffset;

		// Font
		protected SpriteFont font;

		// Animation UsableItem
		protected bool isAnimating;
		protected int animationDuration = 160; //in miliseconds
		protected double animationElapsedTime = 0;
		protected Direction animationDirection;
		protected Vector2 animPrevPos;
		protected Vector2 animGoalPos;
		protected Easer animEaser;

		// Animation Weapon
		protected bool isAnimatingWeapons;
		protected int animationDurationWeapons = 160; //in miliseconds
		protected double animationElapsedTimeWeapons = 0;
		protected Direction animationDirectionWeapons;
		protected Vector2 animPrevPosWeapons;
		protected Vector2 animGoalPosWeapons;
		protected Easer animEaserWeapons;

		#region Properties
		public int Gold { get; set; }
		#endregion

		public int ActiveUsableItemArrayPos
		{
			get { return (activeUsableItem != null) ? usableItems.IndexOf(activeUsableItem) : 0; }
			set { }
		}
		public int ActivWeaponArrayPos
		{
			get { return (activeWeapon != null) ? weapons.IndexOf(activeWeapon) : 0; }
			set { }
		}


		public Inventory(Player owner)
		{
            this.owner = owner;
			usableItems = new List<UsableItem>();
			weapons = new List<Weapon>();

			if (owner.PlayerIndex == PlayerIndex.One)
				screenPosition = new Vector2(200, 200);
			else
				screenPosition = new Vector2(Game1.GraphicsDeviceManager.PreferredBackBufferWidth - 200, Game1.GraphicsDeviceManager.PreferredBackBufferHeight - 200);
			
			itemSize = new Point(60, 60);
			usableItemAmmountDrawOffset = new Vector2(-20, -25);

			// todo : travel inventory through scenes!
			if (DebugOptions.spawnWithStarterItems)
				StarterItems();
		}

		public virtual void StarterItems()
		{
			// Items
			Healthpotion inventoryItem = new Healthpotion(new Vector2(1300, 3800));
			Healthpotion inventoryItem_2 = new Healthpotion(new Vector2(1350, 3820));
			UsableItem inventoryItem_3 = new GodMissleScroll(new Vector2(1350, 3820));
			UsableItem inventoryItem_4 = new GodMissleScroll(new Vector2(1350, 3820));

			CollisionManager.RemoveCollidable(inventoryItem, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(inventoryItem_2, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(inventoryItem_3, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(inventoryItem_4, CollisionManager.itemCollisionChannel);

			AddUsableItem(inventoryItem);
			AddUsableItem(inventoryItem_2);
			AddUsableItem(inventoryItem_3);
			AddUsableItem(inventoryItem_4);

		}

		public virtual void AddUsableItem(UsableItem item)
		{
			if (activeUsableItem == null)
			{
				activeUsableItem = item;
			}

			Console.WriteLine("Füge Item: " + item + " mit anzahl: " + item.stack + " hinzu");

			if (item.stackable)
			{
				// Stack else Add in List
				UsableItem itemInStock = usableItems.FirstOrDefault(i => i.itemName == item.itemName);

				if (itemInStock != null)
				{
					itemInStock.stack += item.stack;
				}
				else
				{
					usableItems.Add(item);
				}
			}
			else
				usableItems.Add(item);
		}
		public virtual void AddWeapon(Weapon weapon)
		{
			if (activeWeapon == null)
			{
				activeWeapon = weapon;
			}

			Console.WriteLine("Füge Weapon: " + weapon + " hinzu");
			weapons.Add(weapon);
		}

		public void RemoveUsableItem(UsableItem item)
		{
			if (activeUsableItem == item)
			{
				if (usableItems.Count() <= 1)
					activeUsableItem = null;
				else
					activeUsableItem = usableItems.ElementAt(GetNextItemPos());
			}
			usableItems.Remove(item);
		}
		public void RemoveWeapon(Weapon weapon)
		{
			if (activeWeapon == weapon)
			{
				if (weapons.Count() <= 1)
					activeWeapon = null;
				else
					activeWeapon = weapons.ElementAt(GetNextWeaponPos());
			}
			weapons.Remove(weapon);
		}

		public void UseActiveUsableItem(GameTime gameTime)
		{
			// Input Threshold
			if (gameTime.TotalGameTime.TotalMilliseconds - lastUseTime > inputUseThreshold)
			{
				lastUseTime = gameTime.TotalGameTime.TotalMilliseconds;

				if (activeUsableItem != null)
				{
					activeUsableItem.Use(owner);
					if (activeUsableItem.stack <= 0)
						RemoveUsableItem(activeUsableItem);
				}
			}
		}
		public virtual void ActivateSpecialAttack(GameTime gameTime, Weapon weapon = null)
		{
			// Input Threshold
			//if (gameTime.TotalGameTime.TotalMilliseconds - lastUseTimeWeapons > inputUseThresholdWeapons)
			//{
			//	lastUseTimeWeapons = gameTime.TotalGameTime.TotalMilliseconds;

			//	if (activeWeapon != null)
			//	{
			//		activeWeapon.ActivateSpecial(owner, gameTime);
			//	}
			//}

			if (weapon == null)
			{
				activeWeapon.ActivateSpecial(owner, gameTime);
			}
			else
			{
				weapon.ActivateSpecial(owner, gameTime);
			}
		}



		#region Draw
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Draw UsableItem Inventory
			if (isAnimating)
			{
				animationElapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
				isAnimating = animationElapsedTime > animationDuration ? false : true;

				// Change current item after animation finished
				if (!isAnimating)
					AfterAnimationFinished();
			}

			DrawUsableItemInventory(gameTime, spriteBatch);


			// Draw Weapon Inventory		
			if (isAnimatingWeapons)
			{
				animationElapsedTimeWeapons += gameTime.ElapsedGameTime.TotalMilliseconds;
				isAnimatingWeapons = animationElapsedTimeWeapons > animationDuration ? false : true;

				// Change current item after animation finished
				if (!isAnimatingWeapons)
					AfterAnimationFinishedWeapons();
			}

			DrawWeaponsInventory(gameTime, spriteBatch);
		}

		public void DrawUsableItemInventory(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Basic Values für Schleifenverarbeitung
			int anzItems = usableItems.Count();

			int mirror = 1;
			if (owner.PlayerIndex == PlayerIndex.Two)
				mirror = -1;

			// Draw UsableItem Inventory
			if (activeUsableItem != null)
			{
				if (isAnimating && anzItems > 1)
				{
					animEaser.Update(gameTime);

					Point itemPosition;
					Texture2D icon;


					float opacity = 0.3f;
					// Von 1 nach 0
					if (animEaser.CurrentValue.X != 0)
						opacity = (animEaser.To.X - animEaser.CurrentValue.X) / animEaser.To.X;


					if (animationDirection == Direction.RIGHT)
					{
						//Draw Previous Item
						itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing + (int)animEaser.CurrentValue.X, 0);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * 0.3f * opacity);

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(GetPrevItemPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 0.3f * opacity);

						//Draw Stack Ammount
						spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(GetPrevItemPos()).stack.ToString(), screenPosition - itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * 0.3f * opacity);
					}
					else
					{
						//Bei verschieben nach rechts rückt ein Item von Links hinein
						itemPosition = new Point(2 * itemSize.X + 2 * itemSpacing + (int)animEaser.CurrentValue.X, 0);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * (0.3f * (1.0f - opacity)));

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(Utility.Mod(GetPrevItemPos() + (int)Direction.LEFT, usableItems.Count())).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * (0.3f * (1.0f - opacity)));

						//Draw Stack Ammount
						spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(Utility.Mod(GetPrevItemPos() + (int)Direction.LEFT, usableItems.Count())).stack.ToString(), screenPosition - itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * (0.3f * (1.0f - opacity)));


						//Draw Previous Item
						itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing + (int)animEaser.CurrentValue.X, 0);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * (0.3f + 0.7f * (1.0f - opacity)));

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(GetPrevItemPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * (0.3f + 0.7f * (1.0f - opacity)));

						//Draw Stack Ammount
						spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(GetPrevItemPos()).stack.ToString(), screenPosition - itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * (0.3f + 0.7f * (1.0f - opacity)));
					}

					int itemPositionCounter = 0;

					if (ActiveUsableItemArrayPos == 0)
						anzItems--;

					for (int i = ActiveUsableItemArrayPos; i < anzItems; i++)
					{
						itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing - (int)animEaser.CurrentValue.X, 0);
						itemPositionCounter++;

						if (animationDirection == Direction.RIGHT && i == ActiveUsableItemArrayPos)
						{
							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * (1.0f - 0.7f * (1.0f - opacity)));

							//Draw Icon
							icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * (1.0f - 0.7f * (1.0f - opacity)));

							//Draw Stack Ammount
							spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(i).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * (1.0f - 0.7f * (1.0f - opacity)));

						}
						else if (animationDirection == Direction.LEFT && ActiveUsableItemArrayPos < 2 && i + 1 == anzItems)
						{
							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * opacity);

							//Draw Icon
							icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * opacity);

							//Draw Stack Ammount
							spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(i).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * opacity);


						}
						else
						{
							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White);

							//Draw Icon
							icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 1.0f);

							//Draw Stack Ammount
							spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(i).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * 1.0f);

						}

					}

					if (ActiveUsableItemArrayPos != 0)
					{
						for (int i = 0; i < GetPrevItemPos(); i++)
						{
							itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing - (int)animEaser.CurrentValue.X, 0);
							itemPositionCounter++;

							if (animationDirection == Direction.LEFT && i + 1 == GetPrevItemPos())
							{
								// Draw Debug Inventory Grid
								Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * opacity);

								//Draw Icon
								icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
								spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * opacity);

								//Draw Stack Ammount
								spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(i).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * opacity);
							}
							else
							{
								// Draw Debug Inventory Grid
								Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.Yellow);

								//Draw Icon
								icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
								spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 1.0f);

								//Draw Stack Ammount
								spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(i).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * 1.0f);
							}
						}
					}

					if (animationDirection == Direction.RIGHT)
					{
						//Draw Last Item
						itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing - (int)animEaser.CurrentValue.X, 0);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * (1.0f - opacity));

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(GetPrevItemPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * (1.0f - opacity));

						//Draw Stack Ammount
						spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(GetPrevItemPos()).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * (1.0f - opacity));
					}
					else
					{
					}

				}
				else
				{
					Point itemPosition;
					Texture2D icon;

					if (anzItems > 1)
					{
						//Draw Previous Item
						itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing, 0);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * 0.3f);

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(GetPrevItemPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 0.3f);

						//Draw Stack Ammount
						spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(GetPrevItemPos()).stack.ToString(), screenPosition - itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * 0.3f);
					}

					int itemPositionCounter = 0;

					if (ActiveUsableItemArrayPos == 0 && anzItems > 1)
						anzItems--;

					for (int i = ActiveUsableItemArrayPos; i < anzItems; i++)
					{
						itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing, 0);
						itemPositionCounter++;

						// Draw Debug Inventory Grid
						Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White);

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 1.0f);

						//Draw Stack Ammount
						spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(i).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White);

					}

					if (ActiveUsableItemArrayPos != 0)
					{
						for (int i = 0; i < GetPrevItemPos(); i++)
						{
							itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing, 0);
							itemPositionCounter++;

							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.Yellow);

							//Draw Icon
							icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 1.0f);

							//Draw Stack Ammount
							spriteBatch.DrawString(font, usableItems.ElementAt<UsableItem>(i).stack.ToString(), screenPosition + itemPosition.ToVector2() * mirror + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White);

						}
					}
				}

				//Draw Selection Highlighter
				Primitives2D.DrawRectangle(spriteBatch, screenPosition, itemSize.ToVector2(), Color.Orange, 2);
			}
			else
			{
				// Kein Item im Inventory
			}
		}
		public virtual void DrawWeaponsInventory(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Basic Values für Schleifenverarbeitung
			int anzWeapons = weapons.Count();

			Vector2 weaponInventoryOffset = new Vector2(0, 100);

			int mirror = 1;
			if (owner.PlayerIndex == PlayerIndex.Two)
				mirror = -1;

			if (activeWeapon != null)
			{
				if (isAnimatingWeapons && anzWeapons > 1)
				{
					animEaserWeapons.Update(gameTime);

					Point itemPosition;
					Texture2D icon;
					float opacity = 0.3f;
					
					// Von 1 nach 0
					if (animEaserWeapons.CurrentValue.X != 0)
						opacity = (animEaserWeapons.To.X - animEaserWeapons.CurrentValue.X) / animEaserWeapons.To.X;


					if (animationDirectionWeapons == Direction.RIGHT)
					{
						//Draw Previous Item
						itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing + (int)animEaserWeapons.CurrentValue.X, (int)weaponInventoryOffset.Y);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, 1), itemSize.ToVector2(), Color.White * 0.3f * opacity);

						//Draw Icon
						icon = weapons.ElementAt<Weapon>(GetPrevWeaponPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * 0.3f * opacity);
					}
					else
					{
						//Bei verschieben nach rechts rückt ein Item von Links hinein
						itemPosition = new Point(2 * itemSize.X + 2 * itemSpacing + (int)animEaserWeapons.CurrentValue.X, (int)weaponInventoryOffset.Y);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, 1), itemSize.ToVector2(), Color.White * (0.3f * (1.0f - opacity)));

						//Draw Icon
						icon = weapons.ElementAt<Weapon>(Utility.Mod(GetPrevWeaponPos() + (int)Direction.LEFT, weapons.Count())).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * (0.3f * (1.0f - opacity)));


						//Draw Previous Item
						itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing + (int)animEaserWeapons.CurrentValue.X, (int)weaponInventoryOffset.Y);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, 1), itemSize.ToVector2(), Color.White * (0.3f + 0.7f * (1.0f - opacity)));

						//Draw Icon
						icon = weapons.ElementAt<Weapon>(GetPrevWeaponPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * (0.3f + 0.7f * (1.0f - opacity)));
					}

					int itemPositionCounter = 0;

					if (ActivWeaponArrayPos == 0)
						anzWeapons--;

					for (int i = ActivWeaponArrayPos; i < anzWeapons; i++)
					{
						itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing - (int)animEaserWeapons.CurrentValue.X, (int)weaponInventoryOffset.Y);
						itemPositionCounter++;

						if (animationDirectionWeapons == Direction.RIGHT && i == ActivWeaponArrayPos)
						{
							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, -mirror), itemSize.ToVector2(), Color.White * (1.0f - 0.7f * (1.0f - opacity)));

							//Draw Icon
							icon = weapons.ElementAt<Weapon>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * (1.0f - 0.7f * (1.0f - opacity)));
						}
						else if (animationDirectionWeapons == Direction.LEFT && ActivWeaponArrayPos < 2 && i + 1 == anzWeapons)
						{
							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, -mirror), itemSize.ToVector2(), Color.White * opacity);

							//Draw Icon
							icon = weapons.ElementAt<Weapon>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * opacity);
						}
						else
						{
							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, -1), itemSize.ToVector2(), Color.White);

							//Draw Icon
							icon = weapons.ElementAt<Weapon>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * 1.0f);
						}

					}

					if (ActivWeaponArrayPos != 0)
					{
						for (int i = 0; i < GetPrevWeaponPos(); i++)
						{
							itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing - (int)animEaserWeapons.CurrentValue.X, (int)weaponInventoryOffset.Y);
							itemPositionCounter++;

							if (animationDirectionWeapons == Direction.LEFT && i + 1 == GetPrevWeaponPos())
							{
								// Draw Debug Inventory Grid
								Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, -mirror), itemSize.ToVector2(), Color.White * opacity);

								//Draw Icon
								icon = weapons.ElementAt<Weapon>(i).inventoryIcon;
								spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * opacity);
							}
							else
							{
								// Draw Debug Inventory Grid
								Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, -1), itemSize.ToVector2(), Color.Yellow);

								//Draw Icon
								icon = weapons.ElementAt<Weapon>(i).inventoryIcon;
								spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * 1.0f);
							}
						}
					}

					if (animationDirectionWeapons == Direction.RIGHT)
					{
						//Draw Last Item
						itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing - (int)animEaserWeapons.CurrentValue.X, (int)weaponInventoryOffset.Y);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, -mirror), itemSize.ToVector2(), Color.White * (1.0f - opacity));

						//Draw Icon
						icon = weapons.ElementAt<Weapon>(GetPrevWeaponPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * (1.0f - opacity));
					}
				}
				else
				{

					Point itemPosition;
					Texture2D icon;

					if (anzWeapons > 1)
					{
						//Draw Previous Item
						itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing, (int)weaponInventoryOffset.Y);
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, 1), itemSize.ToVector2(), Color.White * 0.3f);

						//Draw Icon
						icon = weapons.ElementAt<Weapon>(GetPrevWeaponPos()).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * 0.3f);
					}

					int itemPositionCounter = 0;

					if (ActivWeaponArrayPos == 0 && anzWeapons > 1)
						anzWeapons--;

					for (int i = ActivWeaponArrayPos; i < anzWeapons; i++)
					{
						itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing, (int)weaponInventoryOffset.Y);
						itemPositionCounter++;

						// Draw Debug Inventory Grid
						Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(-mirror, 1), itemSize.ToVector2(), Color.White);

						//Draw Icon
						icon = weapons.ElementAt<Weapon>(i).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * 1.0f);


					}

					if (ActivWeaponArrayPos != 0)
					{
						for (int i = 0; i < GetPrevWeaponPos(); i++)
						{
							itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing, (int)weaponInventoryOffset.Y);
							itemPositionCounter++;

							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(-mirror, 1), itemSize.ToVector2(), Color.Yellow);

							//Draw Icon
							icon = weapons.ElementAt<Weapon>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y - itemPosition.Y * mirror), itemSize), Color.White * 1.0f);
						}
					}
				}

				//Draw Selection Highlighter
				Primitives2D.DrawRectangle(spriteBatch, screenPosition - weaponInventoryOffset, itemSize.ToVector2(), Color.Orange, 2);
			}
		}
		#endregion
		public void LoadContent(ContentManager content)
		{
			// Load Starter Items
			foreach (UsableItem item in usableItems)
				item.LoadContent(content);
			foreach (Weapon item in weapons)
				item.LoadContent(content);

			font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

		}


		public void NavigateItems(GameTime gameTime, Direction direction)
		{
			if (owner.PlayerIndex == PlayerIndex.Two)
				if (direction == Direction.LEFT)
					direction = Direction.RIGHT;
				else if (direction == Direction.RIGHT)
					direction = Direction.LEFT;

			if (usableItems.Count() != 0)
			{
				if (!IsGUIBusy(gameTime))
					StartAnimation(direction);
			}
		}
		public void NavigateWeapons(GameTime gameTime, Direction direction)
		{
			if (owner.PlayerIndex == PlayerIndex.Two)
				if (direction == Direction.LEFT)
					direction = Direction.RIGHT;
				else if (direction == Direction.RIGHT)
					direction = Direction.LEFT;

			if (weapons.Count() != 0)
			{
				if (!IsWeaponsGUIBusy(gameTime))
					StartAnimationWeapons(direction);
			}
		}

		public int GetPrevItemPos()
		{
			return Utility.Mod(usableItems.IndexOf(activeUsableItem) + (int)Direction.LEFT, usableItems.Count());
		}
		public int GetNextItemPos()
		{
			return Utility.Mod(usableItems.IndexOf(activeUsableItem) + (int)Direction.RIGHT, usableItems.Count());
		}

		public int GetPrevWeaponPos()
		{
			return Utility.Mod(weapons.IndexOf(activeWeapon) + (int)Direction.LEFT, weapons.Count());
		}
		public int GetNextWeaponPos()
		{
			return Utility.Mod(weapons.IndexOf(activeWeapon) + (int)Direction.RIGHT, weapons.Count());
		}

		public void StartAnimation(Direction direction)
		{
			//Todo enum verarbeitung
			if (!isAnimating)
			{
				animPrevPos = Vector2.Zero;
				animGoalPos = itemSize.ToVector2() * new Vector2((float)direction, 0);

				isAnimating = true;
				animationElapsedTime = 0.0d;
				animationDirection = direction;

				// Easer
				animEaser = new Easer(animPrevPos, animGoalPos, animationDuration, Easing.LinearEaseIn);
				animEaser.Start();
			}
		}
		public void StartAnimationWeapons(Direction direction)
		{
			//Todo enum verarbeitung
			if (!isAnimatingWeapons)
			{
				animPrevPosWeapons = Vector2.Zero;
				animGoalPosWeapons = itemSize.ToVector2() * new Vector2((float)direction, 0);

				isAnimatingWeapons = true;
				animationElapsedTimeWeapons = 0.0d;
				animationDirectionWeapons = direction;

				// Easer
				animEaserWeapons = new Easer(animPrevPosWeapons, animGoalPosWeapons, animationDurationWeapons, Easing.LinearEaseIn);
				animEaserWeapons.Start();
			}
		}

		public bool IsGUIBusy(GameTime gameTime)
		{
			if (gameTime.TotalGameTime.TotalMilliseconds - lastNavigateTime > navigateTimeThreshold)
			{
				lastNavigateTime = gameTime.TotalGameTime.TotalMilliseconds;
				return false;
			}
			else
			{
				return true;
			}
		}
		public bool IsWeaponsGUIBusy(GameTime gameTime)
		{
			if (gameTime.TotalGameTime.TotalMilliseconds - lastNavigateTimeWeapons > navigateTimeThresholdWeapons)
			{
				lastNavigateTimeWeapons = gameTime.TotalGameTime.TotalMilliseconds;
				return false;
			}
			else
			{
				return true;
			}
		}

		public void AfterAnimationFinished()
		{
			if (activeUsableItem != null)
			{
				int currentPos = usableItems.IndexOf(activeUsableItem);
				currentPos = Utility.Mod(currentPos + (int)animationDirection, usableItems.Count());
				activeUsableItem = usableItems.ElementAt<UsableItem>(currentPos);
			}
		}
		public void AfterAnimationFinishedWeapons()
		{
			if (activeWeapon != null)
			{
				int currentPos = weapons.IndexOf(activeWeapon);
				currentPos = Utility.Mod(currentPos + (int)animationDirectionWeapons, weapons.Count());
				activeWeapon = weapons.ElementAt<Weapon>(currentPos);
			}
		}



    }
}