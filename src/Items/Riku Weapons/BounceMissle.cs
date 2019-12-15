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
	public class BounceMissle : WeaponRiku
	{

		public BounceMissle(Vector2 position)
			: base
			(
				position,
				"rsrc/spritesheets/singleImages/coin-1",
				"Content/rsrc/spritesheets/configFiles/magic_missile_green.anm.txt",
				"MAGIC_MISSILE_UP",
                "BounceMissle"
            )
		{
			shopPrice = 2;
		}
		public override Item Copy()
		{
			return new BounceMissle(WorldPosition);
		}


		public override void PickUp(Player player)
		{
			base.PickUp(player);
		}

		public override void ActivateSpecial(Player player, GameTime gameTime)
		{
			base.ActivateSpecial(player, gameTime);
		}

	}
}
