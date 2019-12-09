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
	public abstract class WeaponRiku : Weapon
	{
		public int stack = 1;
		public bool unlocked = false;
		public String weaponName;

		public bool Unlocked
		{
			get { return unlocked; }
			set { unlocked = value; }
		}

		public WeaponRiku(Vector2 position, String inventoryIconPath, String anmConfigFile, String idleAnim, String weaponName)
			: base
			(
				  position,
				  inventoryIconPath,
				  anmConfigFile,
				  idleAnim,
				  GameplayState.Lane.LaneTwo
			)
		{
			this.weaponName = weaponName;
		}
		
		public override void PickUp(Player player)
		{
			base.PickUp(player);
		}

		public override void ActivateSpecial(Player player, GameTime gameTime)
		{
			if (stack > 0)
				stack--;
			if (stack == 0)
				Unlocked = false;

		}

	}
}
