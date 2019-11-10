using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src;
using EVCMonoGame.src.collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src
{
	public abstract class Enemy : Character
	{

		private Vector2 aggroSize = new Vector2(400, 400);
		private Player target;
		List<Point> waypoints;
		private int agentMindestBreite;
		
		private Rectangle bounds;

		public Rectangle AggroBounds
		{
			get {
				Rectangle bounds = new Rectangle(0, 0, (int)aggroSize.X, (int)aggroSize.Y);
				bounds.X = geoHitbox.Center.X - bounds.Width / 2;
				bounds.Y = geoHitbox.Center.Y - bounds.Height / 2;
				return bounds;
			}
		}
		

		protected Enemy(Rectangle bounds) : base(bounds)
		{
			movementSpeed = 3f;
			agentMindestBreite = geoHitbox.Width;
			CollisionManager.AddCollidable(this, CollisionManager.enemyCollisionChannel);
			CollisionManager.AddCollidable(this, CollisionManager.obstacleCollisionChannel);
		}

		public bool moveTo()
		{
			return false;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			//playerSprite.Draw(gameTime, spriteBatch);

			//Debug
			movementDirection.Normalize();
			Primitives2D.DrawLine(spriteBatch, CollisionBox.Center.ToVector2(), CollisionBox.Center.ToVector2() + movementDirection * 50, Color.White);
			Primitives2D.DrawRectangle(spriteBatch, AggroBounds, Color.Red);

			if(waypoints != null)
				foreach (Point waypoint in waypoints)
				{
					Primitives2D.DrawRectangle(spriteBatch, new Rectangle(waypoint.X* agentMindestBreite, waypoint.Y* agentMindestBreite, agentMindestBreite, agentMindestBreite), Color.White);
				}
			
		}

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);
			//playerSprite.LoadContent(content);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Behaviour Tree replacement - todo: behaviour tree, der in Player range ein Grid anfordert und alle paar Ticks ein Path generiert
			if (CollisionManager.IsPlayerInRange(AggroBounds))
			{
				// Erzeuge Level Grid
				Grid grid = new Grid(new Rectangle(0, 0, 400, 400), agentMindestBreite);
				//Grid grid = new Grid(new Rectangle(CollisionBox.Center.X - 200, CollisionBox.Center.Y - 200, 400, 400), agentMindestBreite); //debug new implementation 

				// Erzeuge Path
				waypoints = grid.Pathfind(new Point((int)CollisionBox.Center.X / agentMindestBreite, (int)CollisionBox.Center.Y / agentMindestBreite), new Point((int)GameplayState.PlayerOne.CollisionBox.Center.X / agentMindestBreite, (int)GameplayState.PlayerOne.CollisionBox.Center.Y / agentMindestBreite) );
				//waypoints = debugGrid.PathfindTo(new Point((int)GameplayState.PlayerOne.CollisionBox.Center.X, (int)GameplayState.PlayerOne.CollisionBox.Center.Y) ); //debug new implementation 

				MoveToCharacter(gameTime, GameplayState.PlayerOne);
			}

			List<Player> players = SearchForPlayers();
			if (players.Count > 0)
			{
				//target = getNearestPlayer()
				target = players.ElementAt(0);
				//MoveToCharacter(gameTime, target);
			}

			//playerSprite.Update(gameTime);
		}

		public List<Player> SearchForPlayers()
		{
			return CollisionManager.GetAllPlayersInRange(AggroBounds);
		}
		

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
				if (waypoints.Count() > 0)
				{

					Vector2 nextWaypoint = waypoints[1].ToVector2() * agentMindestBreite;

			

					movementDirection = nextWaypoint - WorldPosition;

					if (Vector2.Distance(nextWaypoint, WorldPosition) < agentMindestBreite)
					{
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

			// Funktion fixt unsere Position
			if (CollisionManager.IsCollisionAfterMove(this, true, true))
			{
				// Besser wäre eig. eine Attack Range einrichten. To Do
				List<Player> players = CollisionManager.GetAllPlayersInRange(AttackHitbox);
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
