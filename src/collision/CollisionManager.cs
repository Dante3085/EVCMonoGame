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

        private static HashSet<GeometryCollidable> geometryCollidables;
        private HashSet<CombatCollidable> combatCollidables;

        #endregion

        #region Constructors
        public CollisionManager()
        {
            geometryCollidables = new HashSet<GeometryCollidable>();
            combatCollidables = new HashSet<CombatCollidable>();
        }
        #endregion

        #region Drawable
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw Collision information of all GeometryCollidables.
            foreach (GeometryCollidable g in geometryCollidables)
            {
                Color darkBlue = Color.DarkBlue;
                darkBlue.A = 50;
                Primitives2D.DrawRectangle(spriteBatch, g.Bounds, darkBlue, 3);
            }

            // Draw Collision information of all CombatCollidables.
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
            victim.OnCombatCollision();
            victim.ReceiveDamage(attacker.CurrentDamage);
            if (!victim.IsAlive)
            {
               
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
            Vector2 g1CollisionPosition = new Vector2(g1.Position.X, g1.Position.Y);
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

        public static void AddGeometryCollidables(params GeometryCollidable[] geometryCollidables)
        {
            foreach (GeometryCollidable g in geometryCollidables)
            {
                if (!CollisionManager.geometryCollidables.Add(g))
                {
                    throw new ArgumentException("The given GeometryCollidable is already known to this CollisionManger.");
                }
            }
        }

        public static void RemoveGeometryCollidables(params GeometryCollidable[] geometryCollidables)
        {
            foreach (GeometryCollidable g in geometryCollidables)
            {
                if (!CollisionManager.geometryCollidables.Remove(g))
                {
                    throw new ArgumentException("The given GeometryCollidable is not known to this CollisionManager.");
                }
            }
        }

        public void AddCombatCollidables(params CombatCollidable[] combatCollidables)
        {
            foreach (CombatCollidable c in combatCollidables)
            {
                if (!this.combatCollidables.Add(c))
                {
                    throw new ArgumentException("The given CombatCollidable is already known to this CollisionManger.");
                }
            }
        }
        
        public void RemoveCombatCollidables(params CombatCollidable[] combatCollidables)
        {
            foreach (CombatCollidable c in combatCollidables)
            {
                if (!this.combatCollidables.Remove(c))
                {
                    throw new ArgumentException("The given CombatCollidable is not known to this CollisionManager.");
                }
            }
        }

        public static bool IsCollisionOnPosition(GeometryCollidable g1, bool fixMyCollision, bool resolveCollisionWithSliding)
        {
            foreach (GeometryCollidable g2 in geometryCollidables)
            {
                if (g1 != g2)
                {
                    if (g1.Bounds.Intersects(g2.Bounds))
                    {
                        if (fixMyCollision && g1 is GeometryCollidable && g2 is GeometryCollidable)
                        {

                            // Zurück gelegte Distanz aka auch Richtung
                            Vector2 direction = g1.Position - g1.PreviousPosition;

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

                                // g1.Bounds = new Rectangle((g1.PreviousPosition + tempDirection).ToPoint(), g1.Bounds.Size);
                                g1.Position = g1.PreviousPosition + tempDirection;
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
                                    // g1.Bounds = new Rectangle((bumpedPosition + new Vector2(tempDirection.X, 0)).ToPoint(), g1.Bounds.Size);
                                    g1.Position = bumpedPosition + new Vector2(tempDirection.X, 0);

                                    if (!IsCollisionOnPosition(g1, false, false))
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
                                    // g1.Bounds = new Rectangle((bumpedPosition + new Vector2(0, tempDirection.Y)).ToPoint(), g1.Bounds.Size);
                                    g1.Position = bumpedPosition + new Vector2(0, tempDirection.Y);

                                    if (!IsCollisionOnPosition(g1, false, false))
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

        #endregion
    }
}
