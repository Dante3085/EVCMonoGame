using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.states;
using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.Traps;

namespace EVCMonoGame.src
{
    public class TrapRemote : Lever, scenes.IUpdateable
	{

		private Vector2 worldPosition;
		private List<Trap> remotableTrabs;

		private double interactionCooldown = 5000;
		private double currentInteractionCooldown;

		public TrapRemote(Vector2 worldPosition, List<Trap> remotableTrabs) : base(worldPosition)
		{
			this.worldPosition = worldPosition;
			this.remotableTrabs = remotableTrabs;


			foreach (Trap trap in remotableTrabs)
			{
				trap.active = false;
				trap.loop = false;
			}
		}

		public void Update(GameTime gameTime)
		{
			if (currentInteractionCooldown > 0)
				currentInteractionCooldown -= gameTime.ElapsedGameTime.TotalMilliseconds;
			else
				Activated = false;
		}

		public override void Interact(Player player)
		{
			if (currentInteractionCooldown <= 0)
			{
				base.Interact(player);

				foreach (Trap trap in remotableTrabs)
				{
					trap.trapState = Trap.TrapState.DO_DMG;
					trap.SetAnimation = Trap.TrapState.DO_DMG.ToString();
					trap.active = true;
				}

				currentInteractionCooldown = interactionCooldown;
			}
		}
	}
}
