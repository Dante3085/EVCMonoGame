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
using EVCMonoGame.src.Items;

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

            sora.WorldPosition = new Vector2(1400, 4600);
            sora.Sprite.SetAnimation("IDLE_UP");

            riku.WorldPosition = new Vector2(3300, 4500);
            riku.Sprite.SetAnimation("IDLE_UP");

            doorPlayerOne = new Door(new Vector2(4548, 577));
            doorPlayerTwo = new Door(new Vector2(5568, 1537));

			Chest chest_riku = new Chest(new Vector2(3570, 2900), new List<Item>(){
																			new BounceMissle(Vector2.Zero),
																			new BounceMissle(Vector2.Zero),
																			new BounceMissle(Vector2.Zero),
																			new PenetrateMissle(Vector2.Zero),
																			new PenetrateMissle(Vector2.Zero),
																			new PenetrateMissle(Vector2.Zero),
																			new SplitMissle(Vector2.Zero)
			}, GameplayState.Lane.LaneTwo);

			updateables.AddRange(new IUpdateable[]
			{
				chest_riku,
			});

			drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,
				chest_riku
			});

            base.OnEnterScene();

            camera.Zoom = 0.7f;
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
