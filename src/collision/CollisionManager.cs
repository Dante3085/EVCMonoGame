using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;

namespace EVCMonoGame.src.collision
{
    // TODO(Problem): Wie unterscheide ich zwischen Kollisionsarten? Ist einfach nur die Geometrie
    // miteinander kollidiert oder handelte es sich um einen Angriff oder noch etwas anderes?

    class CollisionManager : IUpdateable, IDrawable
    {
        private List<ICollidable> collidables;
        // private List<CombatCollidable> combatCollidables;

        public bool DrawBounds { get; set; }

        public CollisionManager(params ICollidable[] collidables)
        {
            this.collidables = new List<ICollidable>(collidables);
            DrawBounds = false;
        }

        public void Update(GameTime gameTime)
        {
            foreach (ICollidable c1 in collidables)
            {
                foreach(ICollidable c2 in collidables)
                {
                    if (c1 == c2) { continue; }
                    else if (c1.Bounds.Intersects(c2.Bounds))
                    {
                        if (c1 is ICombatCollidable && c2 is ICombatCollidable)
                        {
                            ((ICombatCollidable)c1).HandleCombatCollision((ICombatCollidable)c2);
                        }
                        else
                        {
                            Vector2 shiftVector = c1.ShiftVector;

                        }
                    }
                }
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (DrawBounds)
            {
                foreach (ICollidable c in collidables)
                {
                    Primitives2D.DrawRectangle(spriteBatch, c.Bounds, Color.Blue);
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// Premise: r2 moved into r1 using shiftVector.
        ///// This method returns minimum Vector2 that removes r2 from r1
        ///// in the direction it came from.
        ///// </summary>
        ///// <param name="r1"></param>
        ///// <param name="r2"></param>
        ///// <param name="shiftVector"></param>
        ///// <returns></returns>
        //public static Vector2 calcBackShiftVector(Rectangle r1, Rectangle r2, Vector2 shiftVector)
        //{
        //    Vector2 backShiftVector = new Vector2();

        //    if (shiftVector.X < 0)
        //    {
        //        backShiftVector.X = r1.Right - r2.Left;
        //    }
        //    else if (shiftVector.X > 0)
        //    {
        //        backShiftVector.X = r1.Left - r2.Right;
        //    }

        //    if (shiftVector.Y < 0)
        //    {
        //        backShiftVector.Y = r1.Bottom - r2.Top;
        //    }
        //    else if (shiftVector.Y > 0)
        //    {
        //        backShiftVector.Y = r1.Top - r2.Bottom;
        //    }

        //    return backShiftVector;
        //}
    }
}
