using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.gui;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.A.I;
using EVCMonoGame.src.states;
using C3.MonoGame;
using EVCMonoGame.src.input;
using Microsoft.Xna.Framework.Input;

namespace EVCMonoGame.src.characters
{
    public class Enemy : Character, scenes.IUpdateable, scenes.IDrawable
    {
		#region Fields
		protected Player target;
		protected int agentMindestBreite;
		protected Vector2 movementDirection;

		protected List<Point> waypoints;
		protected Vector2 lastWaypoint;
		protected Vector2 nextWaypoint;
		protected float forcePathfindTimer;
		protected float currentForcePathfindTimer;

		// Stats
		protected float attackSpeed = 1000.0f; // in mili
		protected float attackDmg = 10;
		protected float attackRange = 200;
		protected float cooldownOnAttack = 0.0f; // in mili
		protected bool isAttackOnCooldown = false;
		protected float aggroRange;

        protected int exp;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public Enemy
        (
            String name,
            int maxHp,
            int currentHp,
            int maxMp,
            int currentMp,
            int strength,
            int defense,
            int intelligence,
            int agility,
            float movementSpeed,
            Vector2 position,
            int exp
        ) 
            : base
            (
                  name,
                  maxHp, 
                  currentHp,
                  maxMp,
                  currentMp,
                  strength,
                  defense,
                  intelligence,
                  agility,
                  movementSpeed,
                  position
            )
        {
            this.exp = exp;

            sprite = new AnimatedSprite(position, 5.0f);

			forcePathfindTimer = 5000;

			aggroRange = 600;
			CollisionBox = new Rectangle(WorldPosition.ToPoint(), new Point(100, 100));	// Sprite IDLE Bounds liefert keine Quadratische Hitbox sodass der Pathfinder nicht funktioniert
			agentMindestBreite = CollisionBox.Width;

			CollisionManager.AddCollidable(this, CollisionManager.enemyCollisionChannel);
            // CollisionManager.AddCollidable(this, CollisionManager.combatCollisionChannel);
		}

		#endregion

		#region IDrawable
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			//Debug

			if (DebugOptions.showPathfinding)
			{
				movementDirection.Normalize();
				Primitives2D.DrawLine(spriteBatch, CollisionBox.Center.ToVector2(), CollisionBox.Center.ToVector2() + movementDirection * 50, Color.White, 2);
				Primitives2D.DrawCircle(spriteBatch, CollisionBox.Center.ToVector2(), aggroRange, 20, Color.Red, 2);
			

				if (waypoints != null)
					foreach (Point waypoint in waypoints)
					{
						Primitives2D.DrawRectangle(spriteBatch, new Rectangle(waypoint.X, waypoint.Y, agentMindestBreite, agentMindestBreite), Color.Black, 2);
					}
			}

			if (DebugOptions.showAttackRange)
				Primitives2D.DrawCircle(spriteBatch, CollisionBox.Center.ToVector2(), attackRange, 20, Color.Red);
		}

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);
		}
		#endregion

		#region Updateables
		public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			if (isAttackOnCooldown)
			{
				cooldownOnAttack -= gameTime.ElapsedGameTime.Milliseconds;
				if (cooldownOnAttack <= 0.0)
					isAttackOnCooldown = false;
			}

			target = CollisionManager.GetNearestPlayersInRange(this, aggroRange);

			// Behaviour Tree replacement - todo: behaviour tree, der in Player range ein Grid anfordert und alle paar Ticks ein Path generiert
			if (target != null)
			{
				// Erzeuge Level Grid
				Pathfinder pathfinder = new Pathfinder(new Rectangle(0, 0, 400, 400), agentMindestBreite);
				//Grid grid = new Grid(new Rectangle(CollisionBox.Center.X - 200, CollisionBox.Center.Y - 200, 400, 400), agentMindestBreite); //debug new implementation 

				// Erzeuge Path
				waypoints = pathfinder.Pathfind(CollisionBox.Center, target.CollisionBox.Center);
				//waypoints = debugGrid.PathfindTo(new Point((int)GameplayState.PlayerOne.CollisionBox.Center.X, (int)GameplayState.PlayerOne.CollisionBox.Center.Y) ); //debug new implementation 

				MoveToCharacter(gameTime, target);
			}

		}
		#endregion

		

		public void MoveToCharacter(GameTime gameTime, Character character)
		{
			//ToDo
			// - bool return
			//
			//
			//

			movementDirection = Vector2.Zero;


			PreviousWorldPosition = WorldPosition;


			if (currentForcePathfindTimer > 0.0f || CollisionManager.IsBlockedRaycast(this, character, CollisionManager.obstacleCollisionChannel))
			{
				if (CollisionManager.IsBlockedRaycast(this, character, CollisionManager.obstacleCollisionChannel))
					currentForcePathfindTimer = forcePathfindTimer;


				if (waypoints != null && waypoints.Count() > 1)
				{
					if (nextWaypoint == Vector2.Zero)
						nextWaypoint = waypoints[0].ToVector2();



					if (nextWaypoint == lastWaypoint)
						nextWaypoint = waypoints[1].ToVector2();

					movementDirection = nextWaypoint - WorldPosition;
					
					if (Vector2.Distance(nextWaypoint, WorldPosition) <= movementSpeed*2)
					{
						movementDirection = nextWaypoint - WorldPosition;
						lastWaypoint = nextWaypoint;
						nextWaypoint = Vector2.Zero;
						waypoints.RemoveAt(0);
					}
				}

				currentForcePathfindTimer = currentForcePathfindTimer - gameTime.ElapsedGameTime.Milliseconds;
			}
			else
			{
				movementDirection = character.WorldPosition - WorldPosition;
				nextWaypoint = lastWaypoint;
			}

			// Richtungsvektor Normalizieren
			if (movementDirection != Vector2.Zero)
				movementDirection.Normalize();

			//Snap to Grid
			WorldPosition += movementDirection * movementSpeed;
			sprite.WorldPosition = WorldPosition;

			// Funktion fixt unsere Position
			if (CollisionManager.IsCollisionAfterMove(this, true, true))
			{

				if (target != null)
				{
					if (!isAttackOnCooldown)
						Attack(target);
				}

			}





			// Update Healthbar etc. was sich auf die neue Position bezieht
			OnMove();

		}
	}
}
