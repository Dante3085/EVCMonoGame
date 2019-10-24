using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src
{
    class FpsCounter : Updateable, scenes.IDrawable
    {
        private float fps;
        private SpriteFont font;
        private String str;
        private Vector2 position;
        Color color;

        private int delay;
        private int elapsed;
        private List<float> fpsBuffer;
        private int indexCurrentFpsValue;

        public FpsCounter(Vector2 position, Color color)
        {
            str = "";
            this.position = position;
            this.color = color;

            delay = 500;
            elapsed = 0;
            fpsBuffer = new List<float>();
            indexCurrentFpsValue = 0;
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            fps = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;
            fpsBuffer.Add(fps);

            // Only display next fps value after a certain amount of time.
            // Otherwise humans can't follow it.
            if (elapsed >= delay)
            {
                // Reset elapsed
                elapsed = 0;

                // Put the current fps Value into the display string
                str = fpsBuffer[indexCurrentFpsValue++ % fpsBuffer.Count].ToString();

                // Remove all old fps values from the buffer
                fpsBuffer.RemoveRange(0, indexCurrentFpsValue);

                // Set the index for the next fps value to be displayed.
                indexCurrentFpsValue = 0;
            }
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
