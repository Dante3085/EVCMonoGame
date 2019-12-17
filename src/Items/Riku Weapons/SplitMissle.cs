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
	public class SplitMissle : WeaponRiku
	{

		public SplitMissle(Vector2 position)
			: base
			(
				position,
				"rsrc/spritesheets/singleImages/Magic_Missile_Yellow",
				"Content/rsrc/spritesheets/configFiles/magic_missile_yellow.anm.txt",
				"MAGIC_MISSILE_UP",
                "SplitMissle"
            )
		{
			shopPrice = 11;
			sprite.Scale = 3;
			sprite.RescaleOffsets();
		}

		public override Item Copy()
		{
			return new SplitMissle(WorldPosition);
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
