using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace EVCMonoGame
{
    public class DebugTexts : src.scenes.IDrawable
    {
        private Vector2 position;
        private SpriteFont font;
        private List<String> entries;
        Vector2 textSize;

        public List<String> Entries
        {
            get { return entries; }
        }

        public DebugTexts(Vector2 position)
        {
            this.position = position;
            entries = new List<string>();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entries.Count; ++i)
            {
                Vector2 entryPosition = position;
                entryPosition.Y += i * textSize.Y;

                spriteBatch.DrawString(font, entries[i], entryPosition, Color.White);
            }
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            textSize = font.MeasureString("Sample");
        }
    }
}
