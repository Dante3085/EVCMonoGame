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
    public enum Corner
    {
        LEFT_TOP = 1,
        RIGHT_TOP,
        RIGHT_BOTTOM,
        LEFT_BOTTOM
    }

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
            Vector2 g1CollisionPosition = new Vector2(g1.Position.X,g1.Position.Y);
            Vector2 g2CollisionPosition = new Vector2(g2.Position.X, g2.Position.Y);
            if (g1Shift != Vector2.Zero)
            {
                float length = 0.5f;
                Vector2 backShift = g1Shift * (-1);
                while (g1.Bounds.Intersects(g2.Bounds))
                {
                    backShift = Utility.scaleVectorTo(backShift, length);
                    g1.Position = g1CollisionPosition + backShift;
                    length += 0.5f;
                }
                g1.Position += new Vector2((g1CollisionPosition - g1.Position).X, 0);
                if (g1.Bounds.Intersects(g2.Bounds)) g1.Position = g1.PreviousPosition;
                g1.Position += new Vector2(0, (g1CollisionPosition - g1.Position).Y);
                if (g1.Bounds.Intersects(g2.Bounds)) g1.Position = g1.PreviousPosition;
                
            }
            else if (g2Shift != Vector2.Zero)
            {   
                float length = 0.5f;
                Vector2 backShift = g2Shift * (-1);
                while (g1.Bounds.Intersects(g2.Bounds))
                {
                    backShift = Utility.scaleVectorTo(backShift, length);
                    g2.Position = g2CollisionPosition + backShift;
                    length += 0.5f;
                }
                g2.Position += new Vector2((g2CollisionPosition - g2.Position).X, 0);
                if (g1.Bounds.Intersects(g2.Bounds)) g2.Position = g2.PreviousPosition;
                g2.Position += new Vector2(0, (g2CollisionPosition - g2.Position).Y);
                if (g1.Bounds.Intersects(g2.Bounds)) g2.Position = g2.PreviousPosition;
                
            }
        }
    }
}
