using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.tilemap.tilemapEditor;
using EVCMonoGame.src.input;

namespace EVCMonoGame.src.states
{
    public class TilemapEditorState : GameState
    {
        private TilemapEditor tilemapEditor;
        float pauseAlpha;

        public TilemapEditorState()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            tilemapEditor = new TilemapEditor();
        }

        public override void Update(GameTime gameTime, bool otherstateHasFocus, bool coveredByOtherstate)
        {
            base.Update(gameTime, otherstateHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause state.
            if (coveredByOtherstate)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                tilemapEditor.Update(gameTime);
            }
        }

        public override void HandleInput(InputState input)
        {
            if (InputManager.OnKeyPressed(Keys.Escape)
                || InputManager.OnButtonPressed(Buttons.Start))
            {
                StateManager.AddState(new PauseMenuState(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            StateManager.GraphicsDevice.Clear(ClearOptions.Target,
                                              Color.Black, 0, 0);

            tilemapEditor.Draw(gameTime, StateManager.SpriteBatch);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                StateManager.FadeBackBufferToBlack(alpha);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();
            tilemapEditor.LoadContent(StateManager.Game.Content);
        }
    }
}
