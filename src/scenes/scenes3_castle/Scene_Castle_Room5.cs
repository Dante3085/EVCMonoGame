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
using EVCMonoGame.src.characters.enemies;

namespace EVCMonoGame.src.scenes.castle
{
    // TODO: Throne Room.

    class Scene_Castle_Room5 : Scene
    {
        private PlayerOne sora = GameplayState.PlayerOne;
        private PlayerTwo riku = GameplayState.PlayerTwo;

        private Shadow shadow;
        private Shadow shadow2;

        public Scene_Castle_Room5(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room5.tm.txt");

            sora.WorldPosition = new Vector2(1782, 4000);
            riku.WorldPosition = new Vector2(2180, 4000);

            shadow = new Shadow(new Vector2(2000, 3000));
            shadow2 = new Shadow(new Vector2(2500, 3000));

            updateables.AddRange(new IUpdateable[]
            {
                shadow,
                shadow2,
            });

            drawables.AddRange(new IDrawable[]
            {
                shadow,
                shadow2
            });

            camera.FollowFocusObjects(GameplayState.PlayerOne, GameplayState.PlayerTwo, shadow);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
