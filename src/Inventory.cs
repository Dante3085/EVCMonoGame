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
using EVCMonoGame.src.events;
using EVCMonoGame.src.Items;
using Microsoft.Xna.Framework;

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

		// Items Inventory
		private List<Item> items;
		private Item activeItem;

		// Weapons Inventory
		private List<Weapon> weapons;
		private Item activeWeapon;

		// GUI
		private Vector2 inventoryPosition;
		private Vector2 itemSize;
		private int itemSpacing = 0;
		// Animation
		private bool isAnimating;
		private int animationDuration = 160; //in miliseconds
		private double animationElapsedTime = 0;
		private Vector2 animPrevPos;
		private Vector2 animGoalPos;
		private Easer animEaser;


		public Inventory(Player owner)
		{
			this.owner = owner;
			items = new List<Item>();
			weapons = new List<Weapon>();

			inventoryPosition = new Vector2(200, 600);
			itemSize = new Vector2(60, 60);

			// todo : travel inventory through scenes!
			starterInventory();
		}

		public void starterInventory()
		{

			activeItem = new Item(new Rectangle(0, 0, 0, 0));
			items.Add(activeItem);
			items.Add(new Item(new Rectangle(0, 0, 0, 0)));
			items.Add(new Item(new Rectangle(0, 0, 0, 0)));
			items.Add(new Item(new Rectangle(0, 0, 0, 0)));

		}

		public void addItem(Item item)
		{
			items.Add(item);
		}

		public void removeItem(Item item)
		{
			items.Remove(item);
		}

		public void addWeapon(Weapon weapon)
		{
			weapons.Add(weapon);
		}

		public void removeWeapon(Weapon weapon)
		{
			weapons.Remove(weapon);
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Basic Values für Schleifenverarbeitung
			int posActiveItem = 0;
			int anzItems = items.Count();


			if (activeItem != null) {
				posActiveItem = items.IndexOf(activeItem);
			}

			if (isAnimating)
			{
				animationElapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

				isAnimating = animationElapsedTime > animationDuration ? false : true;
			}

			//for (int i = posActiveItem; i < anzItems; i++)
			//{

			//	//items.ElementAt<Item>(i).getThumbnail();
			//	Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2((i - posActiveItem) * itemSize.X + (i - posActiveItem) * itemPadding, 0), itemSize, Color.White);
			//	Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2(posActiveItem * itemSize.X + posActiveItem * itemPadding, 0), itemSize, Color.Red);
			//}

			//for (int i = 0; i < posActiveItem; i++)
			//{
			//	//items.ElementAt<Item>(i + posActiveItem).getThumbnail();
			//	Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2((i + anzItems - posActiveItem) * itemSize.X + (i + anzItems - posActiveItem) * itemPadding, 0), itemSize, Color.Green);

			//	Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2(posActiveItem * itemSize.X + posActiveItem * itemPadding, 0), itemSize, Color.Red);
			//}

			// Debug For
			for (int i = 0; i < anzItems; i++)
			{

				//items.ElementAt<Item>(i).getThumbnail();
				Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2(i * itemSize.X + i * itemSpacing, 0), itemSize, Color.White);

				if (isAnimating)
				{
					animEaser.Update(gameTime);
					Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2(animEaser.CurrentValue, 0), itemSize, Color.Orange);
				}
				else
				{
					Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2(posActiveItem * itemSize.X + posActiveItem * itemSpacing, 0), itemSize, Color.Orange);
				}
				
			}
		}

		public void LoadContent(ContentManager content)
		{
		}


		public Item NavigateItems(GameTime gameTime, Direction direction)
		{

			if (isGUIBusy(gameTime))
				return activeItem;

			StartAnimation(direction);

			int currentPos = items.IndexOf(activeItem);
			currentPos = Utility.mod(currentPos + (int)direction, items.Count());
			activeItem = items.ElementAt<Item>(currentPos);


			return activeItem;
		}

		public void StartAnimation(Direction direction)
		{
			//Todo enum verarbeitung
			if (!isAnimating)
			{
				int posActiveItem = items.IndexOf(activeItem);
				animPrevPos = new Vector2(posActiveItem * itemSize.X + posActiveItem * itemSpacing, 0);
				int posAfterNavigation = Utility.mod(posActiveItem + (int)direction, items.Count());
				animGoalPos = new Vector2(posAfterNavigation * itemSize.X + posAfterNavigation * itemSpacing, 0);
				isAnimating = true;
				animationElapsedTime = 0.0d;

				// Easer
				animEaser = new Easer(animPrevPos.X, animGoalPos.X, animationDuration, Easing.LinearEaseIn);
				animEaser.start();
			}
		}

		public bool isGUIBusy(GameTime gameTime)
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
	}
}