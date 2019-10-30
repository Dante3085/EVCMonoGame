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

namespace EVCMonoGame.src.Items
{
    public class ItemFinder : Updateable, Collision, scenes.IDrawable
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

        public ItemFinder(Player owner)
		{
			this.owner = owner;
			playerInventory = owner.PlayerInventory;
		}

		public override void Update(GameTime gameTime)
		{
			
		}

		public Item[] SearchItems()
		{
			return null;
		}

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Primitives2D.DrawRectangle(spriteBatch, CollisionBox, Color.Blue);
        }

        public void LoadContent(ContentManager content)
        {
        }
    }
}
