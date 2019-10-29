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
		private double inputTimeThreshold = 140; //in milliseconds
		private double lastInputTime;
		// Items Inventory
		private List<Item> items;
		private Item activeItem;

		// Weapons Inventory
		private List<Weapon> weapons;
		private Item activeWeapon;

		// GUI
		private Vector2 inventoryPosition;
		private Vector2 itemSize;

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
			int itemPadding = 5;

			
			if (activeItem != null) {
				posActiveItem = items.IndexOf(activeItem);
			}

			for (int i = posActiveItem; i < anzItems; i++)
			{
				Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2(i * itemSize.X + i * itemPadding, 0), itemSize, Color.White);
				Primitives2D.DrawRectangle(spriteBatch, inventoryPosition + new Vector2(posActiveItem * itemSize.X + posActiveItem * itemPadding, 0), itemSize, Color.Red);
			}
		}

		public void LoadContent(ContentManager content)
		{
		}


		public Item NavigateItemsLeft(GameTime gameTime)
		{

			if (isGUIBusy(gameTime))
				return activeItem;

			int currentPos = items.IndexOf(activeItem);
			currentPos = Utility.mod(--currentPos,items.Count());
			activeItem = items.ElementAt<Item>(currentPos);


			return activeItem;
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