using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace EVCMonoGame.src
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<IUpdateable> updateables; // Hält alle Sachen die geupdatet werden können.
        private List<IDrawable> drawables;     // Hält alle Sachen die gezeichnet werden können.

        private FpsCounter fpsCounter;
        private AnimatedSprite cronoSprite;
        private AnimatedSprite phantomSprite;

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

            fpsCounter = new FpsCounter(Vector2.Zero, Color.White);

            cronoSprite = new AnimatedSprite("rsrc/spritesheets/CronoTransparentBackground", 
                new Vector2(100, 100), 8.0f);

            // Frames sind leicht falsch(Abgeschnittene Ecken).
            cronoSprite.AddAnimation("IDLE", new Rectangle[]
            {
                new Rectangle(59, 14, 15, 34), new Rectangle(79, 14, 15, 34), new Rectangle(99, 14, 15, 34)
            }, 0.8f);
            cronoSprite.AddAnimation("WALK_UP", new Rectangle[]
            {
                new Rectangle(130, 59, 17, 32), new Rectangle(152, 60, 17, 31), new Rectangle(174, 57, 15, 34), 
                new Rectangle(193, 57, 15, 34), new Rectangle(213, 60, 17, 31), new Rectangle(235, 59, 17, 32),
            }, 0.15f);
            cronoSprite.AddAnimation("WALK_LEFT", new Rectangle[]
            {
                new Rectangle(34, 683, 14, 33), new Rectangle(56, 684, 13, 32), new Rectangle(75, 685, 21, 31),
                new Rectangle(103, 683, 13, 33), new Rectangle(125, 684, 14, 32), new Rectangle(145, 685, 20, 32)
            }, 0.15f);
            cronoSprite.AddAnimation("WALK_DOWN", new Rectangle[]
            {
                new Rectangle(130, 15, 15, 33), new Rectangle(150, 17, 16, 31), new Rectangle(171, 14, 17, 34), 
                new Rectangle(193, 15, 15, 33), new Rectangle(213, 17, 16, 31),
            }, 0.15f);
            cronoSprite.AddAnimation("WALK_RIGHT", new Rectangle[]
            {
                new Rectangle(126, 100, 19, 31), new Rectangle(151, 99, 14, 32), new Rectangle(174, 98, 13, 33), 
                new Rectangle(194, 100, 21, 31), new Rectangle(221, 99, 13, 32), new Rectangle(242, 98, 14, 33),
            }, 0.15f);

            phantomSprite = new AnimatedSprite("rsrc/spritesheets/14_phantom_spritesheet", new Vector2(300, 100), 8.0f);

            updateables = new List<IUpdateable>();
            updateables.Add(fpsCounter);
            updateables.Add(cronoSprite);
            updateables.Add(phantomSprite);

            drawables = new List<IDrawable>();
            drawables.Add(fpsCounter);
            drawables.Add(cronoSprite);
            drawables.Add(phantomSprite);

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
            
            foreach (IDrawable drawable in drawables)
            {
                drawable.LoadContent(Content);
            }

            phantomSprite.AddAnimation("IDLE", 100, 100, 0, 0, 61, 0.05f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach(IDrawable drawable in drawables)
            {
                drawable.UnloadContent();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GlobalState.keyboardState = Keyboard.GetState();
            GlobalState.gamePadState = GamePad.GetState(PlayerIndex.One);

            if (GlobalState.gamePadState.IsButtonDown(Buttons.Start) || 
                GlobalState.keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (GlobalState.keyboardState.IsKeyDown(Keys.W))
            {
                cronoSprite.SetAnimation("WALK_UP");
            }
            else if (GlobalState.keyboardState.IsKeyDown(Keys.A))
            {
                cronoSprite.SetAnimation("WALK_LEFT");
            }
            else if (GlobalState.keyboardState.IsKeyDown(Keys.S))
            {
                cronoSprite.SetAnimation("WALK_DOWN");
            }
            else if (GlobalState.keyboardState.IsKeyDown(Keys.D))
            {
                cronoSprite.SetAnimation("WALK_RIGHT");
            }

            foreach (IUpdateable updateable in updateables)
            {
                updateable.Update(gameTime);
            }

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

            foreach(IDrawable drawable in drawables)
            {
                drawable.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
