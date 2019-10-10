using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.screens;

namespace EVCMonoGame.src
{
    class FpsCounter : Updateable, IDrawable
    {
        private float fps;
        private SpriteFont font;
        private String str;
        private Vector2 position;
        Color color;

        public FpsCounter(Vector2 position, Color color)
        {
            fps = -1;
            str = "";
            this.position = position;
            this.color = color;
        }

        public override void Update(GameTime gameTime)
        {
            fps = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;
            str = fps.ToString();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, str, position, color);
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
        }
    }
}
