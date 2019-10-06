using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;

using EVCMonoGame.src.input;

namespace EVCMonoGame.src.gui
{
    public class Healthbar : IDrawable
    {
        #region Fields

        private Rectangle bounds;

        private Rectangle outlineBounds;
        float outlineThickness;

        // Actual Healthbar bounds(The red bar that shrinks and grows).
        private Rectangle barBounds;
        private Color barColor;

        private int maxHp;
        private int currentHp;

        // For mapping currentHp value on to the barBounds width.
        // slope * currentHp is the width of barBounds.
        private float slope;

        private SpriteFont hpTextFont;
        private String hpTextStr;
        private Rectangle hpTextBounds;

        #endregion
        #region Properties

        public Vector2 Size
        {
            get { return bounds.Size.ToVector2(); }
        }

        public Vector2 Position
        {
            get { return bounds.Location.ToVector2(); }
            set
            {
                // Berechne Richtungsvektor, um den alle Healthbar Elemente verschoben werden müssen.
                Point shiftVector = value.ToPoint() - bounds.Location;

                bounds.Location += shiftVector;
                outlineBounds.Location += shiftVector;
                barBounds.Location += shiftVector;
                hpTextBounds.Location += shiftVector;
            }
        }

        public int MaxHp
        {
            get { return maxHp; }
            set
            {
                maxHp = value < 0 ? 0 : value;

                if (maxHp < currentHp)
                    currentHp = maxHp;

                slope = (float)(outlineBounds.Width - outlineThickness) / maxHp;
                barBounds.Width = (int)(slope * currentHp);
                hpTextStr = currentHp.ToString() + "/" + maxHp.ToString();
            }
        }

        public int CurrentHp
        {
            get { return currentHp; }
            set 
            {
                currentHp = value < 0 ? 0 : value > maxHp ? maxHp : value;
                barBounds.Width = (int)(slope * currentHp);
                hpTextStr = currentHp.ToString() + "/" + maxHp.ToString();

                if (currentHp <= maxHp / 4)
                {
                    barColor = Color.DarkRed;
                }

                else if (currentHp <= maxHp * 0.75)
                {
                    barColor = new Color(238, 238, 0);
                }

                else
                {
                    barColor = new Color(124, 252, 0);
                }
            }
        }

        #endregion

        public Healthbar(int maxHp, int currentHp, Vector2 position, Vector2 size)
        {
            outlineThickness = 1;

            bounds.Location = position.ToPoint();
            bounds.Width = (int)size.X;

            hpTextBounds.Location = position.ToPoint();
            hpTextStr = "Leben";

            outlineBounds.X = (int)position.X;
            outlineBounds.Size = size.ToPoint();

            barBounds.X = (int)(position.X + outlineThickness);
            barBounds.Size = (size - new Vector2(outlineThickness, outlineThickness)).ToPoint();

            // TODO: 
            MaxHp = maxHp;
            CurrentHp = currentHp;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(hpTextFont, hpTextStr, hpTextBounds.Location.ToVector2(), Color.White);
            Primitives2D.FillRectangle(spriteBatch, barBounds, barColor);
            Primitives2D.DrawRectangle(spriteBatch, outlineBounds, Color.White, outlineThickness);
        }

        public void LoadContent(ContentManager content)
        {
            hpTextFont = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            Vector2 hpTextSize = hpTextFont.MeasureString(hpTextStr);

            hpTextBounds.Size = hpTextSize.ToPoint();
            bounds.Height = (int)(hpTextSize.Y + outlineBounds.Height);
            outlineBounds.Y = (int)(bounds.Y + hpTextSize.Y);
            barBounds.Y = (int)(outlineBounds.Y + outlineThickness);


        }

        public void UnloadContent()
        {
            // TODO
            
        }
    }
}
