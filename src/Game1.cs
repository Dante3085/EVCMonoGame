
// usings mit fremden Code
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using C3.MonoGame;
using System;

// usings mit eigenem Code
using EVCMonoGame.src.states;
using EVCMonoGame.src.input;

// TODO: Castlevania CurseOfDarkness Menü musik

namespace EVCMonoGame.src
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private StateManager stateManager;

        private SpriteBatch spriteBatch;

        private FpsCounter fpsCounter;
        private bool drawFpsCounter;

        public static bool MouseVisible
        {
            get;
            set;
        }

        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        private static readonly string[] preloadAssets =
        {
            "rsrc/backgrounds/gradient",
        };

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            MouseVisible = true;

            graphics = new GraphicsDeviceManager(this);

            //graphics.PreferredBackBufferWidth = 3240;
            //graphics.PreferredBackBufferHeight = 2160;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = true;

			// Debug Helpers
			bool bypassMainMenu = false;

            // Create the screen manager component.
            stateManager = new StateManager(this);

            Components.Add(stateManager);

			// Activate the first screens.
			if (bypassMainMenu)
				LoadingState.Load(stateManager, true, PlayerIndex.One, new GameplayState());
			else
			{
				stateManager.AddState(new BackgroundState(), null);
				stateManager.AddState(new MainMenuState(), null);
			}


			fpsCounter = new FpsCounter(Vector2.Zero);
            drawFpsCounter = true;
        }

        protected override void LoadContent()
        {
            fpsCounter.LoadContent(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            IsMouseVisible = MouseVisible;
            if (InputManager.OnKeyPressed(Keys.F2))
            {
                drawFpsCounter = drawFpsCounter ? false : true;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            fpsCounter.Update(gameTime);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);

            spriteBatch.Begin();

            if (drawFpsCounter)
            {
                fpsCounter.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }

            Console.WriteLine("d");
        }
    }
}
