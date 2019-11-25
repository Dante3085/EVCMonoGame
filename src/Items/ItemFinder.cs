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

namespace EVCMonoGame.src.Items
{
    public class ItemFinder : scenes.IUpdateable, Collidable, scenes.IDrawable
    {
        private Player owner;
        private Inventory playerInventory;

        public Vector2 WorldPosition { get => owner.WorldPosition; set { } }

        public Vector2 PreviousWorldPosition { get => owner.WorldPosition; set { } }

        public Rectangle CollisionBox {
            get {
                return owner.CollisionBox;
            }
            set { }
        }

		public bool DoUpdate { get; set; }

		public ItemFinder(Player owner)
		{
			this.owner = owner;
			playerInventory = owner.PlayerInventory;
		}

		public void Update(GameTime gameTime)
		{
			if(CollisionManager.IsCollisionInArea(owner.CollisionBox, CollisionManager.itemCollisionChannel)); //TODO: ItemFinder Bounds die größer ist als PlayerBounds
			List<Collidable> foundItems = CollisionManager.GetAllCollidablesInArea(owner.CollisionBox, CollisionManager.itemCollisionChannel);
			foreach (Item item in foundItems)
			{
				item.PickUp(owner);
			}
		}

		public Item[] SearchItems()
		{
			return null;
		}

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (DebugOptions.showItemFinder)
            {
                Primitives2D.DrawRectangle(spriteBatch, CollisionBox, Color.Blue);
            }
        }

        public void LoadContent(ContentManager content)
        {
        }
    }
}
