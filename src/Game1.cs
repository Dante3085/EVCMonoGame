
// usings mit fremden Code
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using C3.MonoGame;
using System;

// usings mit eigenem Code
using EVCMonoGame.src.input;
using EVCMonoGame.src.gui;

namespace EVCMonoGame.src
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont text;
        private String textString;

        private Texture2D sprite;
        private Vector2 spritePos;

        private Healthbar healthbar;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // Fenstergröße auf 1920 x 1080
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            textString = "EMPTY";
            spritePos = new Vector2(0, 0);

            healthbar = new Healthbar(2385, 1654, new Rectangle(100, 100, 200, 25));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            text = Content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            sprite = Content.Load<Texture2D>("rsrc/spritesheets/1_magicspell_spritesheet");
            healthbar.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.UpdateCurrentInputStates();

            if (InputManager.OnKeyPressed(Keys.Right))
            {
                spritePos.X += 50;
                textString = "Right pressed";
            }
            if (InputManager.OnKeyReleased(Keys.Left))
            {
                spritePos.X -= 50;
                textString = "Left Released";
            }
            if (InputManager.OnKeyPressed(Keys.Space))
            {
                healthbar.MaxHp = healthbar.MaxHp == 100 ? 1000 : 100;
                Console.WriteLine("MaxHp: " + healthbar.MaxHp + ", currentHp: " + healthbar.CurrentHp);
            }

            if (InputManager.IsKeyPressed(Keys.Right))
            {
                healthbar.CurrentHp += 1;
            }
            else if (InputManager.IsKeyPressed(Keys.Left))
            {
                healthbar.CurrentHp -= 1;
            }

            if (InputManager.OnKeyPressed(Keys.Q))
            {
                healthbar.DrawOutline = healthbar.DrawOutline ? false : true;
            }

            if (InputManager.OnKeyPressed(Keys.Escape))
            {
                base.Exit();
            }

            InputManager.UpdatePreviousInputStates();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // SamplerState.PointClamp sorgt dafür, dass Sprites nicht verschwommen sind.
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.DrawString(text, textString, Vector2.Zero, Color.White);
            spriteBatch.Draw(sprite, spritePos, Color.White);
            healthbar.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
