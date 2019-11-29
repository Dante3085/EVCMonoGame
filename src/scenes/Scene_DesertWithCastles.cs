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
using EVCMonoGame.src.Items;

namespace EVCMonoGame.src.scenes
{
    // TODO: Quasi Hub-Scene durch die beide anderen Scenes erreichbar sind.
    //       Man braucht aber erst Schlüssel aus Scene_Forest, um Scene_InsideCastle 
    //       betreten zu können.

    class Scene_DesertWithCastles : Scene
    {
        private Shadow shadow;
        private Defender defender;
        private Gargoyle gargoyle;

        private Item[] items = new Item[30];

        public Scene_DesertWithCastles(SceneManager sceneManager)
            : base(sceneManager)
        {
        }

        public override void OnEnterScene()
        {
			base.OnEnterScene();

            GameplayState.PlayerOne.WorldPosition = new Vector2(600, 2000);

			if (GameplayState.IsTwoPlayer)
			{
                GameplayState.PlayerTwo.WorldPosition = new Vector2(600, 1500);
			}

            // tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/firstTilemapEditorLevel.tm.txt");
            //tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/collisiontest.tm.txt");
            // tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/chronoTriggerLevel.tm.txt");

            // Kollisiondaten explizit an CollisionManager geben. Wird nicht einfach in Tilemap gemacht.
            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/tilemaps/other/Level_DesertWithCastles.tm.txt");

            shadow = new Shadow(new Vector2(800, 1000), this);
            defender = new Defender(new Vector2(800, 900), this);
            gargoyle = new Gargoyle(new Vector2(800, 1300), this);

        updateables.AddRange(new IUpdateable[]
            {
                defender, 
                shadow,
                gargoyle,
            });

            drawables.AddRange(new IDrawable[]
            {
                defender,
                shadow,
                gargoyle,
            });

            sceneManager.GlobalDebugTexts.Entries[0] = "ShadowAnimFrame: ";
            camera.Zoom = .75f;

            for (int i = 0; i < items.Length; ++i)
            {
                items[i] = new InstantConsumable(new Vector2(1000, 2500 + 100*i));
                drawables.Add(items[i]);
                updateables.Add(items[i]);
            }
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
            sceneManager.GlobalDebugTexts.Entries[0] = "DefenderAnimFrame: " + defender.Sprite.FrameIndex;

            

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
                camera.MoveCamera(camera.FocusObject.Position, GameplayState.PlayerOne.Sprite.Position, 1000);
            }
            if (camera.FocusObject.Position == GameplayState.PlayerOne.Sprite.Position)
            {
                camera.SetCameraToFocusObject(GameplayState.PlayerOne.Sprite);
            }

            if (InputManager.OnKeyPressed(Keys.Space))
            {
                sceneManager.SceneTransition(EScene.INSIDE_CASTLE);
            }

            if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One))
            {
                if (CollisionManager.IsPlayerInArea(PlayerIndex.One, new Rectangle(1480, 1280, 365, 255)))
                {
                    sceneManager.SceneTransition(EScene.INSIDE_CASTLE);
                }
                else if (CollisionManager.IsPlayerInArea(PlayerIndex.One, new Rectangle(7500, 3000, 400, 200)))
                {
                    GameplayState.PlayerOne.WorldPosition = new Vector2(7700, 2200);
                }
                else if (CollisionManager.IsPlayerInArea(PlayerIndex.One, new Rectangle(7640, 2220, 200, 180)))
                {
                    GameplayState.PlayerOne.WorldPosition = new Vector2(7630, 3030);
                }
            }

            if (InputManager.OnKeyPressed(Keys.C))
            {
                camera.MoveCamera(new Vector2(300, 300), new Vector2(800, 800), 2000);
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
