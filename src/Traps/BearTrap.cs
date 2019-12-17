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
	class BearTrap : Trap
	{
		protected bool alreadyDidDMG = false;
		protected int dmg = 250;

		public BearTrap(Vector2 worldPosition) : base(worldPosition, true, false)
		{
			currentCooldownTime = 0;
			currentPrepareTime = 0;
			trapState = TrapState.DO_DMG;
			// Trap Sprite
			trapSprite = new AnimatedSprite(worldPosition - new Vector2(-30, -9 * scale), scale);
			trapSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/bear_trap.anm.txt");
			trapSprite.SetAnimation("COOLDOWN");
		}

		//public override void Update(GameTime gameTime)
		//{
		//	if (active)
		//	{

		//		trapSprite.Update(gameTime);
		//	}
		//}

		public override void DoDMG(GameTime gameTime)
		{
			if(!alreadyDidDMG)
			{
				alreadyDidDMG = false;
				foreach (CombatCollidable cc in CollisionManager.combatCollisionChannel)
				{
					if (dmgArea.Contains(cc.HurtBounds.Center) && cc.Combatant == CombatantType.ENEMY)
					{
						CombatArgs combatArgs = new CombatArgs(this, cc, Combatant);
						combatArgs.damage = dmg;
						cc.OnCombatCollision(combatArgs);
						alreadyDidDMG = true;
					}
				}
				if (alreadyDidDMG)
					trapSprite.SetAnimation("DO_DMG");
			}
		}


	}
}
