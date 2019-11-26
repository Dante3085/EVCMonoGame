using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.states;
using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;


namespace EVCMonoGame.src.scenes.tutorial
{
    public class Scene_Tutorial_Room1 : Scene
    {
        public Scene_Tutorial_Room1(SceneManager sceneManager)
            : base(sceneManager)
        {
        }

        public override void OnEnterScene()
        {
			tilemap = new Tilemap(Vector2.Zero,
				"Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room1.tm.txt");

			PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.WorldPosition = new Vector2(2300, 1300);
            playerOne.Sprite.SetAnimation("IDLE_RIGHT");

            playerTwo.WorldPosition = new Vector2(2300, 1900);
            playerTwo.Sprite.SetAnimation("IDLE_RIGHT");

            doorPlayerOne = new Door(new Vector2(5300, 7));
            doorPlayerTwo = new Door(new Vector2(6847, 3));

            updateables.AddRange(new IUpdateable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            base.OnEnterScene();

            camera.Zoom = 0.8f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransition(EScene.TUTORIAL_ROOM_2);
            }

            //if (InputManager.OnKeyPressed(Keys.Space))
            //{
            //    camera.MoveCamera(Vector2.Zero, new Vector2(6000, 1500), 5000);
            //}
            //else if (InputManager.OnKeyPressed(Keys.P))
            //{
            //    camera.FollowPlayers();
            //}
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            tilemap.LoadContent(contentManager);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred ,samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            tilemap.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
