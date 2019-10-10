
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.screens
{
    public class MenuEntry
    {
        private String text;
        private Vector2 position;
        private float scale;
        private Color color;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public int Alpha 
        {
            get { return color.A; }
            set { color.A = (byte)value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public event EventHandler Pressed;

        public MenuEntry(String text)
        {
            this.text = text;
            position = new Vector2(1000, 300);
            scale = 1;
            color = Color.DarkRed;
        }

        public void Draw(SpriteBatch spriteBatch, MenuScreen menuScreen, bool isSelected)
        {
            color = isSelected ? Color.Yellow : Color.White;

            spriteBatch.DrawString(menuScreen.MenuFont, text, position, color, 0, Vector2.Zero, scale,
                SpriteEffects.None, 0);
        }

        public void OnPressed()
        {
            if (Pressed != null)
                Pressed(this, EventArgs.Empty);
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.MenuFont.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.MenuFont.MeasureString(text).X;
        }
    }
}