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
using EVCMonoGame.src.animation;

namespace EVCMonoGame.src
{
    public class LaneTeleporter : Lever, scenes.IDrawable, Interactable, scenes.IUpdateable
	{

		private Vector2 position;
		private Player traveler;
		private Vector2 teleportGoal;
		private double laneTime;
		private enum TeleporterState
		{
			TeleportToLane,
			FightingOnLane,
			TeleportBack
		}
		TeleporterState teleporterState;
		public bool usedOnce = false;

		private AnimatedSprite portalSprite;
		private AnimatedSprite travelSprite;
		private bool drawTransition = false;
		private Easer travelEaser;
		private double laneTransitionTime = 1500;


		public LaneTeleporter(Vector2 position, Player traveler, Vector2 teleportGoal, double laneTime = 10000) : base(position)
		{
			this.position = position;
			this.traveler = traveler;
			this.teleportGoal = teleportGoal;
			this.laneTime = laneTime;

			teleporterState = new TeleporterState();

			// ´Portal Sprite
			portalSprite = new AnimatedSprite(Vector2.Zero, 7);
			portalSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/portal.anm.txt");
			portalSprite.SetAnimation("PORTAL_READY");
			portalSprite.WorldPosition = position - new Vector2(80, 100);

			// PlayerSpendGold Sprite
			travelSprite = new AnimatedSprite(Vector2.Zero);
			travelSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/coin.anm.txt");
			travelSprite.SetAnimation("COIN");

			//CollisionManager.AddCollidable(new GeometryBox(new Rectangle(teleportGoal.ToPoint(), new Point(200, 200))), CollisionManager.obstacleCollisionChannel);
		}

		public override void Interact(Player player)
		{
			if (usedOnce == false)
			{
				base.Interact(player);

				travelEaser = new Easer(traveler.WorldPosition, teleportGoal, (int)laneTransitionTime, Easing.LinearEaseNone);
				travelEaser.Start();
				teleporterState = TeleporterState.TeleportToLane;
			}

		}

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);

			travelSprite.LoadContent(content);
			portalSprite.LoadContent(content);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{

			if(drawTransition)
				travelSprite.Draw(gameTime, spriteBatch);

			portalSprite.Draw(gameTime, spriteBatch);
		}
		
		public void Update(GameTime gameTime)
		{
			portalSprite.Update(gameTime);

			if (!usedOnce && Activated)
			{

				switch (teleporterState)
				{
					case TeleporterState.TeleportToLane:

						if (TransitionBetweenLanes(gameTime))
							teleporterState = TeleporterState.FightingOnLane;

						break;
					case TeleporterState.FightingOnLane:

						if (LaneTime(gameTime))
							teleporterState = TeleporterState.TeleportBack;

						break;
					case TeleporterState.TeleportBack:

						if (TransitionBetweenLanes(gameTime))
						{
							portalSprite.SetAnimation("PORTAL_USED");
							usedOnce = true;
						}
						break;
				}
			}
		}

		public bool TransitionBetweenLanes(GameTime gameTime)
		{
			//Easer
			travelEaser.Update(gameTime);

			// Sprite
			travelSprite.WorldPosition = travelEaser.CurrentValue;
			travelSprite.Update(gameTime);
			drawTransition = true;

			//Player Position for Camera
			traveler.WorldPosition = travelEaser.CurrentValue;
			traveler.HasActiveHurtBounds = false;
			traveler.BlockInput = true;
			traveler.hidePlayer = true;

			if (travelEaser.IsFinished)
			{
				drawTransition = false;
				traveler.WorldPosition = travelEaser.CurrentValue;
				traveler.HasActiveHurtBounds = true;
				traveler.BlockInput = false;
				traveler.hidePlayer = false;
				return true;
			}
			else
				return false;
		}

		public bool LaneTime(GameTime gameTime)
		{
			if (laneTime > 0)
			{
				laneTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
				return false;
			}
			else
			{
				travelEaser.Reverse();
				travelEaser.From = traveler.WorldPosition;
				travelEaser.Start();
				return true;
			}
		}
	}
}
