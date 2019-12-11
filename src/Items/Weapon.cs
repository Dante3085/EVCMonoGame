using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.states;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public abstract class Weapon : InventoryItem
	{
		public double cooldown; //in miliseconds
		public double lastSpecialAttackTime; //in miliseconds

		public Weapon(Vector2 position, String inventoryIconPath, String anmConfigFile, String idleAnim, GameplayState.Lane lane)
			: base
			(
				  position,
				  inventoryIconPath,
				  anmConfigFile,
				  idleAnim,
				  lane
			)
		{
		}
	
		public override void PickUp(Player player)
		{
			base.PickUp(player);

			player.PlayerInventory.AddWeapon(this);
		}

		public virtual void ActivateSpecial(Player player, GameTime gameTime)
		{
			if (!IsOnCooldown(gameTime))
			{
				lastSpecialAttackTime = gameTime.TotalGameTime.TotalMilliseconds;

				//Base Special Attack
			}
		}

		public bool IsOnCooldown(GameTime gameTime)
		{
			return gameTime.TotalGameTime.TotalMilliseconds <= lastSpecialAttackTime + cooldown ? true : true;
		}
	}
}
