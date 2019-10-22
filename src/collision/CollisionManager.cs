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
                Primitives2D.DrawRectangle(spriteBatch, c.GeoHitbox, Color.BlanchedAlmond);


				Primitives2D.DrawCircle(spriteBatch, c.GeoHitbox.Center.ToVector2(), 5f, 10, Color.Red);
			}

		}

        public void LoadContent(ContentManager content)
        {
            
        }

        public static void AddCollidables(params Collidable[] collidables)
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

		public static void removeCollidable(Collidable item)
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
                        if (g1.GeoHitbox.Intersects(g2.GeoHitbox))
                        {
							
							//ResolveGeometryCollision(g1, g2);
							//g1.OnGeometryCollision(g2);
							//g2.OnGeometryCollision(g1);
						}
                    }
                }
            }
        }



		public static bool IsCollisionOnPosition(Collidable g1, bool fixMyCollision, bool resolveCollisionWithSliding)
		{


			foreach (Collidable g2 in collidables)
			{
				if (g1 != g2)
				{
					if (g1.GeoHitbox.Intersects(g2.GeoHitbox))
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
							while (IsCollisionOnPosition(g1, false, false))
							{
								// Math.Sign(0) == 0
								tempDirection.X = tempDirection.X - Math.Sign(tempDirection.X);
								tempDirection.Y = tempDirection.Y - Math.Sign(tempDirection.Y);
								
								if (tempDirection == Vector2.Zero)
									break;

								g1.GeoHitbox = new Rectangle((g1.PreviousWorldPosition + tempDirection).ToPoint(), g1.GeoHitbox.Size);
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
									g1.GeoHitbox = new Rectangle((bumpedPosition + new Vector2(tempDirection.X, 0)).ToPoint(), g1.GeoHitbox.Size);

									if (!IsCollisionOnPosition(g1, false, false))
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
									g1.GeoHitbox = new Rectangle((bumpedPosition + new Vector2(0, tempDirection.Y)).ToPoint(), g1.GeoHitbox.Size);

									if (!IsCollisionOnPosition(g1, false, false))
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

		public static List<Collidable> GetCollidablesOnCollision(Collidable c1)
		{
			List<Collidable> intersectingCollidables = new List<Collidable>();

			foreach (Collidable c2 in collidables)
			{
				if (c1 != c2)
				{
					if (c1.GeoHitbox.Intersects(c2.GeoHitbox))
					{
						intersectingCollidables.Add(c2);
					}
				}
			}


			return intersectingCollidables;
		}

        private void ResolveGeometryCollision(GeometryCollidable g1, GeometryCollidable g2)
        {
            Vector2 g1Shift = g1.WorldPosition - g1.PreviousWorldPosition;
            Vector2 g2Shift = g2.WorldPosition - g2.PreviousWorldPosition;


			if (g1Shift != Vector2.Zero)
            {
                Vector2 g1BackShift = Vector2.Zero;

				
				if (g1Shift.X < 0)
				{
					if (g2.GeoHitbox.Right > g1.GeoHitbox.Left)
						if (g2.GeoHitbox.Top < g1.PreviousWorldPosition.Y + g1.GeoHitbox.Height && g2.GeoHitbox.Bottom > g1.PreviousWorldPosition.Y)
							g1BackShift.X = g2.GeoHitbox.Right - g1.GeoHitbox.Left;

				}
				else if (g1Shift.X > 0)
				{
					if (g2.GeoHitbox.Left < g1.GeoHitbox.Right)
						if (g2.GeoHitbox.Top < g1.PreviousWorldPosition.Y + g1.GeoHitbox.Height && g2.GeoHitbox.Bottom > g1.PreviousWorldPosition.Y)
							g1BackShift.X = g2.GeoHitbox.Left - g1.GeoHitbox.Right;
				}

				if (g1Shift.Y < 0)
				{
					if (g2.GeoHitbox.Bottom > g1.GeoHitbox.Top)
						if (g2.GeoHitbox.Left < g1.PreviousWorldPosition.X + g1.GeoHitbox.Width && g1.PreviousWorldPosition.X < g2.GeoHitbox.Right)
							g1BackShift.Y = g2.GeoHitbox.Bottom - g1.GeoHitbox.Top;
				}
				else if (g1Shift.Y > 0)
				{
					if (g2.GeoHitbox.Top < g1.GeoHitbox.Bottom)
						if (g2.GeoHitbox.Left < g1.PreviousWorldPosition.X + g1.GeoHitbox.Width && g1.PreviousWorldPosition.X < g2.GeoHitbox.Right)
							g1BackShift.Y = g2.GeoHitbox.Top - g1.GeoHitbox.Bottom;
				}

				g1.WorldPosition += g1BackShift;

			}
            else if (g2Shift != Vector2.Zero)
            {
                Vector2 g2BackShift = Vector2.Zero;

				if (g2Shift.X < 0)
				{
					if (g1.GeoHitbox.Right > g2.GeoHitbox.Left)
						if (g1.GeoHitbox.Top < g2.PreviousWorldPosition.Y + g2.GeoHitbox.Height && g1.GeoHitbox.Bottom > g2.PreviousWorldPosition.Y)
							g2BackShift.X = g1.GeoHitbox.Right - g2.GeoHitbox.Left;
				}
				else if (g2Shift.X > 0)
				{
					if (g1.GeoHitbox.Left < g2.GeoHitbox.Right)
						if (g1.GeoHitbox.Top < g2.PreviousWorldPosition.Y + g2.GeoHitbox.Height && g1.GeoHitbox.Bottom > g2.PreviousWorldPosition.Y)
							g2BackShift.X = g1.GeoHitbox.Left - g2.GeoHitbox.Right;
				}

				if (g2Shift.Y < 0)
				{
					if (g1.GeoHitbox.Bottom > g2.GeoHitbox.Top)
						if (g1.GeoHitbox.Left < g2.PreviousWorldPosition.X + g2.GeoHitbox.Width && g2.PreviousWorldPosition.X < g1.GeoHitbox.Right)
							g2BackShift.Y = g1.GeoHitbox.Bottom - g2.GeoHitbox.Top;
				}
				else if (g2Shift.Y > 0)
				{
					if (g1.GeoHitbox.Top < g2.GeoHitbox.Bottom)
						if (g1.GeoHitbox.Left < g2.PreviousWorldPosition.X + g2.GeoHitbox.Width && g2.PreviousWorldPosition.X < g1.GeoHitbox.Right)
							g2BackShift.Y = g1.GeoHitbox.Top - g2.GeoHitbox.Bottom;
				}

				g2.WorldPosition += g2BackShift;
            }
        }

		public static List<GeometryCollidable> GetAllCollidableInRange(Vector2 position, Vector2 collisionSize)
		{
			
			List<GeometryCollidable> collidableList = new List<GeometryCollidable>();
			foreach (GeometryCollidable collidable in geometryCollidables)
			{
				if (collidable.GeoHitbox.Intersects(new Rectangle(position.ToPoint(), collisionSize.ToPoint())))
					collidableList.Add(collidable);
			}
			return collidableList;
		}

    } // End of Class
}
