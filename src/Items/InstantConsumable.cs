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
	
		public int heal = 0;
		public int speed = 0;
		public int exp = 0;
		public int gold = 0;
		
		public bool permaStats;

		public InstantConsumable(Vector2 position) : base(position)
		{
		}

		public override void PickUp(Player player)
		{
			//Player Update Stats
			player.CurrentHp += heal;
			player.movementSpeed += speed;
			player.exp += exp;
			player.gold += gold;

			CollisionManager.RemoveCollidable(this, CollisionManager.itemCollisionChannel);

		}
	}
}