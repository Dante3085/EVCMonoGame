using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.Items
{
	public class InstantConsumable : Item
	{
		public struct ItemStats
		{
			public int heal;
			public int speed;
		}

		public ItemStats stats;
		public bool permaStats;

		public InstantConsumable(Vector2 position) : base(position)
		{
		}

		public override void PickUp(Player player)
		{
			//Player Update Stats
			CollisionManager.RemoveCollidable(this, CollisionManager.itemCollisionChannel);
		}
	}
}