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
using EVCMonoGame.src.tilemap;

namespace EVCMonoGame.src.scenes
{
    public class DebugScreen2 : Scene
    {
        private Player player;
        private SpriteFont randomText;
        private Texture2D background;
        private CollisionManager collisionManager;
        private GeometryBox geometryBox;

        private Tilemap beachTilemap;

        public DebugScreen2(SceneManager sceneManager)
            : base(sceneManager)
        {
            player = new Player(Vector2.Zero, new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Left }, 8);
            geometryBox = new GeometryBox(new Rectangle(300, 400, 200, 1000));

            collisionManager = new CollisionManager();
            collisionManager.AddGeometryCollidables(player.Sprite, geometryBox);

            beachTilemap = new Tilemap("Content/rsrc/tilesets/configFiles/kh_beach.txt", Vector2.Zero);

            camera.SetCameraToFocusObject(player.Sprite, Screenpoint.CENTER);
            camera.SetZoom(0.5f);

            updateables.AddRange(new Updateable[]
            {
                player,
                collisionManager,
            });

            drawables.AddRange(new IDrawable[]
            {
                beachTilemap,
                collisionManager,
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
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            // spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
