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
		private float aggroRange;
		private Player target;
		private int agentMindestBreite;
		Vector2 movementDirection;

		List<Point> waypoints;
		private Vector2 lastWaypoint;
		private Vector2 nextWaypoint;

		// Stats
		protected float attackSpeed = 1000.0f; // in mili
		protected float attackDmg = 10;
		protected float cooldownOnAttack = 0.0f; // in mili
		protected bool isAttackOnCooldown = false;


		#endregion

		#region Properties

		#endregion

		#region Constructors

		public Enemy(int maxHp, int currentHp, Vector2 position)
            : base(maxHp, currentHp, position)
        {
            sprite = new AnimatedSprite(position, 5.0f);

			aggroRange = 600;
			movementSpeed = 3f;
			CollisionBox = new Rectangle(WorldPosition.ToPoint(), new Point(100, 100));	// Sprite IDLE Bounds liefert keine Quadratische Hitbox sodass der Pathfinder nicht funktioniert
			agentMindestBreite = CollisionBox.Width;
			CollisionManager.AddCollidable(this, CollisionManager.enemyCollisionChannel);
		}

		#endregion

		#region IDrawable
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			//Debug

			if (InputManager.IsKeyPressed(Keys.X) || InputManager.IsButtonPressed(Buttons.RightTrigger))
			{
				movementDirection.Normalize();
				Primitives2D.DrawLine(spriteBatch, CollisionBox.Center.ToVector2(), CollisionBox.Center.ToVector2() + movementDirection * 50, Color.White, 2);
				Primitives2D.DrawCircle(spriteBatch, CollisionBox.Center.ToVector2(), aggroRange, 20, Color.Red, 2);
			

			if (waypoints != null)
				foreach (Point waypoint in waypoints)
				{
					Primitives2D.DrawRectangle(spriteBatch, new Rectangle(waypoint.X * agentMindestBreite, waypoint.Y * agentMindestBreite, agentMindestBreite, agentMindestBreite), Color.Black, 2);
				}
			}
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

			// Behaviour Tree replacement - todo: behaviour tree, der in Player range ein Grid anfordert und alle paar Ticks ein Path generiert
			if (CollisionManager.IsPlayerInRange(this, aggroRange))
			{
				// Erzeuge Level Grid
				Pathfinder pathfinder = new Pathfinder(new Rectangle(0, 0, 400, 400), agentMindestBreite);
				//Grid grid = new Grid(new Rectangle(CollisionBox.Center.X - 200, CollisionBox.Center.Y - 200, 400, 400), agentMindestBreite); //debug new implementation 

				// Erzeuge Path
				waypoints = pathfinder.Pathfind(new Point((int)CollisionBox.Center.X / agentMindestBreite, (int)CollisionBox.Center.Y / agentMindestBreite), new Point((int)GameplayState.PlayerOne.CollisionBox.Center.X / agentMindestBreite, (int)GameplayState.PlayerOne.CollisionBox.Center.Y / agentMindestBreite));
				//waypoints = debugGrid.PathfindTo(new Point((int)GameplayState.PlayerOne.CollisionBox.Center.X, (int)GameplayState.PlayerOne.CollisionBox.Center.Y) ); //debug new implementation 

				MoveToCharacter(gameTime, GameplayState.PlayerOne);
			}

			List<Player> players = CollisionManager.GetAllPlayersInRange(this, aggroRange);
			if (players.Count > 0)
			{
				//target = getNearestPlayer()
				target = players.ElementAt(0);
				//MoveToCharacter(gameTime, target);
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

			//todo: wenn collision zwischen Enemy und Spieler, dann Pathfinding aktivieren, ansonsten normal richtungsvektor
			bool usePathfinding = true;
			if (usePathfinding)
			{

				if (waypoints != null && waypoints.Count() > 1)
				{
					Vector2 nextWaypoint = waypoints[1].ToVector2() * agentMindestBreite;
					movementDirection = nextWaypoint - WorldPosition;
					movementDirection = nextWaypoint - WorldPosition;

					if (Vector2.Distance(nextWaypoint, WorldPosition) < agentMindestBreite)
						if (Vector2.Distance(nextWaypoint, WorldPosition) <= movementSpeed)
						{
							movementDirection = nextWaypoint - WorldPosition;
							lastWaypoint = nextWaypoint;
							nextWaypoint = Vector2.Zero;
							waypoints.RemoveAt(0);
						}
				}
			}
			else
				movementDirection = character.WorldPosition - WorldPosition;

			// Richtungsvektor Normalizieren
			if (movementDirection != Vector2.Zero)
				movementDirection.Normalize();

			//Snap to Grid
			WorldPosition += movementDirection * movementSpeed;
			sprite.WorldPosition = WorldPosition;

			// Funktion fixt unsere Position
			if (CollisionManager.IsCollisionAfterMove(this, true, true))
			{
				// Besser wäre eig. eine Attack Range einrichten. To Do
				List<Player> players = CollisionManager.GetAllPlayersInRange(this, aggroRange);
				if (players.Count > 0)
				{
					//target = getNearestPlayer()
					// Attack target and set Cooldown
					if (!isAttackOnCooldown)
						Attack(players.ElementAt(0));
					//else
					//	Console.WriteLine("Attack on Cooldown!");
				}

			}





			// Update Healthbar etc. was sich auf die neue Position bezieht
			OnMove();

		}

		public override void Attack(Character target)
		{

			Console.WriteLine("Attack!");

			isAttackOnCooldown = true;
			cooldownOnAttack = attackSpeed;
			target.OnDamage(attackDmg);
		}

	}
}
