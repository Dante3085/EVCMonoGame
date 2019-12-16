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
using EVCMonoGame.src.Items;
using EVCMonoGame.src.statemachine;

namespace EVCMonoGame.src.characters
{
	public class Enemy : Character
	{
		#region Fields
		public Player target;
		public int agentMindestBreite;
		public Vector2 movementDirection;
		public Vector2 previousMovementDirection;

		// Pathfinding
		public static Pathfinder pathfinder;
		public List<Point> waypoints;
		public Vector2 lastWaypoint;
		public Vector2 nextWaypoint;
		public float forcePathfindTimer;
		public float currentForcePathfindTimer;

		// Stats
		public float attackSpeed = 1000.0f; // in mili
		public float attackDmg = 10;
		public float attackRange = 200;
		public float cooldownOnAttack = 0.0f; // in mili
		public bool isAttackOnCooldown = false;
		public float sightRange;


		// Drops
		public int exp;

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
				  position,
				  CombatantType.ENEMY
			)
		{
			this.exp = exp;

			sprite = new AnimatedSprite(position, 5.0f);

			forcePathfindTimer = 5000;

			sightRange = 600;
			CollisionBox = new Rectangle(WorldPosition.ToPoint(), new Point(100, 100)); // Sprite IDLE Bounds liefert keine Quadratische Hitbox sodass der Pathfinder nicht funktioniert
			agentMindestBreite = CollisionBox.Width;

			CollisionManager.AddCollidable(this, CollisionManager.enemyCollisionChannel);

			// Erzeuge Level Grid einmalig für alle Enemys vom selben Typen (solange keine dynamischen obstacles aktiv werden)
			if (pathfinder == null)
				pathfinder = new Pathfinder(new Rectangle(0, 0, 400, 400), agentMindestBreite);


			this.combatArgs.attacker = this;
			this.combatArgs.targetType = CombatantType.PLAYER;
			this.combatant = CombatantType.ENEMY;
		}

		#endregion

		#region IDrawable
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			if (!IsAlive) //Draw Death Anim //Danach aus Drawable entfernen
				return;
			//Debug

			if (DebugOptions.showPathfinding)
			{
				movementDirection.Normalize();
				Primitives2D.DrawLine(spriteBatch, CollisionBox.Center.ToVector2(), CollisionBox.Center.ToVector2() + movementDirection * 50, Color.White, 2);
				Primitives2D.DrawCircle(spriteBatch, CollisionBox.Center.ToVector2(), sightRange, 20, Color.Red, 2);


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
			/*
            if (!IsAlive)
                return;
			if (isAttackOnCooldown)
			{
				cooldownOnAttack -= gameTime.ElapsedGameTime.Milliseconds;
				if (cooldownOnAttack <= 0.0)
					isAttackOnCooldown = false;
			}
			// Behaviour Tree replacement - todo: behaviour tree, der in Player range ein Grid anfordert und alle paar Ticks ein Path generiert
			if (target != null)
			{
				//Grid grid = new Grid(new Rectangle(CollisionBox.Center.X - 200, CollisionBox.Center.Y - 200, 400, 400), agentMindestBreite); //debug new implementation 
				//waypoints = debugGrid.PathfindTo(new Point((int)GameplayState.PlayerOne.CollisionBox.Center.X, (int)GameplayState.PlayerOne.CollisionBox.Center.Y) ); //debug new implementation 
				MoveToCharacter(gameTime, target);
			} else
				target = CollisionManager.GetNearestPlayerInRange(this, sightRange);
           */
			stateManager.Update(gameTime);
		}
		#endregion



		public void MoveToCharacter(GameTime gameTime, Character character)
		{
			//ToDo
			// - bool return
			//
			//
			//
			previousMovementDirection = movementDirection;
			movementDirection = Vector2.Zero;


			PreviousWorldPosition = WorldPosition;

			// TODO: Einfacher und sauberer schreiben
			if (currentForcePathfindTimer > 0.0f && pathfinder.IsPositionInNavgrid(WorldPosition) || CollisionManager.IsBlockedRaycast(this, character, CollisionManager.obstacleCollisionChannel) && pathfinder.IsPositionInNavgrid(WorldPosition))
			{
				// Erzeuge Path
				waypoints = pathfinder.Pathfind(CollisionBox.Center, target.CollisionBox.Center);

				if (CollisionManager.IsBlockedRaycast(this, character, CollisionManager.obstacleCollisionChannel))
					currentForcePathfindTimer = forcePathfindTimer;


				if (waypoints != null && waypoints.Count() > 1)
				{
					if (nextWaypoint == Vector2.Zero)
						nextWaypoint = waypoints[0].ToVector2();


					if (nextWaypoint == lastWaypoint)
						nextWaypoint = waypoints[1].ToVector2();

					movementDirection = nextWaypoint - WorldPosition;

					if (Vector2.Distance(nextWaypoint, WorldPosition) <= movementSpeed * 2)
					{
						movementDirection = Vector2.Zero;
						WorldPosition = nextWaypoint;
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
					//if (!isAttackOnCooldown)
					//	Attack(target);
				}

			}





			// Update Healthbar etc. was sich auf die neue Position bezieht


		}

		public override void OnCombatCollision(CombatArgs combatArgs)
		{
			base.OnCombatCollision(combatArgs);
			if (combatArgs.victim == this)
			{
				sprite.overlayColorOverTime(Color.OrangeRed, TimeSpan.FromMilliseconds(200));
			}
			if (combatArgs.attacker != null && combatArgs.attacker is Player)
				target = (Player)combatArgs.attacker;
			/*else
				Console.WriteLine("Nicht Player");
                */
		}

		public override void OnDeath()
		{
			base.OnDeath();

			DropLoot();
		}

		public virtual void DropLoot()
		{
			Random random = new Random();

			for (int i = 0; i < 10; i++)
			{

				Vector2 rndCoinPosition = WorldPosition + new Vector2(random.Next(250), random.Next(250));
				InstantConsumable coin = new InstantConsumable(rndCoinPosition,
																"Content/rsrc/spritesheets/configFiles/coin.anm.txt",
																"COIN",
																GameplayState.Lane.LaneBoth);
				coin.gold = random.Next(1, 12);

				Vector2 rndExpBottlePosition = WorldPosition + new Vector2(random.Next(-250, 0), random.Next(-250, 0));
				InstantConsumable expBottle = new InstantConsumable(rndExpBottlePosition,
																		"Content/rsrc/spritesheets/configFiles/exp.anm.txt",
																		"EXP",
																		GameplayState.Lane.LaneBoth,
																		0.5f);
				expBottle.exp = random.Next(10, 21);

				Vector2 rndHealthorbPosition = WorldPosition + new Vector2(random.Next(-250, 250), random.Next(-250, 250));
				InstantConsumable healthorb = new InstantConsumable(rndHealthorbPosition, "Content/rsrc/spritesheets/configFiles/healthorb.anm.txt",
																	"IDLE", GameplayState.Lane.LaneBoth, 3);
				healthorb.heal = random.Next(10, 25);

				Vector2 rndHealthpotionPosition = WorldPosition + new Vector2(random.Next(-100, 100), random.Next(-100, 100));
				Healthpotion healthpotion = new Healthpotion(rndHealthpotionPosition);

				Scene.drawablesToAdd.AddRange(new scenes.IDrawable[]
				{
					coin,
					expBottle,
					healthorb,
					healthpotion,
				});

				Scene.updateablesToAdd.AddRange(new scenes.IUpdateable[]
				{
					coin,
					expBottle,
					healthorb,
					healthpotion,
				});
			}
		}
	}
}
