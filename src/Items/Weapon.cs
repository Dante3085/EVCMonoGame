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
		
		public override Item Copy()
		{
			return null;
		}

		public override void PickUp(Player player)
		{
			base.PickUp(player);

			player.PlayerInventory.AddWeapon(this);
		}
	}
}
