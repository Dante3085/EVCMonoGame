
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
    class ScreenManager
    {
        private List<GameScreen> gameScreens;
        private List<GameScreen> gameScreensToUpdate;
        private List<GameScreen> gameScreensToDraw;

        private SpriteBatch spriteBatch;

        public ScreenManager()
        {
            gameScreens = new List<GameScreen>();
            gameScreensToUpdate = new List<GameScreen>();
            gameScreensToDraw = new List<GameScreen>();
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);

            foreach(GameScreen g in gameScreens)
            {
                g.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
            foreach(GameScreen g in gameScreens)
            {
                g.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameScreen g in gameScreensToUpdate)
            {
                g.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameScreen g in gameScreensToDraw)
            {
                g.Draw(gameTime, spriteBatch);
            }
        }

        public void AddScreen(GameScreen gameScreen)
        {
            gameScreens.Add(gameScreen);
            gameScreensToUpdate.Add(gameScreen);
            gameScreensToDraw.Add(gameScreen);
        }
    }
}
