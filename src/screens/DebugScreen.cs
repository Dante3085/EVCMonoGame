﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.input;
using EVCMonoGame.src.gui;

namespace EVCMonoGame.src.screens
{
    class DebugScreen : GameScreen
    {
        private Player player;
        private SpriteFont randomText;
        private Texture2D background;

        public DebugScreen(ScreenManager screenManager)
            : base(screenManager)
        {
            player = new Player(screenManager.GetViewportCenter());

            updateables.AddRange(new Updateable[] 
            { 
                player,
            });

            drawables.AddRange(new IDrawable[] 
            { 
                player,
            });
        }

        public override void LoadContent(ContentManager content)
        {
            randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            background = content.Load<Texture2D>("rsrc/backgrounds/cyberpunk-street");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.OnKeyPressed(Keys.Space))
            {
                screenManager.ScreenTransition(EGameScreen.DEBUG_2);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(background, screenManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
