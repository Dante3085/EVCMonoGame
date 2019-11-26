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
    public class Scene_Tutorial_Room2 : Scene
    {
        public Scene_Tutorial_Room2(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room2.tm.txt");

            PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.WorldPosition = new Vector2(1400, 4600);
            playerOne.Sprite.SetAnimation("IDLE_UP");

            playerTwo.WorldPosition = new Vector2(3300, 4500);
            playerTwo.Sprite.SetAnimation("IDLE_UP");

            doorPlayerOne = new Door(new Vector2(4548, 577));
            doorPlayerTwo = new Door(new Vector2(5568, 1537));

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
                sceneManager.SceneTransition(EScene.TUTORIAL_ROOM_3);
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            tilemap.LoadContent(contentManager);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            tilemap.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
