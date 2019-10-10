
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.input;

namespace EVCMonoGame.src.screens
{
    public class MenuScreen : GameScreen
    {
        private List<MenuEntry> menuEntries;
        private int selectedEntry;
        private SpriteFont menuFont;

        private Easer pulsatingEaser;
        private Easer fadeEaser;
        private Easer translationEaser;
        private bool transitioning;

        private Vector2 positionFirstEntry;

        public SpriteFont MenuFont
        {
            get { return menuFont; }
        }

        public IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        public MenuScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            menuEntries = new List<MenuEntry>();
            selectedEntry = 0;

            pulsatingEaser = new Easer(1, 1.2f, 1000, Easing.SineEaseInOut);
            fadeEaser = new Easer(255, 0, 1000, Easing.LinearEaseOut);
            translationEaser = new Easer(screenManager.GraphicsDevice.Viewport.Width / 2.0f,
                screenManager.GraphicsDevice.Viewport.Width, 1000, Easing.LinearEaseIn);
            transitioning = false;

            positionFirstEntry = new Vector2(screenManager.GraphicsDevice.Viewport.Width / 2.0f,
                screenManager.GraphicsDevice.Viewport.Height / 2.0f);

            pulsatingEaser.start();
        }

        public override void LoadContent(ContentManager content)
        {
            menuFont = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            pulsatingEaser.Update(gameTime);
            if (pulsatingEaser.IsFinished)
            {
                pulsatingEaser.reverse();
                pulsatingEaser.start();
            }
            menuEntries[selectedEntry].Scale = pulsatingEaser.CurrentValue;

            if (transitioning)
            {
                translationEaser.Update(gameTime);
                fadeEaser.Update(gameTime);

                foreach(MenuEntry m in menuEntries)
                {
                    Vector2 newPos = m.Position;
                    newPos.X += translationEaser.CurrentValue;

                    m.Position = newPos;
                    m.Alpha = (int)fadeEaser.CurrentValue;
                }
            }
            PositionMenuEntries();
            HandleInput();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                bool isSelected = selectedEntry == i;
                menuEntries[i].Draw(spriteBatch, this, isSelected);
            }
            spriteBatch.End();
        }

        private void HandleInput()
        {
            if (InputManager.OnKeyPressed(Keys.Up))
            {
                if (--selectedEntry == -1)
                    selectedEntry = menuEntries.Count - 1;
            }
            else if (InputManager.OnKeyPressed(Keys.Down))
            {
                if (++selectedEntry == menuEntries.Count)
                    selectedEntry = 0;
            }
            else if (InputManager.OnKeyPressed(Keys.Enter))
            {
                menuEntries[selectedEntry].OnPressed();
            }
        }

        private void PositionMenuEntries()
        {
            float entryHeight = menuEntries[0].GetHeight(this);
            float entryWidth = menuEntries[0].GetWidth(this);

            for (int i = 0; i < menuEntries.Count; ++i)
            {
                Vector2 entryPosition = positionFirstEntry;
                entryPosition.Y += i * entryHeight;
                entryPosition.X -= entryWidth;
                menuEntries[i].Position = entryPosition;
            }
        }
    }
}