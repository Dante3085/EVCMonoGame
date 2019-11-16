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
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;

namespace EVCMonoGame.src.scenes
{
    class DebugScene : Scene
    {
        private Player player;
        private Player player2;

        private Shadow[] shadows = new Shadow[50];

        private GeometryBox geometryBox;
        private GeometryBox geometryBox2;
        private SpriteFont randomText;
        private Texture2D background;
        private Tilemap tilemap;

        public DebugScene(SceneManager sceneManager)
            : base(sceneManager)
        {
            OnEnterScene();
        }

        public override void OnEnterScene()
        {
            player = new Player(3000, 2000, new Vector2(500, 1200),
                new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left }, 8);

            player2 = new Player(5000, 3000, new Vector2(200, 500), new Keys[] { Keys.W, Keys.S, Keys.D, Keys.A }, 8);
            player2.DoesUpdateMovement = false;

            sceneManager.GlobalDebugTexts.Entries.Add("playerPos");
            sceneManager.GlobalDebugTexts.Entries.Add("playerBounds");
            sceneManager.GlobalDebugTexts.Entries.Add("elapsed:");
            sceneManager.GlobalDebugTexts.Entries.Add("CurrentAnim:");
            sceneManager.GlobalDebugTexts.Entries.Add("CurrentFrameIndex:");

            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/firstTilemapEditorLevel.tm.txt");
            // tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/collisiontest.tm.txt", collisionManager);
            tilemap.LoadContent(sceneManager.Content);

            updateables.AddRange(new IUpdateable[]
            {
                player,
                player2,
            });

            drawables.AddRange(new IDrawable[]
            {
                player,
                player2,
            });

            base.LoadContent(sceneManager.Content);

            Random rnd = new Random();
            for (int i = 0; i < shadows.Length; ++i)
            {
                shadows[i] = new Shadow(3000, 2000, new Vector2(rnd.Next(0, 3000), rnd.Next(0, 1080)));

                updateables.Add(shadows[i]);
                drawables.Add(shadows[i]);
            }

            camera.SetCameraToFocusObject(player.Sprite, Screenpoint.CENTER);
            camera.SetZoom(1.25f);
        }

        public override void OnExitScene()
        {
            base.OnExitScene();
        }

        public override void LoadContent(ContentManager content)
        {
            tilemap.LoadContent(content);

            randomText = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");

            background = content.Load<Texture2D>("rsrc/backgrounds/map1");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.OnKeyCombinationPressed(Keys.LeftControl, Keys.LeftAlt, Keys.A)) Console.WriteLine("PRESSED");

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
            sceneManager.GlobalDebugTexts.Entries[2] = "ShadowAnimElapsed: " + shadows[0].Sprite.ElapsedMillis;
            sceneManager.GlobalDebugTexts.Entries[3] = "ShadowCurrentAnim: " + shadows[0].Sprite.CurrentAnimation;
            sceneManager.GlobalDebugTexts.Entries[4] = "ShadowAnimFrameIndex: " + shadows[0].Sprite.FrameIndex;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());
            tilemap.Draw(gameTime, spriteBatch);
            // spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
