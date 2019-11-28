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
    public class Scene_Tutorial_Room3 : Scene
    {
        private Shadow shadow1;
        private Shadow shadow2;
        private Shadow shadow3;

        public Scene_Tutorial_Room3(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero, 
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room3.tm.txt");

            PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.WorldPosition = new Vector2(1356, 4265);

            playerTwo.WorldPosition = new Vector2(2069, 4376);

            shadow1 = new Shadow(new Vector2(1330, 3000));
            shadow2 = new Shadow(new Vector2(2100, 3000));
            shadow3 = new Shadow(new Vector2(1200, 2500));

            doorPlayerOne = new Door(new Vector2(1550, 801));
            doorPlayerTwo = new Door(new Vector2(2000, 805));

            updateables.AddRange(new IUpdateable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
                shadow1,
                shadow2,
                shadow3,
            });

            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
                shadow1,
                shadow2,
                shadow3,
            });

            camera.Zoom = 0.7f;
            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (doorPlayerOne.Open && doorPlayerTwo.Open /* &&
                !shadow1.IsAlive && !shadow2.IsAlive */)
            {
                sceneManager.SceneTransition(EScene.TUTORIAL_ROOM_4);
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
