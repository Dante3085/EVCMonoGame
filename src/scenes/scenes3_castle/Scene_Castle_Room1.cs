using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.scenes.castle
{
    public class Scene_Castle_Room1 : Scene
    {
        private Tilemap tilemap;

        private PlayerOne sora = GameplayState.PlayerOne;
        private PlayerTwo riku = GameplayState.PlayerTwo;

        public Scene_Castle_Room1(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room1.tm.txt");

            sora.WorldPosition = new Vector2(1540, 3315);
            riku.WorldPosition = new Vector2(2278, 3256);
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
