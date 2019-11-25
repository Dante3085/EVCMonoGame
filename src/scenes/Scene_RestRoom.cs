using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.scenes
{
    // TODO: Tilemap vor Enemy initialisieren.

    public class Scene_RestRoom : Scene
    {
        private Tilemap tilemap;

        public Scene_RestRoom(SceneManager sceneManager)
            : base(sceneManager)
        {
            
        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

			tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/tilemaps/other/restRoom.tm.txt");

			PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.WorldPosition = new Vector2(1150, 3350);

            playerTwo.WorldPosition = new Vector2(1400, 3350);
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
