using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
			playerOne.WorldPosition = new Vector2(1300, 4000);

			PlayerTwo playerTwo = GameplayState.PlayerTwo;
			playerTwo.WorldPosition = new Vector2(2100, 4000);

			Chest chest_sora = new Chest(	new Vector2(1300, 3500),
											new List<Item>(){
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneOne){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneOne, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneOne, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneOne, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneOne, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneOne, 0.5f) { exp = 3},
												new Healthpotion(Vector2.Zero),
											}, GameplayState.Lane.LaneOne);

			Chest chest_riku = new Chest(	new Vector2(2100, 3500),
											new List<Item>(){
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", GameplayState.Lane.LaneTwo){ gold = 1 },
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneTwo, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneTwo, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneTwo, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneTwo, 0.5f) { exp = 3},
												new InstantConsumable(Vector2.Zero, "Content/rsrc/spritesheets/configFiles/exp.anm.txt", "EXP", GameplayState.Lane.LaneTwo, 0.5f) { exp = 3},
												new BounceMissle(Vector2.Zero),
												new PenetrateMissle(Vector2.Zero),										
												new Healthpotion(Vector2.Zero),
											}, GameplayState.Lane.LaneTwo);

			// Shop
			Item healtpotion = new Healthpotion(Vector2.Zero);
			Item bearTrapItem = new BearTrapItem(Vector2.Zero);
			Shop soraShop = new Shop(new Vector2(700, 3250), new List<Item> { healtpotion, bearTrapItem }, GameplayState.Lane.LaneOne);
            
            // Shop
            Item bounceMissle = new BounceMissle(new Vector2(0, 0));
			Item penetrateMissle = new PenetrateMissle(new Vector2(0, 0));
            Item splitMissle = new SplitMissle(new Vector2(0, 0));
            Item godImperator = new GodImperatorMissle(new Vector2(0, 0));
            Shop rikuShop = new Shop(new Vector2(2350, 3250), new List<Item> { bounceMissle, penetrateMissle, splitMissle, godImperator }, GameplayState.Lane.LaneTwo);

			//Drawables	
				//Chest
			drawables.Add(chest_sora);
			drawables.Add(chest_riku);

				//Shop
			drawables.Add(soraShop);
			drawables.Add(rikuShop);
			

			//Updatables
				//Chest
			updateables.Add(chest_sora);
			updateables.Add(chest_riku);
				//Shop
			updateables.Add(soraShop);
			updateables.Add(rikuShop);

			MediaPlayer.Play(AssetManager.GetSong(ESong.REST_ROOM));
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
