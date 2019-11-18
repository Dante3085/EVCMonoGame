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
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.scenes
{
    class DebugScene : Scene
    {
        private Player player;
        private Player player2;

        private Shadow[] shadows = new Shadow[1];
		
        private SpriteFont randomText;
        private Tilemap tilemap;

        public DebugScene(SceneManager sceneManager)
            : base(sceneManager)
        {
        }

        public override void OnEnterScene()
        {
			base.OnEnterScene();

			player = GameplayState.PlayerOne;
			player.WorldPosition = new Vector2(400, 1000);

			if (GameplayState.IsTwoPlayer)
			{
				player2 = GameplayState.PlayerTwo;
				player2.WorldPosition = new Vector2(200, 500);
				player2.DoesUpdateMovement = false;
			}

            // tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/firstTilemapEditorLevel.tm.txt");
            //tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/collisiontest.tm.txt");
            // tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/chronoTriggerLevel.tm.txt");
            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/ff6Level.tm.txt");

            updateables.AddRange(new IUpdateable[]
            {
            });

            drawables.AddRange(new IDrawable[]
            {
            });

            //        Random rnd = new Random();
            //        for (int i = 0; i < shadows.Length; ++i)
            //        {
            //Vector2 spawnPosition = new Vector2(800, 800);
            //Rectangle spawnBounds = new Rectangle(spawnPosition.ToPoint(), new Point(100, 100));
            //if (!CollisionManager.IsCollisionInArea(spawnBounds, CollisionManager.allCollisionsChannel))
            //{
            //	shadows[i] = new Shadow(3000, 2000, spawnPosition);
            //	updateables.Add(shadows[i]);
            //	drawables.Add(shadows[i]);
            //}
            //else
            //	i--;

            //        }

            shadows[0] = new Shadow(200, 200, new Vector2(800, 1000));
            updateables.Add(shadows[0]);
            drawables.Add(shadows[0]);

            camera.SetCameraToFocusObject(player.Sprite, Screenpoint.CENTER);
            camera.Zoom = 1.25f;
        }

        public override void OnExitScene()
		{
			base.OnExitScene();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            tilemap.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
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

            if (InputManager.OnButtonPressed(Buttons.A) &&
                CollisionManager.IsPlayerInArea(new Rectangle(1480, 1280, 365, 255)))
            {
                sceneManager.SceneTransition(EScene.DEBUG_2);
            }
			
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            tilemap.Draw(gameTime, spriteBatch);
          
            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
