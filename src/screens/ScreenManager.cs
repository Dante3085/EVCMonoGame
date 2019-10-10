
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

    public class ScreenManager
    {
        private List<GameScreen> screens = new List<GameScreen>();
        GameScreen currentScreen;

        private SpriteBatch spriteBatch;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;

        private Texture2D blankTexture;
        private Easer easer;
        private bool transitioning;
        private bool reverseTransitionFinished;

        public ScreenManager(SpriteBatch spriteBatch, ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.graphicsDevice = graphicsDevice;

            screens.Add(new DebugScreen(this, Vector2.Zero));
            currentScreen = screens.First();

            easer = new Easer(0, 255, 1000, Easing.LinearEaseIn);
            transitioning = false;
            reverseTransitionFinished = false;
        }

        public void LoadContent()
        {
            blankTexture = content.Load<Texture2D>("rsrc/backgrounds/blank");

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
            if (transitioning) { UpdateTransition(gameTime); }
        }

        public void Draw(GameTime gameTime)
        {
            currentScreen.Draw(gameTime, spriteBatch);
            if (transitioning) { DrawTransition(); }
        }

        public void ScreenTransition(GameScreen to)
        {
            if (screens.Contains(to))
            {
                screens.Remove(to);
            }
            screens.Add(to);
            to.LoadContent(content);

            transitioning = true;
            easer.start();
        }

        private void UpdateTransition(GameTime gameTime)
        {
            easer.Update(gameTime);
            if (easer.IsFinished)
            {
                if (reverseTransitionFinished)
                {
                    reverseTransitionFinished = false;
                    transitioning = false;
                    easer.reverse();
                }
                else
                {
                    currentScreen = screens.Last();
                    easer.reverse();
                    easer.start();
                    reverseTransitionFinished = true;
                }
            }

        }

        private void DrawTransition()
        {
            spriteBatch.Begin();

            // Achtung: Easer haben immer float Werte. Bei float denkt der Color Konstruktor allerdings
            // er bekommt einen Wert von 0.0 bis 1.0, also nach int casten.
            spriteBatch.Draw(blankTexture, graphicsDevice.Viewport.Bounds, new Color(0, 0, 0, (int)easer.CurrentValue));

            spriteBatch.End();
        }
    }
}
