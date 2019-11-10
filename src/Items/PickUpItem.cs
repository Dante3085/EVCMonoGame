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
		public struct ItemStats
		{
			public int heal;
			public int speed;
		}

		public ItemStats stats;
		public bool permaStats;

		public PickUpItem(Rectangle bounds) : base(bounds)
		{
		}

		public override void PickUp(Player player)
		{
			player.Healthbar.CurrentHp += stats.heal;
			player.PlayerSpeed += stats.speed;
			CollisionManager.RemoveCollidable(this, CollisionManager.itemCollisionChannel);
			player.PlayerInventory.addItem(this);
		}
	}
}
