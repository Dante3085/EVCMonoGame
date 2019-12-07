using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public abstract class InventoryItem : Item, scenes.IUpdateable, scenes.IDrawable
	{
		public Texture2D inventoryIcon;
		public String inventoryIconPath;

		public InventoryItem(Vector2 position, String inventoryIconPath) : 
			base(position, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN")
		{
			this.inventoryIconPath = inventoryIconPath;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!isPickedUp)
				base.Draw(gameTime, spriteBatch);
		}

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);

			inventoryIcon = content.Load<Texture2D>(inventoryIconPath);
		}

		public override void PickUp(Player player)
		{
			CollisionManager.RemoveCollidable(this, CollisionManager.itemCollisionChannel);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
