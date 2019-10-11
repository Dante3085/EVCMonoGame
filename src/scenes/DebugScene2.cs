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

namespace EVCMonoGame.src.scenes
{
    public class DebugScreen2 : Scene
    {
        private Player player;
        private SpriteFont randomText;
        private Texture2D background;

        public DebugScreen2(SceneManager sceneManager)
            : base(sceneManager)
        {
            player = new Player(Vector2.Zero);

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

            background = content.Load<Texture2D>("rsrc/backgrounds/background");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.OnKeyPressed(Keys.Space))
            {
                sceneManager.SceneTransition(EScene.DEBUG);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
