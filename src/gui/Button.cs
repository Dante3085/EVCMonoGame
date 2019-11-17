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
using EVCMonoGame.src.input;

namespace EVCMonoGame.src.gui
{
    // TODO: Fix MouseHover.
    // TODO: Fix TextPlacement.

    public class Button : scenes.IUpdateable, scenes.IDrawable
    {
        #region Fields

        private Rectangle bounds;
        private SpriteFont font;
        private Vector2 textSize;
        private String text;
        private Action action;
        private Color fillColor;

        #endregion

        #region Properties

        public bool DoUpdate
        {
            get; set;
        } = true;

        #endregion

        public Button(Rectangle bounds,String text, Action action)
        {
            this.bounds = bounds;
            this.text = text;
            this.action = action;
            fillColor = Color.AliceBlue;
            fillColor.A = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (bounds.Contains(InputManager.CurrentMousePosition()))
            {
                fillColor.A = 100;
                if (InputManager.OnLeftMouseButtonClicked())
                {
                    action();
                }
            }
            else
            {
                fillColor.A = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Primitives2D.FillRectangle(spriteBatch, bounds, fillColor);
            spriteBatch.DrawString(font, text, bounds.Location.ToVector2() - 
                                               new Vector2(textSize.X * 0.5f, textSize.Y * 0.5f), Color.White);

            spriteBatch.End();
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            textSize = font.MeasureString(text);
        }
    }
}
