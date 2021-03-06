﻿using System;
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
	public class PenetrateMissle : WeaponRiku
	{

		public PenetrateMissle(Vector2 position)
			: base
			(
				position,
				"rsrc/spritesheets/singleImages/Magic_Missile_Blue",
				"Content/rsrc/spritesheets/configFiles/magic_missile_blue.anm.txt",
				"MAGIC_MISSILE_UP",
				"PenetrateMissle"
			)
		{
			shopPrice = 8;
			sprite.Scale = 3;
			sprite.RescaleOffsets();
		}
		public override Item Copy()
		{
			return new PenetrateMissle(WorldPosition);
		}

		public override void ActivateSpecial(Player player, GameTime gameTime)
		{
			base.ActivateSpecial(player, gameTime);
		}

	}
}
