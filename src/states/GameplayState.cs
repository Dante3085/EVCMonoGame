
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.input;
using EVCMonoGame.src.characters;
using System.Collections.Generic;

namespace EVCMonoGame.src.states
{
    /// <summary>
    /// This state implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    internal class GameplayState : GameState
    {
        // TODO: Player hier statisch erzeugen.

        private ContentManager content;
        private SpriteFont gameFont;

		private static PlayerOne playerOne;
		private static PlayerTwo playerTwo;
		private static bool isTwoPlayer;

		private Random random = new Random();

        private float pauseAlpha;

        private SceneManager sceneManager;

		#region Properties
		public static PlayerOne PlayerOne
		{
			get { return playerOne; }
		}
		public static PlayerTwo PlayerTwo
		{
			get { return playerTwo; }
		}

		public static bool IsTwoPlayer
		{
			get { return isTwoPlayer; }
		}
		#endregion

		public GameplayState()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

			playerOne = new PlayerOne(new Vector2(550, 600), new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left });
			playerTwo = new PlayerTwo(new Vector2(550, 800), new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left });
			isTwoPlayer = true;
		}

		public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(StateManager.Game.Services, "Content");

            // Aus irgendeinem Grund ist der StateManager im Konstruktor von Gameplaystate noch null.
            // Hier aber nicht mehr.
            sceneManager = new SceneManager(StateManager.Game, StateManager);
            sceneManager.LoadContent();

            gameFont = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading state.
            // Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            StateManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        /// <summary>
        /// Updates the state of the game. This method checks the Gamestate.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherstateHasFocus,
                                                       bool coveredByOtherstate)
        {
            base.Update(gameTime, otherstateHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause state.
            if (coveredByOtherstate)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                sceneManager.Update(gameTime);
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay state is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (InputManager.OnKeyPressed(Keys.Escape)
                || InputManager.OnButtonPressed(Buttons.Start, PlayerIndex.One))
            {
                StateManager.AddState(new PauseMenuState(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            StateManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

            sceneManager.Draw(gameTime);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = StateManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                StateManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}