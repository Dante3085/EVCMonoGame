
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
    public enum EGameScreen
    {
        MAIN_MENU,
        OPTIONS,
        DEBUG,
        DEBUG_2,
    }

    public class ScreenManager
    {
        private Dictionary<EGameScreen, GameScreen> gameScreens;
        private GameScreen currentScreen;

        private GameScreen nextScreen;
        private GameScreen previousScreen;
        private Texture2D screenTransitionTexture;
        private Easer easer;
        private bool transitioning;
        private bool reverseTransitionFinished;

        private Game game;
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;

        private FpsCounter fpsCounter;

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public ScreenManager(Game game, SpriteBatch spriteBatch, ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.graphicsDevice = graphicsDevice;

            gameScreens = new Dictionary<EGameScreen, GameScreen>();
            gameScreens[EGameScreen.MAIN_MENU] = new MainMenuScreen(this);
            gameScreens[EGameScreen.DEBUG] = new DebugScreen(this);
            gameScreens[EGameScreen.DEBUG_2] = new DebugScreen2(this);
            currentScreen = previousScreen = gameScreens[EGameScreen.MAIN_MENU];

            nextScreen = null;
            easer = new Easer(0, 255, 1000, Easing.SineEaseIn);
            transitioning = false;
            reverseTransitionFinished = false;

            fpsCounter = new FpsCounter(Vector2.Zero, Color.White);
        }

        public void LoadContent()
        {
            screenTransitionTexture = content.Load<Texture2D>("rsrc/backgrounds/blank");
            fpsCounter.LoadContent(content);

            foreach (GameScreen g in gameScreens.Values)
            {
                g.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime)
        {
            // GameScreen Updating
            currentScreen.Update(gameTime);

            // Global Updating
            if (transitioning) { UpdateTransition(gameTime); }
            fpsCounter.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            // GameScreen Drawing
            currentScreen.Draw(gameTime, spriteBatch);

            // Global Drawing
            spriteBatch.Begin();

            if (transitioning) { DrawTransition(); }
            fpsCounter.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public void ScreenTransition(EGameScreen to)
        {
            if (!gameScreens.ContainsKey(to))
            {
                throw new ArgumentException(to + " is not known to the ScreenManager.");
            }

            nextScreen = gameScreens[to];
            transitioning = true;
            easer.start();
        }

        public void TransitionToPreviousScreen()
        {
            nextScreen = previousScreen;
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
                    previousScreen = currentScreen;
                    currentScreen = nextScreen;
                    easer.reverse();
                    easer.start();
                    reverseTransitionFinished = true;
                }
            }

        }

        private void DrawTransition()
        {
            // Achtung: Easer haben immer float Werte. Bei float denkt der Color Konstruktor allerdings
            // er bekommt einen Wert von 0.0 bis 1.0, also nach int casten.
            spriteBatch.Draw(screenTransitionTexture, graphicsDevice.Viewport.Bounds, new Color(0, 0, 0, (int)easer.CurrentValue));
        }

        public Vector2 GetViewportCenter()
        {
            Vector2 viewportCenter = Vector2.Zero;
            viewportCenter.X += graphicsDevice.Viewport.Width * 0.5f;
            viewportCenter.Y += graphicsDevice.Viewport.Height * 0.5f;
            return viewportCenter;
        }

        public void Exit()
        {
            game.Exit();
        }
    }
}
