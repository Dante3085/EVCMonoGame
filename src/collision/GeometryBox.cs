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
    public class GeometryBox : Collidable
    {
        private Rectangle bounds;
        private Vector2 previousPosition;

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public Vector2 Position
        {
            get { return bounds.Location.ToVector2(); }
            set
            {
                previousPosition = bounds.Location.ToVector2();
                bounds.Location = value.ToPoint();
            }
        }

        public Vector2 PreviousPosition
        {
            get { return previousPosition; }
        }

        public Rectangle CollisionBox
        {
            get { return bounds; }
        }

        public Vector2 WorldPosition
        {
            get { return bounds.Location.ToVector2(); }
            set
            {
                previousPosition = bounds.Location.ToVector2();
                bounds.Location = value.ToPoint();
            }
        }

        public Vector2 PreviousWorldPosition
        {
            get { return previousPosition; }
        }

        public GeometryBox(Rectangle bounds)
        {
            this.bounds = bounds;
            previousPosition = bounds.Location.ToVector2();
        }
    }
}
