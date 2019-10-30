﻿using System;
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
            this.camera = new Camera(sceneManager, player.Sprite, Screenpoint.CENTER, new Vector2(-player.Sprite.Bounds.Width*0.5f, -player.Sprite.Bounds.Height * 0.5f));
            this.camera.setZoom(1.2f);
            geometryBox = new GeometryBox(new Rectangle(300, 400, 200, 1000));

            collisionManager = new CollisionManager();
            collisionManager.AddGeometryCollidables(player.Sprite, geometryBox);

            beachTilemap = new Tilemap("Content/rsrc/tilesets/configFiles/kh_beach.txt", Vector2.Zero);

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
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.getTransformationMatrix());

            // spriteBatch.Draw(background, sceneManager.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.DrawString(randomText, "This is random Text inside the DebugScreen.",
                new Vector2(100, 100), Color.DarkRed);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
