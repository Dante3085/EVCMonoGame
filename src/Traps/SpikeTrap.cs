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
	class SpikeTrap : Trap
	{
		protected bool alreadyDidDMG = false;
		protected int dmg = 250;

		public SpikeTrap(Vector2 worldPosition, bool active = true, bool loop = true) : base(worldPosition, active, loop)
		{
			// Trap Sprite
			trapSprite = new AnimatedSprite(worldPosition - new Vector2(0, -9 * scale), scale);
			trapSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/spike_trap.anm.txt");
			trapSprite.SetAnimation("COOLDOWN");
		}

		public override void DoDMG(GameTime gameTime)
		{
			currentDoDMGTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (currentDoDMGTime <= 0)
			{
				currentDoDMGTime = doDMGTime;
				trapSprite.SetAnimation("COOLDOWN");
				alreadyDidDMG = false;
				if (loop)
					trapState = TrapState.COOLDOWN;
				else
					active = false;

			}
			else if(!alreadyDidDMG)
			{
				foreach (CombatCollidable cc in CollisionManager.combatCollisionChannel)
				{
					if (dmgArea.Contains(cc.HurtBounds.Center))
					{
						CombatArgs combatArgs = new CombatArgs(this, cc, Combatant);
						combatArgs.damage = dmg;
						cc.OnCombatCollision(combatArgs);
					}
				}
				alreadyDidDMG = true;
			}
		}


	}
}
