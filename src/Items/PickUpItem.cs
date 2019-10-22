using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.collision;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.Items
{
	public class PickUpItem : Item
	{
		public struct Stats
		{
			public int heal;

		}

		Stats stats;
		public bool permaStats;

		public PickUpItem(Rectangle bounds, Stats stats) : base(bounds)
		{
			this.stats = stats;
		}

		public void PickUp(Player player)
		{
			player.Healthbar.CurrentHp += stats.heal;
			CollisionManager.removeCollidable(this);

		}


	}
}
