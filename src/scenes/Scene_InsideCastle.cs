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
    // TODO: Diese Scene ist das letzte Level und ist erst erreichbar, wenn in Scene_Forest
    //       ein Schlüssel zur Tür am großen Schloss in Scene_DesertWithCastles gefunden wurde.

    public class Scene_InsideCastle : Scene
    {
        private SpriteFont randomText;
        private Texture2D background;

        private Tilemap tilemap;


        public Scene_InsideCastle(SceneManager sceneManager)
            : base(sceneManager)
        {
        }

        public override void OnEnterScene()
        {
			base.OnEnterScene();

            GameplayState.PlayerOne.WorldPosition = new Vector2(-150, 100);
            GameplayState.PlayerTwo.WorldPosition = new Vector2(100, 100);

            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/tilemaps/other/Level_InsideCastle.tm.txt");
            
            camera.Zoom = 1.5f;

            sceneManager.GlobalDebugTexts.Entries.Clear();
            sceneManager.GlobalDebugTexts.Entries.Add("SoraFrameIndex: ");
            sceneManager.GlobalDebugTexts.Entries.Add("RikuFrameIndex: ");

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
                sceneManager.SceneTransition(EScene.SAND_CASTLES);
            }

            if (InputManager.OnKeyPressed(Keys.L))
            {
                GameplayState.PlayerTwo.Sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/riku.anm.txt");
                GameplayState.PlayerOne.Sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/sora.anm.txt");
            }

            sceneManager.GlobalDebugTexts.Entries[0] = "SoraFrameIndex: " + GameplayState.PlayerOne.Sprite.FrameIndex;
            sceneManager.GlobalDebugTexts.Entries[1] = "RikuFrameIndex: " + GameplayState.PlayerTwo.Sprite.FrameIndex;

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
