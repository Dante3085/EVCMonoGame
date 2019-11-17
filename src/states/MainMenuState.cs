
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.states
{
    /// <summary>
    /// The main menu state is the first thing displayed when the game starts up.
    /// </summary>
    internal class MainMenuState : MenuState
    {
        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuState()
            : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry tilemapEditorEntry = new MenuEntry("Tilemap Editor");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            tilemapEditorEntry.Selected += TilemapEditorMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(tilemapEditorEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion Initialization

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingState.Load(StateManager, true, e.PlayerIndex, new GameplayState());
        }

        private void TilemapEditorMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingState.Load(StateManager, true, e.PlayerIndex, new TilemapEditorState());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            StateManager.AddState(new OptionsMenuState(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxState confirmExitMessageBox = new MessageBoxState(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            StateManager.AddState(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            StateManager.Game.Exit();
        }

        #endregion Handle Input
    }
}