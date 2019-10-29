using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.Items;

namespace EVCMonoGame.src.Items
{
	public class ItemFinder : Updateable
	{
		private Player owner;
		public ItemFinder(Player owner)
		{
			this.owner = owner;
		}

		public override void Update(GameTime gameTime)
		{
			
		}

		public Item[] searchItems()
		{
			return null;
		}
	}
}
