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
        private String strHpText;
        private Vector2 hpTextPosition;

        #endregion
        #region Properties

        public int MaxHp
        {
            get { return maxHp; }
            set
            {
                maxHp = value < 0 ? 0 : value;

                if (maxHp < currentHp)
                    currentHp = maxHp;

                slope = (float)outlineBounds.Width / maxHp;
                barBounds.Width = (int)(slope * currentHp);
                // strHpText = currentHp.ToString() + "/" + maxHp.ToString();
            }
        }

        public int CurrentHp
        {
            get { return currentHp; }
            set 
            {
                currentHp = value < 0 ? 0 : value > maxHp ? maxHp : value;
                barBounds.Width = (int)(slope * currentHp);
                // strHpText = strHpText = currentHp.ToString() + "/" + maxHp.ToString();

                if (currentHp <= maxHp / 4)
                {
                    barColor = Color.DarkRed;
                }

                else if (currentHp <= maxHp * 0.75)
                {
                    barColor = Color.Yellow;
                }

                else
                {
                    barColor = Color.Green;
                }
            }
        }

        public bool DrawOutline { get; set; }
        #endregion

        public Healthbar(int maxHp, int currentHp, Rectangle bounds)
        {
            outlineBounds = bounds;
            outlineThickness = 1;

            barBounds.X = outlineBounds.X; // X-Coordinate does not change with varying outlineThickness.
            barBounds.Y = (int)(outlineBounds.Y + outlineThickness);
            barBounds.Width = (int)(outlineBounds.Width - outlineThickness);
            barBounds.Height = (int)(outlineBounds.Height - outlineThickness);

            barColor = Color.Green;

            MaxHp = maxHp;
            CurrentHp = currentHp;

            // strHpText = currentHp.ToString() + "/" + maxHp.ToString();s
            strHpText = "Leben";
            hpTextPosition = new Vector2(outlineBounds.X, outlineBounds.Y);

            DrawOutline = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Primitives2D.FillRectangle(spriteBatch, barBounds, barColor);

            if (DrawOutline)
            {
                Primitives2D.DrawRectangle(spriteBatch, outlineBounds, Color.White, outlineThickness);
            }
            spriteBatch.DrawString(hpTextFont, strHpText, hpTextPosition, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            hpTextFont = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            hpTextPosition.Y -= hpTextFont.MeasureString(strHpText).Y;
        }

        public void UnloadContent()
        {
            // TODO
            
        }
    }
}
