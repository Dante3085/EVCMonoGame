
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System;

using EVCMonoGame.src.input;

namespace EVCMonoGame.src.states
{
    /// <summary>
    /// The state manager is a component which manages one or more Gamestate
    /// instances. It maintains a stack of states, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active state.
    /// </summary>
    public class StateManager : DrawableGameComponent
    {
        #region Fields

        private List<GameState> states = new List<GameState>();
        private List<GameState> statesToUpdate = new List<GameState>();

        private InputState input = new InputState();

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D blankTexture;

        private bool isInitialized;

        private bool traceEnabled;

        #endregion Fields

        #region Properties

        /// <summary>
        /// A default SpriteBatch shared by all the states. This saves
        /// each state having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        /// <summary>
        /// A default font shared by all the states. This saves
        /// each state having to bother loading their own local copy.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// If true, the manager prints out a list of all the states
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Constructs a new state manager component.
        /// </summary>
        public StateManager(Game game)
            : base(game)
        {
            // we must set EnabledGestures before we can query for them, but
            // we don't assume the game wants to read them.
            TouchPanel.EnabledGestures = GestureType.None;
        }

        /// <summary>
        /// Initializes the state manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the state manager.
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            blankTexture = content.Load<Texture2D>("rsrc/backgrounds/blank");

            // Tell each of the states to load their content.
            foreach (GameState state in states)
            {
                state.LoadContent();
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the states to unload their content.
            foreach (GameState state in states)
            {
                state.UnloadContent();
            }
        }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Allows each state to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            InputManager.UpdateInputStates(gameTime);

            if (InputManager.AreAllButtonsPressed(Buttons.Back, Buttons.Start)
                || InputManager.AreAllKeysPressed(Keys.Escape, Keys.Delete))
            {
                Game.Exit();
            }

            // Read the keyboard and gamepad.
            input.Update();

            // Make a copy of the master state list, to avoid confusion if
            // the process of updating one state adds or removes others.
            statesToUpdate.Clear();

            foreach (GameState state in states)
                statesToUpdate.Add(state);

            bool otherStateHasFocus = !Game.IsActive;
            bool coveredByOtherState = false;

            // Loop as long as there are states waiting to be updated.
            while (statesToUpdate.Count > 0)
            {
                // Pop the topmost state off the waiting list.
                GameState state = statesToUpdate[statesToUpdate.Count - 1];

                statesToUpdate.RemoveAt(statesToUpdate.Count - 1);

                // Update the state.
                state.Update(gameTime, otherStateHasFocus, coveredByOtherState);

                if (state.StateBehaviour == StateBehaviour.TransitionOn ||
                    state.StateBehaviour == StateBehaviour.Active)
                {
                    // If this is the first active state we came across,
                    // give it a chance to handle input.
                    if (!otherStateHasFocus)
                    {
                        state.HandleInput(input);

                        otherStateHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // states that they are covered by it.
                    if (!state.IsPopup)
                        coveredByOtherState = true;
                }
            }

            // Print debug trace?
            if (traceEnabled)
                Tracestates();
        }

        /// <summary>
        /// Prints a list of all the states, for debugging.
        /// </summary>
        private void Tracestates()
        {
            List<string> stateNames = new List<string>();

            foreach (GameState state in states)
                stateNames.Add(state.GetType().Name);

            Debug.WriteLine(string.Join(", ", stateNames.ToArray()));
        }

        /// <summary>
        /// Tells each state to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameState state in states)
            {
                if (state.StateBehaviour == StateBehaviour.Hidden)
                    continue;

                state.Draw(gameTime);
            }
        }

        #endregion Update and Draw

        #region Public Methods

        /// <summary>
        /// Adds a new state to the state manager.
        /// </summary>
        public void AddState(GameState state, PlayerIndex? controllingPlayer)
        {
            state.ControllingPlayer = controllingPlayer;
            state.StateManager = this;
            state.IsExiting = false;

            // If we have a graphics device, tell the state to load content.
            if (isInitialized)
            {
                state.LoadContent();
            }

            states.Add(state);

            // update the TouchPanel to respond to gestures this state is interested in
            TouchPanel.EnabledGestures = state.EnabledGestures;
        }

        /// <summary>
        /// Removes a state from the state manager. You should normally
        /// use Gamestate.Exitstate instead of calling this directly, so
        /// the state can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveState(GameState state)
        {
            // If we have a graphics device, tell the state to unload content.
            if (isInitialized)
            {
                state.UnloadContent();
            }

            states.Remove(state);
            statesToUpdate.Remove(state);

            // if there is a state still in the manager, update TouchPanel
            // to respond to gestures that state is interested in.
            if (states.Count > 0)
            {
                TouchPanel.EnabledGestures = states[states.Count - 1].EnabledGestures;
            }
        }

        /// <summary>
        /// Expose an array holding all the states. We return a copy rather
        /// than the real master list, because states should only ever be added
        /// or removed using the Addstate and Removestate methods.
        /// </summary>
        public GameState[] GetStates()
        {
            return states.ToArray();
        }

        /// <summary>
        /// Helper draws a translucent black fullstate sprite, used for fading
        /// states in and out, and for darkening the background behind popups.
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin();

            spriteBatch.Draw(blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             Color.Black * alpha);

            spriteBatch.End();
        }

        #endregion Public Methods
    }
}