using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;
using Microsoft.Xna.Framework.Content;

namespace EVCMonoGame.src.gui
{
    public class ExperienceBar
    {
        private Rectangle bounds;

        private Rectangle outlineBounds;
        float outlineThickness;

        // Actual Healthbar bounds(The red bar that shrinks and grows).
        private Rectangle barBounds;
        private Color barColor = Color.Blue;

        private int maxExp;
        private int currentExp;

        // For mapping currentExp value on to the barBounds width.
        // slope * currentExp is the width of barBounds.
        private float slope;

        private SpriteFont expTextFont;
        private String expTextStr;
        private Rectangle expTextBounds;

        private int level = 0;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public Vector2 Size
        {
            get { return bounds.Size.ToVector2(); }
        }

        public Vector2 Position
        {
            get { return bounds.Location.ToVector2(); }
            set
            {
                // Berechne Richtungsvektor, um den alle ExperienceBar Elemente verschoben werden müssen.
                Point shiftVector = value.ToPoint() - bounds.Location;

                bounds.Location += shiftVector;
                outlineBounds.Location += shiftVector;
                barBounds.Location += shiftVector;
                expTextBounds.Location += shiftVector;
            }
        }

        public int MaxExp
        {
            get { return maxExp; }
            set
            {
                maxExp = value < 0 ? 0 : value;

                if (maxExp < currentExp)
                    currentExp = maxExp;

                slope = (float)(outlineBounds.Width - outlineThickness) / maxExp;
                barBounds.Width = (int)(slope * currentExp);
                expTextStr = currentExp.ToString() + "/" + maxExp.ToString();
            }
        }

        public int CurrentExp
        {
            get { return currentExp; }
            set
            {
                currentExp = value < 0 ? 0 : value > maxExp ? maxExp : value;
                barBounds.Width = (int)(slope * currentExp);

                if (currentExp == maxExp)
                {
                    ++level;
                    currentExp = 0;
                }

                expTextStr = "LV: " + level.ToString();
            }
        }

        public ExperienceBar(int maxExp, int currentExp, Vector2 position, Vector2 size)
        {
            outlineThickness = 2;

            bounds.Location = position.ToPoint();
            bounds.Width = (int)size.X;

            expTextBounds.Location = position.ToPoint();
            expTextStr = "Leben";

            outlineBounds.X = (int)position.X;
            outlineBounds.Size = size.ToPoint();

            barBounds.X = (int)(position.X + outlineThickness);
            barBounds.Size = (size - new Vector2(outlineThickness, outlineThickness)).ToPoint();

            // TODO: 
            MaxExp = maxExp;
            CurrentExp = currentExp;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(expTextFont, expTextStr, expTextBounds.Location.ToVector2(), Color.White);
            Primitives2D.FillRectangle(spriteBatch, barBounds, barColor);
            Primitives2D.DrawRectangle(spriteBatch, outlineBounds, Color.White, outlineThickness);
        }

        public void LoadContent(ContentManager content)
        {
            expTextFont = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            Vector2 expTextSize = expTextFont.MeasureString(expTextStr);

            expTextBounds.Size = expTextSize.ToPoint();
            bounds.Height = (int)(expTextSize.Y + outlineBounds.Height);
            outlineBounds.Y = (int)(bounds.Y + expTextSize.Y);
            barBounds.Y = (int)(outlineBounds.Y + outlineThickness);

        }
    }
}
