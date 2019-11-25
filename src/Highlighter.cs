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
using EVCMonoGame.src.states;

namespace EVCMonoGame.src
{
    public class Highlighter : scenes.IUpdateable, scenes.IDrawable
    {
        private Rectangle area;
        private Color color = Color.Blue;
        private Easer colorEaser = new Easer(Vector2.Zero, new Vector2(100, 0), 1000, Easing.SineEaseInOut);

        public bool DoUpdate
        {
            get; set;
        } = true;

        public Highlighter(Rectangle area)
        {
            this.area = area;

            colorEaser.Start();
        }

        public void Update(GameTime gameTime)
        {
            colorEaser.Update(gameTime);

            color.A = (byte)colorEaser.CurrentValue.X;

            if (colorEaser.IsFinished)
            {
                colorEaser.Reverse();
                colorEaser.Start();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Primitives2D.FillRectangle(spriteBatch, area, color);

            Primitives2D.DrawCircle(spriteBatch, area.Location.ToVector2(), 100, 20, color, 10);
        }

        public void LoadContent(ContentManager content)
        {
            
        }
    }
}
