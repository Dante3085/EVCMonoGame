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
	class FireTrap : scenes.IUpdateable, scenes.IDrawable, CombatCollidable
	{
		private Vector2 worldPosition;
		private AnimatedSprite trapSprite;
		private enum TrapState
		{
			COOLDOWN,
			PREPARE,
			DO_DMG
		}
		private TrapState trapState;

		private double cooldownTime = 2000;
		private double prepareTime = 2000;
		private double doDMGTime = 1000;

		private double currentCooldownTime;
		private double currentPrepareTime;
		private double currentDoDMGTime;

		private float scale = 5f;

		private Rectangle dmgArea;
		private int damageEachTick = 2;

		#region Propertie
		public bool DoUpdate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public Rectangle HurtBounds => throw new NotImplementedException();

		public Rectangle AttackBounds => throw new NotImplementedException();

		public bool HasActiveAttackBounds => throw new NotImplementedException();

		public bool HasActiveHurtBounds => throw new NotImplementedException();

		public bool IsAlive => throw new NotImplementedException();

		public CombatArgs CombatArgs => throw new NotImplementedException();

		public bool FlaggedForRemove { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public CombatantType Combatant => CombatantType.TRAP;
		#endregion

		public FireTrap(Vector2 worldPosition)
		{
			this.worldPosition = worldPosition;
			trapState = TrapState.COOLDOWN;
			currentCooldownTime = cooldownTime;
			currentPrepareTime = prepareTime;
			currentDoDMGTime = doDMGTime;

			dmgArea = new Rectangle(worldPosition.ToPoint(), new Point((int)(32 * scale), (int)(32 * scale)));

			// Trap Sprite
			trapSprite = new AnimatedSprite(worldPosition, scale);
			trapSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/fire_trap.anm.txt");
			trapSprite.SetAnimation("COOLDOWN");
		}


		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			trapSprite.Draw(gameTime, spriteBatch);
			//if(DebugOptions.showAttackBounds)
			spriteBatch.DrawRectangle(dmgArea, Color.Red);
		}

		public void LoadContent(ContentManager content)
		{
			trapSprite.LoadContent(content);
		}

		public void Update(GameTime gameTime)
		{
			switch (trapState)
			{
				case TrapState.COOLDOWN:
					Cooldown(gameTime);
					break;
				case TrapState.PREPARE:
					Prepare(gameTime);
					break;
				case TrapState.DO_DMG:
					DoDMG(gameTime);
					break;
			}

			trapSprite.Update(gameTime);
		}

		public bool Cooldown(GameTime gameTime)
		{
			currentCooldownTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (currentCooldownTime <= 0)
			{
				currentCooldownTime = cooldownTime;
				trapState = TrapState.PREPARE;
				trapSprite.SetAnimation("PREPARE");

				return true;
			}
			else
			{
				return false;
			}
		}
		public bool Prepare(GameTime gameTime)
		{
			currentPrepareTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (currentPrepareTime <= 0)
			{
				currentPrepareTime = prepareTime;
				trapState = TrapState.DO_DMG;
				trapSprite.SetAnimation("DO_DMG");
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool DoDMG(GameTime gameTime)
		{
			currentDoDMGTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (currentDoDMGTime <= 0)
			{
				currentDoDMGTime = doDMGTime;
				trapState = TrapState.COOLDOWN;
				trapSprite.SetAnimation("COOLDOWN");
				return true;
			}
			else
			{
				foreach (CombatCollidable cc in CollisionManager.combatCollisionChannel)
				{
					if (dmgArea.Contains(cc.HurtBounds.Center))
					{
						CombatArgs combatArgs = new CombatArgs(this, cc, Combatant);
						combatArgs.damage = damageEachTick;
						cc.OnCombatCollision(combatArgs);
					}
				}

				return false;
			}
		}

		public void OnCombatCollision(CombatArgs combatArgs)
		{
			throw new NotImplementedException();
		}
	}
}
