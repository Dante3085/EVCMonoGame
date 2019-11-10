using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using C3.MonoGame;

using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.events;
using EVCMonoGame.src.input;
using Microsoft.Xna.Framework.Input;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.collision
{
    public static class CollisionManager
    {
        public static List<Collision> allCollisionsChannel = new List<Collision>();
		public static List<Collision> obstacleCollisionChannel = new List<Collision>();
		public static List<Collision> enemyCollisionChannel = new List<Collision>();
		public static List<Collision> itemCollisionChannel = new List<Collision>();
		public static List<Collision> playerCollisionChannel = new List<Collision>();

		private static byte[,] navGrid;
		private static int debugGridCellSize;



		public static bool DrawBoundingBoxes { get; set; } = true;
		
		

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!DrawBoundingBoxes)
                return;

            foreach (Collision c in allCollisionsChannel)
            {
                Primitives2D.DrawRectangle(spriteBatch, c.CollisionBox, Color.BlanchedAlmond);


				Primitives2D.DrawCircle(spriteBatch, c.CollisionBox.Center.ToVector2(), 5f, 10, Color.Red);
			}

			if (navGrid != null && InputManager.IsKeyPressed(Keys.X))
			{
				for (var i = 0; i < navGrid.GetLength(0); i++)
				{
					for (var j = 0; j < navGrid.GetLength(1); j++)
					{
						if (navGrid[i, j] == 1)
							Primitives2D.DrawRectangle(spriteBatch, new Rectangle(i * debugGridCellSize, j * debugGridCellSize, debugGridCellSize, debugGridCellSize), Color.Red);
						//else
						//	Primitives2D.DrawRectangle(spriteBatch, new Rectangle(i * debugAgentMindestBreite, j * debugAgentMindestBreite, debugAgentMindestBreite, debugAgentMindestBreite), Color.Green);
					}
				}
			}

		}
		
        public static void AddCollidables(bool excludeFromAllCollisonChannel = false, params Collision[] collidables)
        {
            foreach (Collision c in collidables)
            {
                CollisionManager.allCollisionsChannel.Add(c);

				if (c is Player && !playerCollisionChannel.Contains(c))
					playerCollisionChannel.Add((Collision)c);
            }
        }
		public static void AddCollidable(Collision collidable, List<Collision> collisionChannel, bool excludeFromAllCollisonChannel = false)
		{
			if(!excludeFromAllCollisonChannel && !allCollisionsChannel.Contains(collidable))
				allCollisionsChannel.Add(collidable);

			if (!collisionChannel.Contains(collidable))
				collisionChannel.Add((Collision)collidable);
		}

		public static void RemoveCollidable(Collision c, List<Collision> collisionChannel)
		{
			CollisionManager.allCollisionsChannel.Remove(c);
		}

		public static void CleanCollisonManager()
		{
			allCollisionsChannel.Clear();
			obstacleCollisionChannel.Clear();
			enemyCollisionChannel.Clear();
			itemCollisionChannel.Clear();
		}

		public static bool IsObstacleCollision(Collision g1)
		{
			foreach (Collision obstacle in obstacleCollisionChannel)
			{
				if (g1 != obstacle)
				{
					if (g1.CollisionBox.Intersects(obstacle.CollisionBox))
						return true;
				}
			}

			return false;
		}

		public static bool IsCollisionAfterMove(Collision g1, bool fixMyCollision, bool resolveCollisionWithSliding)
		{
            bool isCollision = false;
			foreach (Collision g2 in allCollisionsChannel)
			{
				if (g1 != g2)
				{
					if (g1.CollisionBox.Intersects(g2.CollisionBox))
					{
                        isCollision = true;

						if (fixMyCollision && IsObstacleCollision(g1))
						{

							// Zurück gelegte Distanz aka auch Richtung
							Vector2 direction = g1.WorldPosition - g1.PreviousWorldPosition;

							// Only Integers
							direction.X = (int)Math.Round(direction.X);
							direction.Y = (int)Math.Round(direction.Y);

							Vector2 tempDirection = direction;


							// Interpoliere Bewegung soweit raus, bis keine Collision mehr entsteht (Diagonale Achsenkollision)
							while (IsObstacleCollision(g1))
							{
								// Math.Sign(0) == 0
								tempDirection.X = tempDirection.X - Math.Sign(tempDirection.X);
								tempDirection.Y = tempDirection.Y - Math.Sign(tempDirection.Y);
								
								if (tempDirection == Vector2.Zero)
									break;

								g1.CollisionBox = new Rectangle((g1.PreviousWorldPosition + tempDirection).ToPoint(), g1.CollisionBox.Size);
							}

							// Fixed Position
							Vector2 bumpedPosition = g1.WorldPosition = g1.PreviousWorldPosition + tempDirection;
							tempDirection = direction;

							//Slide entlang der Wand und überprüfe auf erneuter Collision
							if (resolveCollisionWithSliding)
							{
								// Teste X Achse
								while (tempDirection.X != 0)
								{

									tempDirection.X = tempDirection.X - Math.Sign(tempDirection.X);
									g1.CollisionBox = new Rectangle((bumpedPosition + new Vector2(tempDirection.X, 0)).ToPoint(), g1.CollisionBox.Size);

									if (!IsObstacleCollision(g1))
									{
										g1.WorldPosition = bumpedPosition + new Vector2(tempDirection.X, 0);
										bumpedPosition = g1.WorldPosition;
										tempDirection = direction;
										break;
									}

								}

								// Teste Y Achse
								while (tempDirection.Y != 0)
								{

									tempDirection.Y = tempDirection.Y - Math.Sign(tempDirection.Y);
									g1.CollisionBox = new Rectangle((bumpedPosition + new Vector2(0, tempDirection.Y)).ToPoint(), g1.CollisionBox.Size);

									if (!IsObstacleCollision(g1))
									{
										g1.WorldPosition = bumpedPosition + new Vector2(0, tempDirection.Y);
										bumpedPosition = g1.WorldPosition;
										break;
									}

								}
							}
						}
					}
				}


			}

			return isCollision;
		}

		public static List<Collision> GetCollidablesOnCollision(Collision c1)
		{
			List<Collision> intersectingCollidables = new List<Collision>();

			foreach (Collision c2 in allCollisionsChannel)
			{
				if (c1 != c2)
				{
					if (c1.CollisionBox.Intersects(c2.CollisionBox))
					{
						intersectingCollidables.Add(c2);
					}
				}
			}


			return intersectingCollidables;
		}


		public static List<Collision> GetAllCollidablesByPosition(Vector2 position, Vector2 collisionSize, List<Collision> collisionChannel)
		{
			
			List<Collision> collidableList = new List<Collision>();
			foreach (Collision collidable in collisionChannel)
			{
				if (collidable.CollisionBox.Intersects(new Rectangle(position.ToPoint(), collisionSize.ToPoint())))
					collidableList.Add(collidable);
			}
			return collidableList;
		}

		public static List<Player> GetAllPlayersInRange(Rectangle bounds)
		{
			List<Player> intersectingPlayers = new List<Player>();

			foreach (Player player in playerCollisionChannel)
			{
				if (bounds.Intersects(player.CollisionBox))
				{
					intersectingPlayers.Add(player);
				}
			}

			return intersectingPlayers;
		}

		public static bool IsPlayerInRange(Rectangle bounds)
		{
			foreach (Player player in playerCollisionChannel)
			{
				if (bounds.Intersects(player.CollisionBox))
				{
					return true;
				}
			}
			return false;
		}

		///	<summary>
		///	Generiert ein 2D-Grid worin sämtliche Kollisionen auftreten
		///	</summary>
		///	<param name="levelWidth">Höhe des Grids</param>
		/// <param name="levelHeight">Breite des Grids</param>
		/// <param name="gridCellSize">Agent navigations breite. Bestimmt die größe der einzelnen Gridzellen.</param>
		public static byte[,] GenerateLevelNavGrid(int levelWidth, int levelHeight, int agentMindestBreite = 10)
		{
			debugGridCellSize = agentMindestBreite;

			navGrid = new byte[levelWidth, levelHeight];

			foreach (Collision gc in obstacleCollisionChannel)
			{
				if (!playerCollisionChannel.Contains(gc) && !enemyCollisionChannel.Contains(gc))
				{
					Point startPos = new Point((int)(gc.WorldPosition.X / agentMindestBreite), (int)(gc.WorldPosition.Y / agentMindestBreite)); // if in bounce fehlt

					int displacement = 0;

					float collisionPositionOffset = gc.WorldPosition.X - agentMindestBreite * startPos.X;
					float collisionGridWidth = agentMindestBreite;

					if (gc.CollisionBox.Width % agentMindestBreite > 0)
						collisionGridWidth = (gc.CollisionBox.Width / agentMindestBreite) * agentMindestBreite;

					if (collisionPositionOffset + gc.CollisionBox.Width - collisionGridWidth > agentMindestBreite)
					{
						displacement = 1;
					}
					//todo für Y

					for (int i = 0; i < Math.Ceiling((decimal)gc.CollisionBox.Width / agentMindestBreite) + displacement; i++)
					{
						for (int j = 0; j < Math.Ceiling((decimal)gc.CollisionBox.Height / agentMindestBreite); j++)
						{
							navGrid[startPos.X + i, startPos.Y + j] = 1;
						}
					}
				}


			}

			return navGrid;
		}


		///	<summary>
		///	Generiert ein 2D-Grid innerhalb einer Bounds worin sämtliche Kollisionen auftreten
		///	</summary>
		///	<param name="gridBounds">Bounds in der das Grid augezogen und in Gridzellen unterteilt wird. WorldPosition wird beachtet!</param>
		/// <param name="gridCellSize">Bestimmt die Breite der Zellen in dem die GridBounds unterteilt wird.</param>
		public static byte[,] GenerateNavGrid(Rectangle gridBounds, int gridCellSize = 10)
		{
			debugGridCellSize = gridCellSize;


			// Grid aufgeteilt in Zellen
			navGrid = new byte[gridBounds.Width / gridCellSize, gridBounds.Height / gridCellSize];



			List<Collision> obstacles = GetAllCollidablesByPosition(gridBounds.Location.ToVector2(), gridBounds.Size.ToVector2(), obstacleCollisionChannel);

			foreach (Collision gc in obstacles)
			{
				if (!playerCollisionChannel.Contains(gc) && !enemyCollisionChannel.Contains(gc))
				{



					Point startPos = new Point((int)(gc.WorldPosition.X / gridCellSize), (int)(gc.WorldPosition.Y / gridCellSize)); // if in bounce fehlt

					int displacement = 0;

					float collisionPositionOffset = gc.WorldPosition.X - gridCellSize * startPos.X;
					float collisionGridWidth = gridCellSize;

					if (gc.CollisionBox.Width % gridCellSize > 0)
						collisionGridWidth = (gc.CollisionBox.Width / gridCellSize) * gridCellSize;

					if (collisionPositionOffset + gc.CollisionBox.Width - collisionGridWidth > gridCellSize)
					{
						displacement = 1;
					}
					//todo für Y

					for (int i = 0; i < Math.Ceiling((decimal)gc.CollisionBox.Width / gridCellSize) + displacement; i++)
					{
						for (int j = 0; j < Math.Ceiling((decimal)gc.CollisionBox.Height / gridCellSize); j++)
						{
							navGrid[startPos.X + i, startPos.Y + j] = 1;
						}
					}
				}


			}

			return navGrid;
		}

	} // End of Class
}
