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
	public abstract class Trap : scenes.IUpdateable, scenes.IDrawable, CombatCollidable
	{
		protected Vector2 worldPosition;
		protected AnimatedSprite trapSprite;
		public enum TrapState
		{
			COOLDOWN,
			PREPARE,
			DO_DMG
		}
		public TrapState trapState;

		protected double cooldownTime = 2000;
		protected double prepareTime = 2000;
		protected double doDMGTime = 1000;

		protected double currentCooldownTime;
		protected double currentPrepareTime;
		protected double currentDoDMGTime;

		protected float scale = 5f;

		protected Rectangle dmgArea;
		protected int damageEachTick = 2;

		public bool active;
		public bool loop;

		#region Propertie
		public String SetAnimation
		{
			set => trapSprite.SetAnimation(value);
		}
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

		public Trap(Vector2 worldPosition, bool active, bool loop)
		{
			this.worldPosition = worldPosition;
			this.active = active;
			this.loop = loop;

			trapState = TrapState.COOLDOWN;
			currentCooldownTime = cooldownTime;
			currentPrepareTime = prepareTime;
			currentDoDMGTime = doDMGTime;

			dmgArea = new Rectangle(new Point((int)worldPosition.X, (int)worldPosition.Y - 40), new Point((int)(35 * scale), (int)(40 * scale)));

			// Trap Sprite
			trapSprite = new AnimatedSprite(worldPosition, scale);
			trapSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/fire_trap.anm.txt");
			trapSprite.SetAnimation("COOLDOWN");
		}


		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			trapSprite.Draw(gameTime, spriteBatch);
			if(DebugOptions.showAttackBounds)
				spriteBatch.DrawRectangle(dmgArea, Color.Red);
		}

		public virtual void LoadContent(ContentManager content)
		{
			trapSprite.LoadContent(content);
		}

		public virtual void Update(GameTime gameTime)
		{
			if (active)
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
		}

		public virtual void Cooldown(GameTime gameTime)
		{
			currentCooldownTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (currentCooldownTime <= 0)
			{
				currentCooldownTime = cooldownTime;
				trapState = TrapState.PREPARE;
				trapSprite.SetAnimation("PREPARE");
			}
		}
		public virtual void Prepare(GameTime gameTime)
		{
			currentPrepareTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (currentPrepareTime <= 0)
			{
				currentPrepareTime = prepareTime;
				trapState = TrapState.DO_DMG;
				trapSprite.SetAnimation("DO_DMG");
			}
		}
		public virtual void DoDMG(GameTime gameTime)
		{
			currentDoDMGTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (currentDoDMGTime <= 0)
			{
				currentDoDMGTime = doDMGTime;
				trapSprite.SetAnimation("COOLDOWN");

				if (loop)
					trapState = TrapState.COOLDOWN;
				else
					active = false;
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
			}
		}

		public void OnCombatCollision(CombatArgs combatArgs)
		{
			throw new NotImplementedException();
		}
	}
}
