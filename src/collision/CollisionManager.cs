using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.collision
{
    // TODO: Fix ResolveGeometryCollision()
    // TODO: Figure out how to properly convey CombatArgs to victim and handle them in a unified way
    //       so that the same attack always has the same effect.
    // TODO: How to properly remove CombatCollidables when they are dead ?
    // TODO: 

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

        private HashSet<Collidable> collidables;
        private HashSet<GeometryCollidable> geometryCollidables;
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
            // Probleme: Woher soll CollisionManager wissen welches CombatEvent mit welchen Argumenten 
            // der attacker an das victim senden möchte.

            //victim.OnCombatCollision(attacker);
            //if (!victim.IsAlive)
            //{
            //    // TODO: Liste für Collidables, die entfernt werden müssen.
            //    // Vielleicht dann doch lieber Collidable Hierarchie ? Typ check mit enum ?
            //    // Hier kann nicht entfernt werden, da doppelte for-Schleife dann Probleme bekommt.
            //}

            CombatArgs attackerCombatArgs = attacker.CurrentCombatArgs;
            attackerCombatArgs.attacker = attacker;
            attackerCombatArgs.victim = victim;

            victim.OnCombatCollision(attackerCombatArgs);
            if (!victim.IsAlive)
            {
                Console.WriteLine("Victim died in CombatCollision.");
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
            //Vector2 g1CollisionPosition = new Vector2(g1.Position.X, g1.Position.Y);
            //Vector2 g2CollisionPosition = new Vector2(g2.Position.X, g2.Position.Y);
            Vector2 g1CollisionPosition = g1.Position;
            Vector2 g2CollisionPosition = g2.Position;
            if (g1Shift == Vector2.Zero && g2Shift == Vector2.Zero) return;
            if (g2Shift == Vector2.Zero)
            {
                float length = 0.5f;
                Vector2 backShift = g1Shift * (-1);
                Vector2 startPosition = g1.PreviousPosition;
                while (g1.Bounds.Intersects(g2.Bounds))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g1.Position = g1CollisionPosition + backShift;
                    length += 0.5f;
                }
                g1.Position += new Vector2((g1CollisionPosition - g1.Position).X, 0);
                if (g1.Bounds.Intersects(g2.Bounds)) g1.Position = g1.PreviousPosition;
                g1.Position += new Vector2(0, (g1CollisionPosition - g1.Position).Y);
                if (g1.Bounds.Intersects(g2.Bounds)) g1.Position = g1.PreviousPosition;
                g1.Position = startPosition;
                g1.Position = g1.PreviousPosition;

            }
            else if (g1Shift == Vector2.Zero)
            {
                float length = 0.5f;
                Vector2 backShift = g2Shift * (-1);
                Vector2 startPosition = g2.PreviousPosition;
                while (g1.Bounds.Intersects(g2.Bounds))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g2.Position = g2CollisionPosition + backShift;
                    length += 0.5f;
                }
                g2.Position += new Vector2((g2CollisionPosition - g2.Position).X, 0);
                if (g1.Bounds.Intersects(g2.Bounds)) g2.Position = g2.PreviousPosition;
                g2.Position += new Vector2(0, (g2CollisionPosition - g2.Position).Y);
                if (g1.Bounds.Intersects(g2.Bounds)) g2.Position = g2.PreviousPosition;
                g2.Position = startPosition;
                g2.Position = g2.PreviousPosition;
            }
            else if (g1Shift != Vector2.Zero && g2Shift != Vector2.Zero)
            {
                Vector2 g1StartPosition = g1.PreviousPosition;
                Vector2 g2StartPosition = g2.PreviousPosition;
                Vector2 g1CollisionSolution;
                Vector2 g2CollisionSolution;
                float g1CollisionSolutionLength = 0;
                float g2CollisionSolutionLength = 0;
                
                //g1 Collision Solution
                float length = 0.5f;
                Vector2 backShift = g1Shift * (-1);
                Vector2 startPosition = g1.PreviousPosition;
                while (g1.Bounds.Intersects(g2.Bounds))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g1.Position = g1CollisionPosition + backShift;
                    length += 0.5f;
                }
                g1.Position += new Vector2((g1CollisionPosition - g1.Position).X, 0);
                if (g1.Bounds.Intersects(g2.Bounds)) g1.Position = g1.PreviousPosition;
                g1.Position += new Vector2(0, (g1CollisionPosition - g1.Position).Y);
                if (g1.Bounds.Intersects(g2.Bounds)) g1.Position = g1.PreviousPosition;
                g1CollisionSolution = g1.Position;
                g1.Position = g1StartPosition;
                g1.Position = g1CollisionPosition;
                g1CollisionSolutionLength = (g1CollisionSolution - g1CollisionPosition).Length();
               
                //g2 Collision Solution
                length = 0.5f;
                backShift = g2Shift * (-1);
                while (g1.Bounds.Intersects(g2.Bounds))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g2.Position = g2CollisionPosition + backShift;
                    length += 0.5f;
                }
                g2.Position += new Vector2((g2CollisionPosition - g2.Position).X, 0);
                if (g1.Bounds.Intersects(g2.Bounds)) g2.Position = g2.PreviousPosition;
                g2.Position += new Vector2(0, (g2CollisionPosition - g2.Position).Y);
                if (g1.Bounds.Intersects(g2.Bounds)) g2.Position = g2.PreviousPosition;
                g2CollisionSolution = g2.Position;
                g2.Position = g2StartPosition;
                g2.Position = g2CollisionPosition;
                g2CollisionSolutionLength = (g2CollisionSolution - g2CollisionPosition).Length();

                //chosing of the correct solution
                if (g1CollisionSolutionLength < g2CollisionSolutionLength)
                {
                    g1.Position = g1StartPosition;
                    g1.Position = g1CollisionSolution;
                }
                else
                {
                    g2.Position = g2StartPosition;
                    g2.Position = g2CollisionSolution;
                }
            }
        }
        #endregion
        #endregion
        #region CollisionManager

        public void AddGeometryCollidables(params GeometryCollidable[] geometryCollidables)
        {
            foreach (GeometryCollidable g in geometryCollidables)
            {
                if (!this.geometryCollidables.Add(g))
                {
                    throw new ArgumentException("The given GeometryCollidable is already known to this CollisionManger.");
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

        #endregion
    }
}
