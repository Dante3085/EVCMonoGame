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

namespace EVCMonoGame.src.Items
{
	public class Shop : scenes.IUpdateable, scenes.IDrawable, Interactable
	{
		private List<Item> sellableItems;
		private Vector2 shopPosition;
		private GameplayState.Lane lane;

		// Input // Debug weil das Bugs verursacht wenn zwei spieler den allg. shop zeitgleich benutzen. InputHandling bitte im Spieler
		private int InteractThreshold = 2000;
		private double lastInteract = 0;
		private GameTime gameTime;

		private Item selledItem;

		private ContentManager contentManager;

		#region Properties
		public bool DoUpdate { get; set; }
		public Rectangle InteractableBounds {
			get => new Rectangle(shopPosition.ToPoint(), new Point(800, 400) );
			set { }
		}
		#endregion

		//what are you buying stranger?
		public Shop(Vector2 position, List<Item> sellableItems, GameplayState.Lane lane)
		{
			this.sellableItems = sellableItems;
			shopPosition = position;
			this.lane = lane;

			// Lege Items aus
			ArrangeItems();

			CollisionManager.AddInteractable(this);
		}


		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (Item sellableItem in sellableItems)
			{
				sellableItem.Draw(gameTime, spriteBatch);
			}
			if(selledItem != null)
				selledItem.Draw(gameTime, spriteBatch);
		}

		public void LoadContent(ContentManager content)
		{
			contentManager = content; // Für alle Items die zur Runtime erstellt werden

			foreach (Item sellableItem in sellableItems)
			{
				sellableItem.LoadContent(content);
			}
		}

		public void Update(GameTime gameTime)
		{
			this.gameTime = gameTime;

			foreach (Item sellableItem in sellableItems)
			{
				sellableItem.Update(gameTime);
			}

			if (selledItem != null)
				selledItem.Update(gameTime);
		}

		// Legt Items im Pattern vorm Shop aus
		public void ArrangeItems()
		{
			Vector2 itemPosition = Vector2.Zero;
			Vector2 offset = new Vector2(0, 100);

			foreach (Item sellableItem in sellableItems)
			{
				sellableItem.WorldPosition = shopPosition + itemPosition;
				itemPosition += offset;

				sellableItem.isInShop = true;
			}
		}

		public void Interact(Player player)
		{
			if (!IsShopForPlayer(player))
			{
				//Console.WriteLine("Shop nicht für dich verfügbar");
				return;
			}

			if (lastInteract + InteractThreshold < gameTime.TotalGameTime.TotalMilliseconds)
			{
				lastInteract = gameTime.TotalGameTime.TotalMilliseconds;

				foreach (Item sellableItem in sellableItems)
					if (CollisionManager.IsPlayerInArea(player.PlayerIndex, sellableItem.CollisionBox))
						if (CheckGold(player, sellableItem))
							SellItem(player, sellableItem);
						else
							Console.WriteLine("Nicht genug Gold");
			}
			
							
		}

		public void SellItem(Player buyer, Item selledItem)
		{
			buyer.PlayerInventory.Gold -= selledItem.shopPrice;
			InventoryItem spawnedItem = null;

			// Allgemeiner Shop
			if (lane == GameplayState.Lane.LaneBoth)
				spawnedItem = (UsableItem)selledItem.Copy();
			else
			{
				// Sora Shop
				if (lane == GameplayState.Lane.LaneOne)
					spawnedItem = (UsableItem)selledItem.Copy();

				// Riku Shop
				if (lane == GameplayState.Lane.LaneTwo)
					spawnedItem = (Weapon)selledItem.Copy();
			}

			Console.WriteLine("Verkauft: " + selledItem);
			spawnedItem.lane = buyer.lane;
			spawnedItem.LoadContent(contentManager);
			this.selledItem = spawnedItem; //dirty! - TODO bessere lösung
		}

		public bool CheckGold(Player player, Item item)
		{
			return player.PlayerInventory.Gold >= item.shopPrice ? true : false;
		}

		public bool IsShopForPlayer(Player player)
		{
			if (lane == GameplayState.Lane.LaneBoth)
				return true;
			else
			{
				// Verkaufe nur an Sora
				if (player == GameplayState.PlayerOne && lane == GameplayState.Lane.LaneOne)
					return true;

				// Verkaufe nur an Riku
				if (player == GameplayState.PlayerTwo && lane == GameplayState.Lane.LaneTwo)
					return true;
			}
			return false;
		}
	}
}
