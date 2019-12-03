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

namespace EVCMonoGame.src
{
	public class Inventory : scenes.IDrawable
	{
		private Player owner;

		// Inventory Allgemeine Settings
		private int inputTimeThreshold = 160; //in milliseconds
		private double lastInputTime;

		public enum Direction
		{
			UP = -1,
			DOWN = 1,
			LEFT = -1,
			RIGHT = 1,
		}


		// Weapons Inventory
		private List<Weapon> weapons;
		private Weapon activeWeapon;

		// Items Inventory
		private List<UsableItem> usableItems;
		private UsableItem activeUsableItem;

		// GUI
		private Vector2 screenPosition;
		private Point itemSize;
		private int itemSpacing = 0;

		// Animation
		private bool isAnimating;
		private int animationDuration = 160; //in miliseconds
		private double animationElapsedTime = 0;
		private Direction animationDirection;
		private Vector2 animPrevPos;
		private Vector2 animGoalPos;
		private Easer animEaser;

		public int ActiveUsableItemArrayPos
		{
			get { return usableItems.IndexOf(activeUsableItem); }
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
				screenPosition = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 200, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 200);

			itemSize = new Point(60, 60);

			// todo : travel inventory through scenes!
			if(DebugOptions.spawnWithStarterItems)
				StarterItems();
		}

		public void StarterItems()
		{

			UsableItem inventoryItem = new UsableItem(new Vector2(1300, 3800), "rsrc/spritesheets/singleImages/arrow");
			UsableItem inventoryItem_2 = new UsableItem(new Vector2(1350, 3820), "rsrc/spritesheets/singleImages/boss_bee");
			UsableItem inventoryItem_3 = new UsableItem(new Vector2(1350, 3820), "rsrc/spritesheets/singleImages/boss_bee");
			UsableItem inventoryItem_4 = new UsableItem(new Vector2(1350, 3820), "rsrc/spritesheets/singleImages/coin-1");

			CollisionManager.RemoveCollidable(inventoryItem, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(inventoryItem_2, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(inventoryItem_3, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(inventoryItem_4, CollisionManager.itemCollisionChannel);

			AddUsableItem(inventoryItem);
			AddUsableItem(inventoryItem_2);
			AddUsableItem(inventoryItem_3);
			AddUsableItem(inventoryItem_4);

		}

		public void AddUsableItem(UsableItem item)
		{
			if (activeUsableItem == null)
			{
				activeUsableItem = item;
			}
			usableItems.Add(item);
		}

		public void RemoveUsableItem(UsableItem item)
		{
			if (activeUsableItem == item)
			{
				activeUsableItem = usableItems.ElementAt<UsableItem>(0);
			}
			usableItems.Remove(item);
		}

		public void AddWeapon(Weapon weapon)
		{
			weapons.Add(weapon);
		}

		public void RemoveWeapon(Weapon weapon)
		{
			weapons.Remove(weapon);
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Basic Values für Schleifenverarbeitung
			int posActiveItem = 0;
			int anzItems = usableItems.Count();


			if (activeUsableItem != null)
			{
				posActiveItem = usableItems.IndexOf(activeUsableItem);
			}

			if (isAnimating)
			{
				animationElapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
				isAnimating = animationElapsedTime > animationDuration ? false : true;

				// Change current item after animation finished
				if (!isAnimating)
					AfterAnimationFinished();
			}
			

			int mirror = 1;
			if (owner.PlayerIndex == PlayerIndex.Two)
				mirror = -1;


			if (isAnimating)
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

				}
				else
				{
					//Bei verschieben nach rechts rückt ein Item von Links hinein
					itemPosition = new Point(2 * itemSize.X + 2 * itemSpacing + (int)animEaser.CurrentValue.X, 0);
					Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * (0.3f * (1.0f - opacity)));

					//Draw Icon
					icon = usableItems.ElementAt<UsableItem>(Utility.Mod(GetPrevItemPos() + (int)Direction.LEFT, usableItems.Count())).inventoryIcon;
					spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * (0.3f * (1.0f - opacity)));



					//Draw Previous Item
					itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing + (int)animEaser.CurrentValue.X, 0);
					Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * (0.3f + 0.7f * (1.0f - opacity)));

					//Draw Icon
					icon = usableItems.ElementAt<UsableItem>(GetPrevItemPos()).inventoryIcon;
					spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * (0.3f + 0.7f * (1.0f - opacity)));

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
					}
					else if (animationDirection == Direction.LEFT &&  ActiveUsableItemArrayPos <  2 && i + 1 == anzItems)
					{
						// Draw Debug Inventory Grid
						Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * opacity);

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * opacity);

					} else
					{
						// Draw Debug Inventory Grid
						Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White);

						//Draw Icon
						icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
						spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 1.0f);

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
						}
						else
						{
							// Draw Debug Inventory Grid
							Primitives2D.DrawRectangle(spriteBatch, screenPosition + itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.Yellow);

							//Draw Icon
							icon = usableItems.ElementAt<UsableItem>(i).inventoryIcon;
							spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X + itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 1.0f);
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
				}
				else
				{
				}

			}
			else
			{
				//Draw Previous Item
				Point itemPosition = new Point(1 * itemSize.X + 1 * itemSpacing, 0);
				Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2() * new Vector2(mirror, mirror), itemSize.ToVector2(), Color.White * 0.3f);

				//Draw Icon
				Texture2D icon = usableItems.ElementAt<UsableItem>(GetPrevItemPos()).inventoryIcon;
				spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X * mirror, (int)screenPosition.Y), itemSize), Color.White * 0.3f);


				int itemPositionCounter = 0;

				if (ActiveUsableItemArrayPos == 0)
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
					}
				}
			}


			//Draw Selection Highlighter
			//Primitives2D.DrawRectangle(spriteBatch, screenPosition, itemSize.ToVector2(), Color.Orange, 2);
		}

		public void LoadContent(ContentManager content)
		{
			// Load Starter Items
			foreach (UsableItem item in usableItems)
				item.LoadContent(content);
			foreach (Weapon item in weapons)
				item.LoadContent(content);
		}


		public void NavigateItems(GameTime gameTime, Direction direction)
		{
			if (usableItems.Count() != 0)
			{
				if (!IsGUIBusy(gameTime))
					StartAnimation(direction);
			}
		}

		//TODO Für beide item listen
		public int GetPrevItemPos()
		{
			return Utility.Mod(usableItems.IndexOf(activeUsableItem) + (int)Direction.LEFT, usableItems.Count());
		}

		public int GetNextItemPos()
		{
			return Utility.Mod(usableItems.IndexOf(activeUsableItem) + (int)Direction.RIGHT, usableItems.Count());
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

		public bool IsGUIBusy(GameTime gameTime)
		{
			if (gameTime.TotalGameTime.TotalMilliseconds - lastInputTime > inputTimeThreshold)
			{
				lastInputTime = gameTime.TotalGameTime.TotalMilliseconds;
				return false;
			}
			else
			{
				return true;
			}
		}

		public void AfterAnimationFinished()
		{
			int currentPos = usableItems.IndexOf(activeUsableItem);
			currentPos = Utility.Mod(currentPos + (int)animationDirection, usableItems.Count());
			activeUsableItem = usableItems.ElementAt<UsableItem>(currentPos);
		}
	}
}