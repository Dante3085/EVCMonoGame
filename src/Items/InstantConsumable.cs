using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.states;
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

		public InstantConsumable(Vector2 position, String anmConfigFile, String idleAnim, GameplayState.Lane lane, float scale = 1.0f) 
			: base
			(
				position,
				anmConfigFile,
				idleAnim,
				lane
			)
		{
			sprite.Scale = scale;
		}

		public override Item Copy()
		{
			return new InstantConsumable(WorldPosition, anmConfigFile, idleAnim, lane, sprite.Scale);
		}

		public override void PickUp(Player player)
		{
			//Player Update Stats
			player.CurrentHp += heal;
			if (player.CurrentHp > player.MaxHp)
			{
				player.CurrentHp = player.MaxHp;
			}


			player.movementSpeed += speed;
			player.exp += exp;
			player.PlayerInventory.Gold += gold;
			player.expBar.CurrentExp += exp;

			player.CheckLevelUp();

			if (gold > 0)
				player.ShowGold(true, 2000);

			CollisionManager.RemoveCollidable(this, CollisionManager.itemCollisionChannel);

		}
	}
}