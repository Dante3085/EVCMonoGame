﻿using System;
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

namespace EVCMonoGame.src.collision
{
    public static class CollisionManager
    {
        private static List<Collision> collidableChannel = new List<Collision>();
        private static List<GeometryCollidable> geometryCollidableChannel = new List<GeometryCollidable>();

		

		public static bool DrawBoundingBoxes { get; set; } = true;
		
		

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!DrawBoundingBoxes)
                return;

            foreach (Collision c in collidableChannel)
            {
                Primitives2D.DrawRectangle(spriteBatch, c.CollisionBox, Color.BlanchedAlmond);


				Primitives2D.DrawCircle(spriteBatch, c.CollisionBox.Center.ToVector2(), 5f, 10, Color.Red);
			}

		}
		
        public static void AddCollidables(params Collision[] collidables)
        {
            foreach (Collision c in collidables)
            {
                CollisionManager.collidableChannel.Add(c);

                if (c is GeometryCollidable)
                {
                    geometryCollidableChannel.Add((GeometryCollidable)c);
                }
            }
        }

		public static void removeCollidable(Collision c)
		{
			CollisionManager.collidableChannel.Remove(c);

			if (c is GeometryCollidable)
			{
				geometryCollidableChannel.Remove((GeometryCollidable)c);
			}
		}

		public static void CleanCollisonManager()
		{
			collidableChannel.Clear();
			geometryCollidableChannel.Clear();
		}

		public static bool IsGeoCollision(Collision g1)
		{
			foreach (GeometryCollidable g2 in geometryCollidableChannel)
			{
				if (g1 != g2)
				{
					if (g1.CollisionBox.Intersects(g2.CollisionBox))
						return true;
				}
			}

			return false;
		}

		public static bool IsCollisionAfterMove(Collision g1, bool fixMyCollision, bool resolveCollisionWithSliding)
		{

			foreach (Collision g2 in collidableChannel)
			{
				if (g1 != g2)
				{
					if (g1.CollisionBox.Intersects(g2.CollisionBox))
					{
						if (fixMyCollision && g1 is GeometryCollidable && g2 is GeometryCollidable)
						{

							// Zurück gelegte Distanz aka auch Richtung
							Vector2 direction = g1.WorldPosition - g1.PreviousWorldPosition;

							// Only Integers
							direction.X = (int)Math.Round(direction.X);
							direction.Y = (int)Math.Round(direction.Y);

							Vector2 tempDirection = direction;


							// Interpoliere Bewegung soweit raus, bis keine Collision mehr entsteht (Diagonale Achsenkollision)
							while (IsGeoCollision(g1))
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

									if (!IsGeoCollision(g1))
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

									if (!IsGeoCollision(g1))
									{
										g1.WorldPosition = bumpedPosition + new Vector2(0, tempDirection.Y);
										bumpedPosition = g1.WorldPosition;
										break;
									}

								}
							}
						}
						
						return true;
					}
				}


			}

			return false;
		}

		public static List<Collision> GetCollidablesOnCollision(Collision c1)
		{
			List<Collision> intersectingCollidables = new List<Collision>();

			foreach (Collision c2 in collidableChannel)
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


		public static List<GeometryCollidable> GetAllCollidableInRange(Vector2 position, Vector2 collisionSize)
		{
			
			List<GeometryCollidable> collidableList = new List<GeometryCollidable>();
			foreach (GeometryCollidable collidable in geometryCollidableChannel)
			{
				if (collidable.CollisionBox.Intersects(new Rectangle(position.ToPoint(), collisionSize.ToPoint())))
					collidableList.Add(collidable);
			}
			return collidableList;
		}

    } // End of Class
}
