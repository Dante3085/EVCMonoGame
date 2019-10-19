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
        #region Fields
        private List<Collidable> collidables;
        private List<GeometryCollidable> geometryCollidables;
        private List<CombatCollidable> combatCollidables;
        #endregion

        #region Constructors
        public CollisionManager()
        {
            collidables = new List<Collidable>();
            geometryCollidables = new List<GeometryCollidable>();
            combatCollidables = new List<CombatCollidable>();
        }
        #endregion

        #region Drawable
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Collidable c in collidables)
            {
                Color darkBlue = Color.DarkBlue;
                darkBlue.A = 50;
                Primitives2D.DrawRectangle(spriteBatch, c.Bounds, darkBlue, 2);
            }

            foreach (CombatCollidable c in combatCollidables)
            {
                Color darkGreen = Color.DarkGreen;
                darkGreen.A = 50;
                Primitives2D.FillRectangle(spriteBatch, c.HurtBounds, darkGreen);

                Color darkRed = Color.DarkRed;
                darkRed.A = 50;
                Primitives2D.FillRectangle(spriteBatch, c.AttackBounds, darkRed);
            }
        }

        public void LoadContent(ContentManager content)
        {

        }
        #endregion
        #region Updateable
        public override void Update(GameTime gameTime)
        {
            CheckGeometryCollisions();
            CheckCombatCollisions();
        }

        #region UpdateableHelper
        private void CheckCombatCollisions()
        {
            foreach (CombatCollidable c1 in combatCollidables)
            {
                foreach (CombatCollidable c2 in combatCollidables)
                {
                    if (c1 == c2)
                    {
                        continue;
                    }
                    else
                    {
                        if (c1.HasActiveAttackBounds)
                        {
                            if (c1.AttackBounds.Intersects(c2.HurtBounds))
                            {
                                ResolveCombatCollision(c1, c2);
                            }
                        }
                    }
                }
            }
        }

        private void ResolveCombatCollision(CombatCollidable attacker, CombatCollidable victim)
        {
            victim.ReceiveDamage(attacker.CurrentDamage);
            if (!victim.IsAlive)
            {
                Console.WriteLine("Jemand ist im Kampf gestorben.");
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
        #endregion
        #endregion
        #region CollisionManager
        public void AddCollidables(params Collidable[] collidables)
        {
            foreach (Collidable c in collidables)
            {
                this.collidables.Add(c);

                if (c is GeometryCollidable)
                {
                    geometryCollidables.Add((GeometryCollidable)c);
                }

                if (c is CombatCollidable)
                {
                    combatCollidables.Add((CombatCollidable)c);
                }
            }
        }
        #endregion
    }
}
