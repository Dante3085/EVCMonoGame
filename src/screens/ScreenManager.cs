
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.screens
{
    public enum TransitionType
    {
        blackScreen
    }

    public class ScreenManager
    {
        private List<GameScreen> screens = new List<GameScreen>();
        GameScreen currentScreen;
        private List<GameScreen> screensToDraw = new List<GameScreen>();

        private SpriteBatch spriteBatch;
        private ContentManager content;

        public ScreenManager(SpriteBatch spriteBatch, ContentManager content)
        {
            this.spriteBatch = spriteBatch;
            this.content = content;

            screens.Add(new DebugScreen(this));


            currentScreen = screens.First();
        }

        public void LoadContent()
        {
            foreach (GameScreen g in screens)
            {
                g.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            currentScreen.Draw(gameTime, spriteBatch);
        }

        public void TransitionToScreen(GameScreen from, GameScreen to, TransitionType transitionType)
        {
            switch(transitionType)
            {
                default:
                    break;
            }
            currentScreen = to;
        }

        public void TransitionToScreen(GameScreen to, TransitionType transitionType)
        {
            switch (transitionType)
            {
                default:
                    break;
            }
            currentScreen = to;
        }
    }
}
