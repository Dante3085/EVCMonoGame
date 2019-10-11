
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EVCMonoGame.src.states
{
    /// <summary>
    /// Helper class represents a single entry in a Menustate. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    internal class MenuEntry
    {
        #region Fields

        /// <summary>
        /// The text rendered for this entry.
        /// </summary>
        private string text;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        private float selectionFade;

        /// <summary>
        /// The position at which the entry is drawn. This is set by the Menustate
        /// each frame in Update.
        /// </summary>
        private Vector2 position;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        #endregion Events

        #region Initialization

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public MenuEntry(string text)
        {
            this.text = text;
        }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(MenuState menuState, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuState menuState, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= menuState.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            StateManager stateManager = menuState.StateManager;
            SpriteBatch spriteBatch = stateManager.SpriteBatch;
            SpriteFont font = stateManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuState menuState)
        {
            return menuState.StateManager.Font.LineSpacing;
        }

        /// <summary>
        /// Queries how wide the entry is, used for centering on the state.
        /// </summary>
        public virtual int GetWidth(MenuState menuState)
        {
            return (int)menuState.StateManager.Font.MeasureString(Text).X;
        }

        #endregion Update and Draw
    }
}