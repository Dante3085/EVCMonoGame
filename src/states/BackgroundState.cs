
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EVCMonoGame.src.states
{
    /// <summary>
    /// The background state sits behind all the other menu states.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the states on top of it may be doing.
    /// </summary>
    internal class BackgroundState : GameState
    {
        #region Fields

        private ContentManager content;
        private Texture2D backgroundTexture;

        #endregion Fields

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundState()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Loads graphics content for this state. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(StateManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("rsrc/backgrounds/forestBackground");
        }

        /// <summary>
        /// Unloads graphics content for this state.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Updates the background state. Unlike most states, this should not
        /// transition off even if it has been covered by another state: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherstate parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherstateHasFocus,
                                                       bool coveredByOtherstate)
        {
            base.Update(gameTime, otherstateHasFocus, false);
        }

        /// <summary>
        /// Draws the background state.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = StateManager.SpriteBatch;
            Viewport viewport = StateManager.GraphicsDevice.Viewport;
            Rectangle fullstate = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullstate,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }

        #endregion Update and Draw
    }
}