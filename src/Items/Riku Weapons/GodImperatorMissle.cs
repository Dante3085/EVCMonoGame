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
	public class GodImperatorMissle : WeaponRiku
	{

		public GodImperatorMissle(Vector2 position)
			: base
			(
				position,
				"rsrc/spritesheets/singleImages/arrow",
				"Content/rsrc/spritesheets/configFiles/magic_missile_white.anm.txt",
				"MAGIC_MISSILE_UP",
                "GodImperatorMissle"
            )
		{
			shopPrice = 100;
		}
		public override Item Copy()
		{
			return new GodImperatorMissle(WorldPosition);
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
