using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public class InventoryItem : Item, scenes.IUpdateable, scenes.IDrawable
	{

		public InventoryItem(Vector2 position) : base(position)
		{

		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{

		}

		public override void LoadContent(ContentManager content)
		{

		}

		public override void PickUp(Player player)
        {
            player.PlayerInventory.AddItem(this);
        }

		public override void Update(GameTime gameTime)
		{

		}
	}
}
