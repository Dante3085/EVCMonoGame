using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EVCMonoGame.src
{
    class FpsCounter : IUpdateable, IDrawable
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

        public void Update(GameTime gameTime)
        {
            fps = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;
            str = fps.ToString();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, str, position, color);
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
        }

        public void UnloadContent()
        {
            // TODO: Was könnte hier hinkommen ?
        }
    }
}
