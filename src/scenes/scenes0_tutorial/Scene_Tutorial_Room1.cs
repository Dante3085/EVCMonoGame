using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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

            sora.WorldPosition = new Vector2(2300, 1300);
            sora.Sprite.SetAnimation("IDLE_RIGHT");

            riku.WorldPosition = new Vector2(2300, 1900);
            riku.Sprite.SetAnimation("IDLE_RIGHT");

            doorPlayerOne = new Door(new Vector2(5300, 7));
            doorPlayerTwo = new Door(new Vector2(6847, 3));
			

            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
            });

            base.OnEnterScene();

            camera.Zoom = 0.7f;
            //camera.MinZoom = 0.6f;
            //camera.MaxZoom = 0.4f;

            MediaPlayer.Play(AssetManager.GetSong(ESong.BEGINNING));
            MediaPlayer.IsRepeating = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
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
    }
}
