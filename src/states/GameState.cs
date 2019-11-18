
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace EVCMonoGame.src.states
{
    /// <summary>
    /// Enum describes the state transition state.
    /// </summary>
    public enum StateBehaviour
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    /// <summary>
    /// A state is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as states.
    /// </summary>
    public abstract class GameState
    {
        #region Properties

        /// <summary>
        /// Normally when one state is brought up over the top of another,
        /// the first state will transition off to make room for the new
        /// one. This property indicates whether the state is only a small
        /// popup, in which case states underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        private bool isPopup = false;

        /// <summary>
        /// Indicates how long the state takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        private TimeSpan transitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// Indicates how long the state takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        private TimeSpan transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Gets the current position of the state transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        private float transitionPosition = 1;

        /// <summary>
        /// Gets the current alpha of the state transition, ranging
        /// from 1 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        /// <summary>
        /// Gets the current state transition state.
        /// </summary>
        public StateBehaviour StateBehaviour
        {
            get { return stateBehaviour; }
            protected set { stateBehaviour = value; }
        }

        private StateBehaviour stateBehaviour = StateBehaviour.TransitionOn;

        /// <summary>
        /// There are two possible reasons why a state might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// state that is on top of it, or it could be going away for good.
        /// This property indicates whether the state is exiting for real:
        /// if set, the state will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        private bool isExiting = false;

        /// <summary>
        /// Checks whether this state is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !otherStateHasFocus &&
                       (stateBehaviour == StateBehaviour.TransitionOn ||
                        stateBehaviour == StateBehaviour.Active);
            }
        }

        private bool otherStateHasFocus;

        /// <summary>
        /// Gets the manager that this state belongs to.
        /// </summary>
        public StateManager StateManager
        {
            get { return stateManager; }
            internal set { stateManager = value; }
        }

        private StateManager stateManager;

        /// <summary>
        /// Gets the index of the player who is currently controlling this state,
        /// or null if it is accepting input from any player. This is used to lock
        /// the game to a specific player profile. The main menu responds to input
        /// from any connected gamepad, but whichever player makes a selection from
        /// this menu is given control over all subsequent states, so other gamepads
        /// are inactive until the controlling player returns to the main menu.
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        private PlayerIndex? controllingPlayer;

        /// <summary>
        /// Gets the gestures the state is interested in. states should be as specific
        /// as possible with gestures to increase the accuracy of the gesture engine.
        /// For example, most menus only need Tap or perhaps Tap and VerticalDrag to operate.
        /// These gestures are handled by the stateManager when states change and
        /// all gestures are placed in the InputState passed to the HandleInput method.
        /// </summary>
        public GestureType EnabledGestures
        {
            get { return enabledGestures; }
            protected set
            {
                enabledGestures = value;

                // the state manager handles this during state changes, but
                // if this state is active and the gesture types are changing,
                // we have to update the TouchPanel ourself.
                if (StateBehaviour == StateBehaviour.Active)
                {
                    TouchPanel.EnabledGestures = value;
                }
            }
        }

        private GestureType enabledGestures = GestureType.None;

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Load graphics content for the state.
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Unload content for the state.
        /// </summary>
        public virtual void UnloadContent() { }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Allows the state to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the state
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherstateHasFocus,
                                                      bool coveredByOtherstate)
        {
            this.otherStateHasFocus = otherstateHasFocus;

            if (isExiting)
            {
                // If the state is going away to die, it should transition off.
                stateBehaviour = StateBehaviour.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // When the transition finishes, remove the state.
                    StateManager.RemoveState(this);
                }
            }
            else if (coveredByOtherstate)
            {
                // If the state is covered by another, it should transition off.
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // Still busy transitioning.
                    stateBehaviour = StateBehaviour.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    stateBehaviour = StateBehaviour.Hidden;
                }
            }
            else
            {
                // Otherwise the state should transition on and become active.
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    stateBehaviour = StateBehaviour.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    stateBehaviour = StateBehaviour.Active;
                }
            }
        }

        /// <summary>
        /// Helper for updating the state transition position.
        /// </summary>
        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }

        /// <summary>
        /// Allows the state to handle user input. Unlike Update, this method
        /// is only called when the state is active, and not when some other
        /// state has taken the focus.
        /// </summary>
        public virtual void HandleInput(InputState input) { }

        /// <summary>
        /// This is called when the state should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }

        #endregion Update and Draw

        #region Public Methods

        /// <summary>
        /// Tells the state to go away. Unlike stateManager.Removestate, which
        /// instantly kills the state, this method respects the transition timings
        /// and will give the state a chance to gradually transition off.
        /// </summary>
        public void ExitState()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the state has a zero transition time, remove it immediately.
                StateManager.RemoveState(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }

        #endregion Public Methods
    }
}