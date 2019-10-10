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
using EVCMonoGame.src.gui;

namespace EVCMonoGame.src.screens
{
    class DebugScreen : GameScreen
    {
        private Player player;
        private SpriteFont randomText;

        private Random random;

        public DebugScreen(ScreenManager screenManager, Vector2 position)
            : base(screenManager)
        {
            player = new Player();

            updateables.AddRange(new Updateable[] 
            { 
                player,
            });

            drawables.AddRange(new IDrawable[] 
            { 
                player,
            });

            random = new Random();
        }

        public override void LoadContent(ContentManager content)
        {
            randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.OnKeyPressed(Keys.Space))
            {
                screenManager.ScreenTransition(new DebugScreen(screenManager, 
                    new Vector2(random.Next(0, 1000), random.Next(0, 1000))));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
