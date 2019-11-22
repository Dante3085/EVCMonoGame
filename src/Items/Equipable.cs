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
	public class Equipable : Item, scenes.IUpdateable, scenes.IDrawable
	{

		public Equipable(Rectangle bounds) : base(bounds)
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
			throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{

		}
	}
}
