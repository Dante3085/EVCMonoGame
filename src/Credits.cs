using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src
{
    public class Credits : scenes.IUpdateable, scenes.IDrawable
    {
        private Vector2 position;
        private Viewport viewport;

        private List<String> creditElements = new List<String>();
        private float verticalSpacing;
        private SpriteFont bigFont;
        private Vector2 fontSize;

        private float durationInSeconds = 0;
        private float elapsedSeconds = 0;
        private SceneManager sceneManager;

        public bool DoUpdate
        {
            get; set;
        }

        public Credits(Vector2 position, Viewport viewport, float verticalSpacing, SceneManager sceneManager, float durationInSeconds,
                       params String[] creditElements)
        {
            this.position = position;
            this.viewport = viewport;
            this.verticalSpacing = verticalSpacing;
            this.creditElements.AddRange(creditElements);
            this.sceneManager = sceneManager;
            this.durationInSeconds = durationInSeconds;
        }

        public void Update(GameTime gameTime)
        {
            elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += 0.5f;

            if (elapsedSeconds >= durationInSeconds)
            {
                sceneManager.GoToMainMenu();
            }
        }

        public void LoadContent(ContentManager content)
        {
            bigFont = content.Load<SpriteFont>("rsrc/fonts/BigFont");
            fontSize = bigFont.MeasureString("Teststring");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int i = 0; i < creditElements.Count; ++i)
            {
                spriteBatch.DrawString
                (
                    bigFont, 
                    creditElements[i], 
                    new Vector2(position.X, position.Y + i * fontSize.Y + i * verticalSpacing),
                    Color.Black
                );
            }
        }

        public void IsFinished()
        {
            // TODO:
        }
    }
}
