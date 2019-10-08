
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

        private AnimatedSprite cronoSprite;
        private AnimatedSprite cronoSprite2;
        private float cronoSpeed;

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

            healthbar = new Healthbar(346, 867, new Vector2(100, 100), new Vector2(250, 30));

            cronoSpeed = 8;
            cronoSprite = new AnimatedSprite("rsrc/spritesheets/CronoTransparentBackground",
                new Vector2(100, 100), 6.0f);

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
            cronoSprite.SetAnimation("IDLE");

            cronoSprite2 = new AnimatedSprite("rsrc/spritesheets/CronoTransparentBackground",
                new Vector2(300, 100), 6.0f);

            cronoSprite2.AddAnimation("IDLE", new Rectangle[]
            {
                new Rectangle(59, 14, 15, 34), new Rectangle(79, 14, 15, 34), new Rectangle(99, 14, 15, 34)
            }, 0.8f);
            cronoSprite2.AddAnimation("WALK_UP", new Rectangle[]
            {
                new Rectangle(130, 59, 17, 32), new Rectangle(152, 60, 17, 31), new Rectangle(174, 57, 15, 34),
                new Rectangle(193, 57, 15, 34), new Rectangle(213, 60, 17, 31), new Rectangle(235, 59, 17, 32),
            }, 0.15f);
            cronoSprite2.AddAnimation("WALK_LEFT", new Rectangle[]
            {
                new Rectangle(34, 683, 14, 33), new Rectangle(56, 684, 13, 32), new Rectangle(75, 685, 21, 31),
                new Rectangle(103, 683, 13, 33), new Rectangle(125, 684, 14, 32), new Rectangle(145, 685, 20, 32)
            }, 0.15f);
            cronoSprite2.AddAnimation("WALK_DOWN", new Rectangle[]
            {
                new Rectangle(130, 15, 15, 33), new Rectangle(150, 17, 16, 31), new Rectangle(171, 14, 17, 34),
                new Rectangle(193, 15, 15, 33), new Rectangle(213, 17, 16, 31),
            }, 0.15f);
            cronoSprite2.AddAnimation("WALK_RIGHT", new Rectangle[]
            {
                new Rectangle(126, 100, 19, 31), new Rectangle(151, 99, 14, 32), new Rectangle(174, 98, 13, 33),
                new Rectangle(194, 100, 21, 31), new Rectangle(221, 99, 13, 32), new Rectangle(242, 98, 14, 33),
            }, 0.15f);
            cronoSprite2.SetAnimation("IDLE");

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
            cronoSprite.LoadContent(Content);
            cronoSprite2.LoadContent(Content);
            healthbar.LoadContent(Content);
            healthbar.Position = cronoSprite.Position - new Vector2(0, healthbar.Size.Y);
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

            if (InputManager.OnKeyPressed(Keys.Left)) { cronoSprite.SetAnimation("WALK_LEFT"); }
            else if (InputManager.OnKeyPressed(Keys.Up)) { cronoSprite.SetAnimation("WALK_UP"); }
            else if (InputManager.OnKeyPressed(Keys.Right)) { cronoSprite.SetAnimation("WALK_RIGHT"); }
            else if (InputManager.OnKeyPressed(Keys.Down)) { cronoSprite.SetAnimation("WALK_DOWN"); }
            
            if (InputManager.OnKeyReleased(Keys.Left)
                || InputManager.OnKeyReleased(Keys.Up)
                || InputManager.OnKeyReleased(Keys.Right)
                || InputManager.OnKeyReleased(Keys.Down)) 
            {
                cronoSprite.SetAnimation("IDLE");
            }

            if (InputManager.IsKeyPressed(Keys.Left))
            {
                cronoSprite.Position += new Vector2(-cronoSpeed, 0);
                healthbar.Position = cronoSprite.Position - new Vector2(0, healthbar.Size.Y);
            }

            else if (InputManager.IsKeyPressed(Keys.Up))
            {
                cronoSprite.Position += new Vector2(0, -cronoSpeed);
                healthbar.Position = cronoSprite.Position - new Vector2(0, healthbar.Size.Y);
            }

            else if (InputManager.IsKeyPressed(Keys.Right))
            {
                cronoSprite.Position += new Vector2(cronoSpeed, 0);
                healthbar.Position = cronoSprite.Position - new Vector2(0, healthbar.Size.Y);
            }

            else if (InputManager.IsKeyPressed(Keys.Down))
            {
                cronoSprite.Position += new Vector2(0, cronoSpeed);
                healthbar.Position = cronoSprite.Position - new Vector2(0, healthbar.Size.Y);
            }

            if (InputManager.IsKeyPressed(Keys.A)) { healthbar.CurrentHp -= 1; }
            else if (InputManager.IsKeyPressed(Keys.D)) { healthbar.CurrentHp += 1; }

            cronoSprite.Update(gameTime);
            cronoSprite2.Update(gameTime);

            if (InputManager.OnKeyPressed(Keys.Escape)) { base.Exit(); }
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
            // spriteBatch.Draw(sprite, spritePos, Color.White);
            healthbar.Draw(gameTime, spriteBatch);
            cronoSprite.Draw(gameTime, spriteBatch);
            cronoSprite2.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
