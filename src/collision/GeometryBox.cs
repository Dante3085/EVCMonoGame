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
    public class GeometryBox : GeometryCollidable
    {
        private Rectangle bounds;
        private Vector2 previousPosition;

        public GeometryBox(Rectangle bounds)
        {
            this.bounds = bounds;
            previousPosition = bounds.Location.ToVector2();
        }

        public Rectangle Bounds
        {
            get { return bounds; }
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Primitives2D.DrawRectangle(spriteBatch, bounds, Color.AliceBlue);
        }

        public void LoadContent(ContentManager content)
        {
            // Nothing to load
        }
    }
}
