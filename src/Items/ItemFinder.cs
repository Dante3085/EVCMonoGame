using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.Items;
using EVCMonoGame.src.collision;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.Items
{
    public class ItemFinder : scenes.IUpdateable, scenes.IDrawable
    {
        private Player owner;
        private Inventory playerInventory;
        private float finderRange;

        private List<Item> itemsToPullList;
        private float itemDraggingSpeed = 8.2f;

        public bool FlaggedForRemove
        {
            get; set;
        } = false;
        

		public bool DoUpdate { get; set; }

		public ItemFinder(Player owner)
		{
			this.owner = owner;
			playerInventory = owner.PlayerInventory;

            finderRange = 400f;

            itemDraggingSpeed += owner.MovementSpeed; 

            itemsToPullList = new List<Item>();
		}

		public void Update(GameTime gameTime)
		{
            if (CollisionManager.IsCollidableInRange(owner, finderRange, CollisionManager.itemCollisionChannel));
			List<Collidable> foundItems = CollisionManager.GetAllCollidablesInRange(owner, finderRange, CollisionManager.itemCollisionChannel);
			foreach (Item item in foundItems)
			{
				if (!item.isInShop)
				{
					// Nur aufheben wenn für sich bestimmt oder alle
					if (item.lane == GameplayState.Lane.LaneBoth || owner.lane == item.lane)
					{
						itemsToPullList.Add(item);
						item.PickUp(owner);
					}

				}
			}

            PullItemsNearPlayer();
        }

        public void PullItemsNearPlayer()
        {
            List<Item> removeFromPulling = new List<Item>();

            foreach(Item item in itemsToPullList)
            {

				if (Vector2.Distance(owner.CollisionBox.Center.ToVector2(), item.WorldPosition) < 25)
				{
					removeFromPulling.Add(item);
					item.isPickedUp = true;
				}
				else
				{
					Vector2 direction = owner.CollisionBox.Center.ToVector2() - item.WorldPosition;


					direction.Normalize();
					direction = new Vector2(direction.X * itemDraggingSpeed, direction.Y * itemDraggingSpeed);

					item.WorldPosition += direction;
				}
                
            }

            itemsToPullList = itemsToPullList.Except(removeFromPulling).ToList<Item>();
            removeFromPulling.Clear();
        }

		public Item[] SearchItems()
		{
			return null;
		}

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (DebugOptions.showItemFinder)
            {
                Primitives2D.DrawCircle(spriteBatch, owner.CollisionBox.Center.ToVector2(), finderRange, 12, Color.Blue);
            }
        }

        public void LoadContent(ContentManager content)
        {
        }
    }
}
