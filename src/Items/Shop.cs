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

namespace EVCMonoGame.src.Items
{
	public class Shop : scenes.IUpdateable, scenes.IDrawable, Interactable
	{
		private List<Item> sellableItems;
		private Vector2 shopPosition;
		private GameplayState.Lane lane;

		private GameTime gameTime;

		private List<Item> drawSelledItems;
		private Item activeItem;


		// Font
		protected SpriteFont font;
		public AnimatedSprite goldSprite;
		private bool drawGold;

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

			// Sprite
			goldSprite = new AnimatedSprite(Vector2.Zero);
			goldSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/coin.anm.txt");
			goldSprite.SetAnimation("COIN");

			// Lege Items aus
			ArrangeItems();

            drawSelledItems = new List<Item>();

			CollisionManager.AddInteractable(this);
		}

		public void LoadContent(ContentManager content)
		{
			contentManager = content; // Für alle Items die zur Runtime erstellt werden

			foreach (Item sellableItem in sellableItems)
			{
				sellableItem.LoadContent(content);
			}

			goldSprite.LoadContent(content);

			font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (Item sellableItem in sellableItems)
			{
				sellableItem.Draw(gameTime, spriteBatch);
			}
            for (int i = drawSelledItems.Count - 1; i >= 0; i--)
                drawSelledItems.ElementAt<Item>(i).Draw(gameTime, spriteBatch);

			if (drawGold)
			{
				goldSprite.Draw(gameTime, spriteBatch);
				spriteBatch.DrawString(font, "x" + activeItem.shopPrice, activeItem.WorldPosition + new Vector2(50, 75), Color.White);
			}
		}


		public void Update(GameTime gameTime)
		{
			this.gameTime = gameTime;

			activeItem = null;

            // Collision Update
			foreach (Item sellableItem in sellableItems)
			{
				sellableItem.Update(gameTime);
			}

			if (lane == GameplayState.Lane.LaneOne)
				activeItem = GetNearestItem(GameplayState.PlayerOne);
			if (lane == GameplayState.Lane.LaneTwo)
				activeItem = GetNearestItem(GameplayState.PlayerTwo);

			if (activeItem != null)
				drawGold = true;
			else
				drawGold = false;

            // Update Item das vom Spieler gekauft und gezogen wird
            for (int i = drawSelledItems.Count - 1; i >= 0; i--)
                if (drawSelledItems.ElementAt<Item>(i).isPickedUp)
                    drawSelledItems.RemoveAt(i);
                else
                    drawSelledItems.ElementAt<Item>(i).Update(gameTime);

            if(lane == GameplayState.Lane.LaneOne && CollisionManager.IsPlayerInArea(PlayerIndex.One, InteractableBounds))
            {
                GameplayState.PlayerOne.ShowGold(true);
            }

            if (lane == GameplayState.Lane.LaneTwo && CollisionManager.IsPlayerInArea(PlayerIndex.Two, InteractableBounds))
            {
                GameplayState.PlayerTwo.ShowGold(true);
            }

            if (lane == GameplayState.Lane.LaneBoth)
            {
                if(CollisionManager.IsPlayerInArea(PlayerIndex.One, InteractableBounds))
                    GameplayState.PlayerOne.ShowGold(true);

                if (CollisionManager.IsPlayerInArea(PlayerIndex.Two, InteractableBounds))
                    GameplayState.PlayerTwo.ShowGold(true);
            }

			if (drawGold)
			{
				goldSprite.Scale = 1f;
				goldSprite.WorldPosition = activeItem.WorldPosition + new Vector2(-10, 65);
				goldSprite.Update(gameTime);
			}
			else
				goldSprite.Scale = 0f;

		}

		// Legt Items im Pattern vorm Shop aus
		public void ArrangeItems()
		{
			Vector2 itemPosition = Vector2.Zero;
			Vector2 offset = new Vector2(150, 0);

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
            
            Item sellableItem = GetNearestItem(player);

            if (sellableItem != null)
				if (CheckGold(player, sellableItem))
					SellItem(player, sellableItem);
				else
					Console.WriteLine("Nicht genug Gold");
		}

        public Item GetNearestItem(Player player)
        {
            Item nearestItem = null;

            foreach (Item item in sellableItems)
            {
                if (item.CollisionBox.Intersects(player.CollisionBox))
                    if (nearestItem == null)
                        nearestItem = item;
                    else if (Vector2.Distance(player.CollisionBox.Center.ToVector2(), item.CollisionBox.Center.ToVector2()) < Vector2.Distance(player.CollisionBox.Center.ToVector2(), nearestItem.CollisionBox.Center.ToVector2()))
                        nearestItem = item;
            }

            return nearestItem;
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
			drawSelledItems.Add(spawnedItem); //dirty! - TODO bessere lösung
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
