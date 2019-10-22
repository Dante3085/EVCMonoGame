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
    public class CollisionManager : Updateable, scenes.IDrawable
    {
        private static List<Collidable> collidables;
        private static List<GeometryCollidable> geometryCollidables;

		

		public bool DrawBoundingBoxes { get; set; } = true;

        public CollisionManager()
        {
            collidables = new List<Collidable>();
            geometryCollidables = new List<GeometryCollidable>();
		}

        public override void Update(GameTime gameTime)
        {
            CheckGeometryCollisions();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!DrawBoundingBoxes)
                return;

            foreach (Collidable c in collidables)
            {
                Primitives2D.DrawRectangle(spriteBatch, c.Bounds, Color.BlanchedAlmond);


				Primitives2D.DrawCircle(spriteBatch, c.Bounds.Center.ToVector2(), 5f, 10, Color.Red);
			}

		}

        public void LoadContent(ContentManager content)
        {
            
        }

        public void AddCollidables(params Collidable[] collidables)
        {
            foreach (Collidable c in collidables)
            {
                CollisionManager.collidables.Add(c);

                if (c is GeometryCollidable)
                {
                    geometryCollidables.Add((GeometryCollidable)c);
                }
            }
        }

		public void removeCollidable(Collidable item)
		{
			
			if (item is GeometryCollidable)
			{
				geometryCollidables.Remove((GeometryCollidable)item);
			}
			else
			{
				collidables.Remove(item);
			}
		}

        private void CheckGeometryCollisions()
        {
            foreach (GeometryCollidable g1 in geometryCollidables)
            {
                foreach (GeometryCollidable g2 in geometryCollidables)
                {
                    if (g1 == g2)
                    {
                        continue;
                    }
                    else
                    {
                        if (g1.Bounds.Intersects(g2.Bounds))
                        {
							
							//ResolveGeometryCollision(g1, g2);
							//g1.OnGeometryCollision(g2);
							//g2.OnGeometryCollision(g1);
						}
                    }
                }
            }
        }



		public static bool isCollisionOnPosition(GeometryCollidable g1, bool fixMyCollision, bool resolveCollisionWithSliding)
		{


			foreach (GeometryCollidable g2 in geometryCollidables)
			{
				if (g1 != g2)
				{
					if (g1.Bounds.Intersects(g2.Bounds))
					{
						if (fixMyCollision)
						{

							// Zurück gelegte Distanz aka auch Richtung
							Vector2 direction = g1.Position - g1.PreviousPosition;

							// Only Integers
							direction.X = (int)Math.Round(direction.X);
							direction.Y = (int)Math.Round(direction.Y);

							Vector2 tempDirection = direction;


							// Interpoliere Bewegung soweit raus, bis keine Collision mehr entsteht (Diagonale Achsenkollision)
							while (isCollisionOnPosition(g1, false, false))
							{
								// Math.Sign(0) == 0
								tempDirection.X = tempDirection.X - Math.Sign(tempDirection.X);
								tempDirection.Y = tempDirection.Y - Math.Sign(tempDirection.Y);
								
								if (tempDirection == Vector2.Zero)
									break;

								g1.Bounds = new Rectangle((g1.PreviousPosition + tempDirection).ToPoint(), g1.Bounds.Size);
							}

							// Fixed Position
							Vector2 bumpedPosition = g1.Position = g1.PreviousPosition + tempDirection;
							tempDirection = direction;

							//Slide entlang der Wand und überprüfe auf erneuter Collision
							if (resolveCollisionWithSliding)
							{
								// Teste X Achse
								while (tempDirection.X != 0)
								{

									tempDirection.X = tempDirection.X - Math.Sign(tempDirection.X);
									g1.Bounds = new Rectangle((bumpedPosition + new Vector2(tempDirection.X, 0)).ToPoint(), g1.Bounds.Size);

									if (!isCollisionOnPosition(g1, false, false))
									{
										g1.Position = bumpedPosition + new Vector2(tempDirection.X, 0);
										bumpedPosition = g1.Position;
										tempDirection = direction;
										break;
									}

								}

								// Teste Y Achse
								while (tempDirection.Y != 0)
								{

									tempDirection.Y = tempDirection.Y - Math.Sign(tempDirection.Y);
									g1.Bounds = new Rectangle((bumpedPosition + new Vector2(0, tempDirection.Y)).ToPoint(), g1.Bounds.Size);

									if (!isCollisionOnPosition(g1, false, false))
									{
										g1.Position = bumpedPosition + new Vector2(0, tempDirection.Y);
										bumpedPosition = g1.Position;
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

        private void ResolveGeometryCollision(GeometryCollidable g1, GeometryCollidable g2)
        {
            Vector2 g1Shift = g1.Position - g1.PreviousPosition;
            Vector2 g2Shift = g2.Position - g2.PreviousPosition;


			if (g1Shift != Vector2.Zero)
            {
                Vector2 g1BackShift = Vector2.Zero;

				
				if (g1Shift.X < 0)
				{
					if (g2.Bounds.Right > g1.Bounds.Left)
						if (g2.Bounds.Top < g1.PreviousPosition.Y + g1.Bounds.Height && g2.Bounds.Bottom > g1.PreviousPosition.Y)
							g1BackShift.X = g2.Bounds.Right - g1.Bounds.Left;

				}
				else if (g1Shift.X > 0)
				{
					if (g2.Bounds.Left < g1.Bounds.Right)
						if (g2.Bounds.Top < g1.PreviousPosition.Y + g1.Bounds.Height && g2.Bounds.Bottom > g1.PreviousPosition.Y)
							g1BackShift.X = g2.Bounds.Left - g1.Bounds.Right;
				}

				if (g1Shift.Y < 0)
				{
					if (g2.Bounds.Bottom > g1.Bounds.Top)
						if (g2.Bounds.Left < g1.PreviousPosition.X + g1.Bounds.Width && g1.PreviousPosition.X < g2.Bounds.Right)
							g1BackShift.Y = g2.Bounds.Bottom - g1.Bounds.Top;
				}
				else if (g1Shift.Y > 0)
				{
					if (g2.Bounds.Top < g1.Bounds.Bottom)
						if (g2.Bounds.Left < g1.PreviousPosition.X + g1.Bounds.Width && g1.PreviousPosition.X < g2.Bounds.Right)
							g1BackShift.Y = g2.Bounds.Top - g1.Bounds.Bottom;
				}

				g1.Position += g1BackShift;

			}
            else if (g2Shift != Vector2.Zero)
            {
                Vector2 g2BackShift = Vector2.Zero;

				if (g2Shift.X < 0)
				{
					if (g1.Bounds.Right > g2.Bounds.Left)
						if (g1.Bounds.Top < g2.PreviousPosition.Y + g2.Bounds.Height && g1.Bounds.Bottom > g2.PreviousPosition.Y)
							g2BackShift.X = g1.Bounds.Right - g2.Bounds.Left;
				}
				else if (g2Shift.X > 0)
				{
					if (g1.Bounds.Left < g2.Bounds.Right)
						if (g1.Bounds.Top < g2.PreviousPosition.Y + g2.Bounds.Height && g1.Bounds.Bottom > g2.PreviousPosition.Y)
							g2BackShift.X = g1.Bounds.Left - g2.Bounds.Right;
				}

				if (g2Shift.Y < 0)
				{
					if (g1.Bounds.Bottom > g2.Bounds.Top)
						if (g1.Bounds.Left < g2.PreviousPosition.X + g2.Bounds.Width && g2.PreviousPosition.X < g1.Bounds.Right)
							g2BackShift.Y = g1.Bounds.Bottom - g2.Bounds.Top;
				}
				else if (g2Shift.Y > 0)
				{
					if (g1.Bounds.Top < g2.Bounds.Bottom)
						if (g1.Bounds.Left < g2.PreviousPosition.X + g2.Bounds.Width && g2.PreviousPosition.X < g1.Bounds.Right)
							g2BackShift.Y = g1.Bounds.Top - g2.Bounds.Bottom;
				}

				g2.Position += g2BackShift;
            }
        }

		public static List<GeometryCollidable> GetAllCollidableInRange(Vector2 position, Vector2 collisionSize)
		{
			
			List<GeometryCollidable> collidableList = new List<GeometryCollidable>();
			foreach (GeometryCollidable collidable in geometryCollidables)
			{
				if (collidable.Bounds.Intersects(new Rectangle(position.ToPoint(), collisionSize.ToPoint())))
					collidableList.Add(collidable);
			}
			return collidableList;
		}

    } // End of Class
}
