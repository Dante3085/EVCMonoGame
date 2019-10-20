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
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.scenes
{
    class DebugScene : Scene
    {
        private Player player;
        private Player player2;
        private SpriteFont randomText;
        private Texture2D background;
        private GeometryBox geometryBox;
        private GeometryBox geometryBox2;
        private Tilemap tilemap;

        private CollisionManager collisionManager;

        public DebugScene(SceneManager sceneManager)
            : base(sceneManager)
        {
            player = new Player(sceneManager.GetViewportCenter(), 
                new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left });

            player2 = new Player(new Vector2(200, 500), new Keys[] { Keys.W, Keys.S, Keys.D, Keys.A });
            player2.DoesUpdateMovement = false;

            geometryBox = new GeometryBox(new Rectangle(550, 370, 800, 100));
            geometryBox2 = new GeometryBox(new Rectangle(1400, 480, 500, 25));
            tilemap = new Tilemap("Content/rsrc/tilesets/configFiles/kh.txt", Vector2.Zero);

            sceneManager.GlobalDebugTexts.Entries.Add("playerPos");
            sceneManager.GlobalDebugTexts.Entries.Add("playerBounds");
            sceneManager.GlobalDebugTexts.Entries.Add("elapsed:");
            sceneManager.GlobalDebugTexts.Entries.Add("CurrentAnim:");
            sceneManager.GlobalDebugTexts.Entries.Add("CurrentFrameIndex:");

            collisionManager = new CollisionManager();
            collisionManager.AddGeometryCollidables(player.Sprite, player2.Sprite, geometryBox, geometryBox2);
            collisionManager.AddCombatCollidables(player, player2);

            updateables.AddRange(new Updateable[] 
            { 
                player,
                player2,
                collisionManager,
            });

            drawables.AddRange(new IDrawable[] 
            { 
                tilemap,
                player,
                player2,
                collisionManager,
            });
        }

        public override void LoadContent(ContentManager content)
        {
            randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            background = content.Load<Texture2D>("rsrc/backgrounds/map1");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.OnKeyPressed(Keys.Space))
            {
                sceneManager.SceneTransition(EScene.DEBUG_2);
            }

            sceneManager.GlobalDebugTexts.Entries[0] = "PlayerPos: " + player.Sprite.Position;
            sceneManager.GlobalDebugTexts.Entries[1] = "PlayerBounds: " + player.Sprite.Bounds;
            sceneManager.GlobalDebugTexts.Entries[2] = "ElapsedMillis: " + player.Sprite.ElapsedMillis;
            sceneManager.GlobalDebugTexts.Entries[3] = "CurrentAnim: " + player.Sprite.CurrentAnimation;
            sceneManager.GlobalDebugTexts.Entries[3] = "FrameIndex: " + player.Sprite.FrameIndex;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
