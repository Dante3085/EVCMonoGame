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
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.scenes
{
    public class DebugScreen2 : Scene
    {
        private Player player;
        private SpriteFont randomText;
        private Texture2D background;

        private Tilemap tilemap;


        public DebugScreen2(SceneManager sceneManager)
            : base(sceneManager)
        {
        }

        public override void OnEnterScene()
        {
			base.OnEnterScene();

			player = GameplayState.PlayerOne;
			player.WorldPosition = new Vector2(400, 500);

            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/ff6Level2.tm.txt");
            
            camera.SetCameraToFocusObject(player.Sprite, Screenpoint.CENTER);
            camera.Zoom = 0.5f;

            updateables.AddRange(new IUpdateable[]
            {
				//Enemies
				//Items
				//etc.
            });

            drawables.AddRange(new IDrawable[]
            {
            });
			
        }

        public override void OnExitScene()
        {
            base.OnExitScene();
        }

        public override void LoadContent(ContentManager content)
        {
            tilemap.LoadContent(content);

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
            tilemap.Draw(gameTime, spriteBatch);
            // spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
