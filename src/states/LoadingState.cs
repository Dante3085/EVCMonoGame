
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EVCMonoGame.src.states
{
    /// <summary>
    /// The loading state coordinates transitions between the menu system and the
    /// game itself. Normally one state will transition off at the same time as
    /// the next state is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    ///
    /// - Tell all the existing states to transition off.
    /// - Activate a loading state, which will transition on at the same time.
    /// - The loading state watches the state of the previous states.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next state, which may take a long time to load its data. The loading
    ///   state will be the only thing displayed while this load is taking place.
    /// </summary>
    internal class LoadingState : GameState
    {
        #region Fields

        private bool loadingIsSlow;
        private bool otherstatesAreGone;

        private GameState[] statesToLoad;

        #endregion Fields

        #region Initialization

        /// <summary>
        /// The constructor is private: loading states should
        /// be activated via the static Load method instead.
        /// </summary>
        private LoadingState(StateManager stateManager, bool loadingIsSlow,
                              GameState[] statesToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.statesToLoad = statesToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Activates the loading state.
        /// </summary>
        public static void Load(StateManager stateManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer,
                                params GameState[] statesToLoad)
        {
            // Tell all the current states to transition off.
            foreach (GameState state in stateManager.GetStates())
                state.ExitState();

            // Create and activate the loading state.
            LoadingState loadingstate = new LoadingState(stateManager,
                                                            loadingIsSlow,
                                                            statesToLoad);

            stateManager.AddState(loadingstate, controllingPlayer);
        }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Updates the loading state.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherstateHasFocus,
                                                       bool coveredByOtherstate)
        {
            base.Update(gameTime, otherstateHasFocus, coveredByOtherstate);

            // If all the previous states have finished transitioning
            // off, it is time to actually perform the load.
            if (otherstatesAreGone)
            {
                StateManager.RemoveState(this);

                foreach (GameState state in statesToLoad)
                {
                    if (state != null)
                    {
                        StateManager.AddState(state, ControllingPlayer);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                StateManager.Game.ResetElapsedTime();
            }
        }

        /// <summary>
        /// Draws the loading state.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // If we are the only active state, that means all the previous states
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // states to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((StateBehaviour == StateBehaviour.Active) &&
                (StateManager.GetStates().Length == 1))
            {
                otherstatesAreGone = true;
            }

            // The gameplay state takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = StateManager.SpriteBatch;
                SpriteFont font = StateManager.Font;

                const string message = "Loading...";

                // Center the text in the viewport.
                Viewport viewport = StateManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                // Draw the text.
                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }

        #endregion Update and Draw
    }
}