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
using EVCMonoGame.src.enemies;
using EVCMonoGame.src.tilemap;

namespace EVCMonoGame.src.scenes
{
    class DebugScene : Scene
    {
        private Player player;
        private Player player2;

        private Shadow shadow;

        private GeometryBox geometryBox;
        private GeometryBox geometryBox2;
        private SpriteFont randomText;
        private Texture2D background;
        private Tilemap tilemap;

        private CollisionManager collisionManager;

        public DebugScene(SceneManager sceneManager)
            : base(sceneManager)
        {
            player = new Player(new Vector2(400, 500), 
                new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left }, 8);

            player2 = new Player(new Vector2(200, 500), new Keys[] { Keys.W, Keys.S, Keys.D, Keys.A }, 8);
            player2.DoesUpdateMovement = false;

            shadow = new Shadow(new Vector2(1000, 600));

            geometryBox = new GeometryBox(new Rectangle(550, 370, 800, 100));
            geometryBox2 = new GeometryBox(new Rectangle(1300, 480, 500, 25));

            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/firstTilemapEditorLevel.tm.txt");

            sceneManager.GlobalDebugTexts.Entries.Add("playerPos");
            sceneManager.GlobalDebugTexts.Entries.Add("playerBounds");
            sceneManager.GlobalDebugTexts.Entries.Add("elapsed:");
            sceneManager.GlobalDebugTexts.Entries.Add("CurrentAnim:");
            sceneManager.GlobalDebugTexts.Entries.Add("CurrentFrameIndex:");

            collisionManager = new CollisionManager();
            collisionManager.AddGeometryCollidables(player.Sprite, player2.Sprite, shadow.Sprite, geometryBox,
                                                    geometryBox2);
            collisionManager.AddCombatCollidables(player, player2, shadow);

            camera.SetCameraToFocusObject(player.Sprite, Screenpoint.CENTER);
            camera.SetZoom(1.25f);

            updateables.AddRange(new Updateable[] 
            { 
                player,
                player2,

                shadow,

                collisionManager,
            });

            drawables.AddRange(new IDrawable[] 
            { 
                tilemap,
                player,
                player2,

                shadow,

                // collisionManager,
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
            if (InputManager.OnKeyPressed(Keys.F1))
            {
                if (drawables.Contains(collisionManager))
                {
                    drawables.Remove(collisionManager);
                }
                else
                {
                    drawables.Add(collisionManager);
                }
            }

            if (InputManager.OnKeyPressed(Keys.H))
            {
                camera.MoveCamera(camera.FocusObject.Position, camera.FocusObject.Position + new Vector2(50, 50), 1000);
            }
            if (InputManager.OnKeyPressed(Keys.J))
            {
                camera.MoveCamera(camera.FocusObject.Position, camera.FocusObject.Position - new Vector2(50, 50), 1000);
            }
            if (InputManager.OnKeyPressed(Keys.K))
            {
                camera.MoveCamera(camera.FocusObject.Position, player.Sprite.Position, 1000);
            }
            if (camera.FocusObject.Position == player.Sprite.Position)
            {
                camera.SetCameraToFocusObject(player.Sprite);
            }

            if (InputManager.OnKeyPressed(Keys.Space))
            {
                sceneManager.SceneTransition(EScene.DEBUG_2);
            }

            sceneManager.GlobalDebugTexts.Entries[0] = "PlayerPos: " + player.Sprite.Position;
            sceneManager.GlobalDebugTexts.Entries[1] = "PlayerBounds: " + player.Sprite.Bounds;
            sceneManager.GlobalDebugTexts.Entries[2] = "Shadow ElapsedMillis: " + shadow.Sprite.ElapsedMillis;
            sceneManager.GlobalDebugTexts.Entries[3] = "Shadow CurrentAnim: " + shadow.Sprite.CurrentAnimation;
            sceneManager.GlobalDebugTexts.Entries[3] = "Shadow FrameIndex: " + shadow.Sprite.FrameIndex;

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
