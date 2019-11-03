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
    public class FpsCounter
    {
        private SpriteFont font;
        private String str;
        private Vector2 position;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FpsCounter(Vector2 position)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            // After 1 second passed put the amount of frames that happened
            // into frameRate. That's basically our fps.
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ++frameCounter;
            str = String.Format("fps: {0}", frameRate);

            spriteBatch.DrawString(font, str, new Vector2(33, 33), Color.DarkRed);
            spriteBatch.DrawString(font, str, new Vector2(32, 32), Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
        }
    }
}
