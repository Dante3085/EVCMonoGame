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
        private Tilemap tilemap;

        public Scene_Tutorial_Room3(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room3.tm.txt");
            CollisionManager.AddCollidables(
                CollisionManager.obstacleCollisionChannel, collidables: tilemap.CollisionBoxes.ToArray());


            PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.WorldPosition = new Vector2(10300, 8900);

            playerTwo.WorldPosition = new Vector2(11101, 9070);
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
