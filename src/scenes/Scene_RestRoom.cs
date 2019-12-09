﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.Items;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.input;

namespace EVCMonoGame.src.scenes
{
    // TODO: Zwischen jeder Karte in RestRoom.
    // TODO: Wie bekommt RestRoom mitgeteilt in welchen Raum er überleiten soll.

    public class Scene_RestRoom : Scene
    {
		private Rectangle doorAreaSora = new Rectangle(650, 130, 365, 255);
		private Rectangle doorAreaRiku = new Rectangle(2550, 122, 450, 265);

        public Scene_RestRoom(SceneManager sceneManager)
            : base(sceneManager)
        {
            
        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            camera.FollowPlayers();

			tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/tilemaps/other/restRoom.tm.txt");

			PlayerOne playerOne = GameplayState.PlayerOne;
			playerOne.PlayerInventory.Gold = 100;
			playerOne.CurrentHp = 10;
			playerOne.WorldPosition = new Vector2(600, 800);

			PlayerTwo playerTwo = GameplayState.PlayerTwo;
			playerTwo.PlayerInventory.Gold = 500;
			playerTwo.WorldPosition = new Vector2(900, 750);


			Item potion = new Healthpotion(new Vector2(1200, 3800));
			Item potion_2 = new Healthpotion(new Vector2(1250, 3800));
			

			Enemy shadow = new Shadow(new Vector2(1350, 4150));
			Enemy shadow_2 = new Shadow(new Vector2(1300, 4150));

			// Shop
			Item inventoryItem = new Healthpotion(new Vector2(1300, 3800));
			Item inventoryItem_2 = new GodMissleScroll(new Vector2(1350, 3820));
			Shop soraShop = new Shop(new Vector2(350, 900), new List<Item> { inventoryItem, inventoryItem_2 }, GameplayState.Lane.LaneOne);


			// Shop
			Item weapon = new CoinBombMissle(new Vector2(1300, 3800));
			Item weapon_2 = new PenetrateMissle(new Vector2(1350, 3820));
			Shop rikuShop = new Shop(new Vector2(1350, 700), new List<Item> { weapon, weapon_2 }, GameplayState.Lane.LaneTwo);

			//Drawables	
			//Items
			drawables.Add(potion);
			drawables.Add(potion_2);

				//Shop
			drawables.Add(soraShop);
			drawables.Add(rikuShop);

			//Enemys
			drawables.Add(shadow);
			drawables.Add(shadow_2);

			//Updatables
				//Items
			updateables.Add(potion);
			updateables.Add(potion_2);
				//Shop
			updateables.Add(soraShop);
			updateables.Add(rikuShop);
			//Enemys
			updateables.Add(shadow);
			updateables.Add(shadow_2);
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

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Go to BarrenFallsEntrance
			if ((CollisionManager.IsPlayerInArea(PlayerIndex.One, doorAreaSora) && InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) ||
				CollisionManager.IsPlayerInArea(PlayerIndex.Two, doorAreaRiku) && InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two)))
			{
				sceneManager.SceneTransitionNextRoom();
			}
		}
	}
}
