using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.Traps
{
	class FireTrap : Trap
	{

		public FireTrap(Vector2 worldPosition, bool active = true, bool loop = true) : base(worldPosition, active, loop)
		{
			// Trap Sprite
			trapSprite = new AnimatedSprite(worldPosition, scale);
			trapSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/fire_trap.anm.txt");
			trapSprite.SetAnimation("COOLDOWN");
		}

		


	}
}
