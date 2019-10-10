using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Pressed += PlayGameMenuEntryPressed;
            optionsMenuEntry.Pressed += OptionsMenuEntryPressed;
            exitMenuEntry.Pressed += ExitMenuEntryPressed;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void PlayGameMenuEntryPressed(object sender, EventArgs e)
        {
            Console.WriteLine("PlayGameMenuEntrySelected");
        }

        private void OptionsMenuEntryPressed(object sender, EventArgs e)
        {
            Console.WriteLine("OptionsMenuEntrySelected");
        }

        private void ExitMenuEntryPressed(object sender, EventArgs e)
        {
            Console.WriteLine("ExitMenuEntryPressed");
            screenManager.Exit();
        }
    }
}
