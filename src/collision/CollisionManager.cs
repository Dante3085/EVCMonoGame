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

namespace EVCMonoGame.src.collision
{
    public class CollisionManager : Updateable, scenes.IDrawable
    {
        private List<Collidable> collidables;
        private List<GeometryCollidable> geometryCollidables;

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
            }
        }

        public void LoadContent(ContentManager content)
        {
            
        }

        public void AddCollidables(params Collidable[] collidables)
        {
            foreach (Collidable c in collidables)
            {
                this.collidables.Add(c);

                if (c is GeometryCollidable)
                {
                    geometryCollidables.Add((GeometryCollidable)c);
                }
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
                            ResolveGeometryCollision(g1, g2);
                        }
                    }
                }
            }
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
                    g1BackShift.X = g2.Bounds.Right - g1.Bounds.Left;
                }
                else if (g1Shift.X > 0)
                {
                    g1BackShift.X = g2.Bounds.Left - g1.Bounds.Right;
                }

                if (g1Shift.Y < 0)
                {
                    g1BackShift.Y = g2.Bounds.Bottom - g1.Bounds.Top;
                }
                else if (g1Shift.Y > 0)
                {
                    g1BackShift.Y = g2.Bounds.Top - g1.Bounds.Bottom;
                }

                g1.Position += g1BackShift;
            }
            else if (g2Shift != Vector2.Zero)
            {
                Vector2 g2BackShift = Vector2.Zero;
                if (g2Shift.X < 0)
                {
                    g2BackShift.X = g1.Bounds.Right - g2.Bounds.Left;
                }
                else if (g2Shift.X > 0)
                {
                    g2BackShift.X = g1.Bounds.Left - g2.Bounds.Right;
                }

                if (g2Shift.Y < 0)
                {
                    g2BackShift.Y = g1.Bounds.Bottom - g2.Bounds.Top;
                }
                else if (g2Shift.Y > 0)
                {
                    g2BackShift.Y = g1.Bounds.Top - g2.Bounds.Bottom;
                }

                g2.Position += g2BackShift;
            }
        }
    }
}
