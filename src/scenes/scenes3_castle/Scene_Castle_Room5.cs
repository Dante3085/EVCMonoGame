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

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.input;
using EVCMonoGame.src.Items.usableItems;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.Traps;

namespace EVCMonoGame.src.scenes.castle
{
    // TODO: Throne Room.

    class Scene_Castle_Room5 : Scene
    {
        private Shadow shadow;
        private Shadow shadow2;

        private Hades hades;

        private Rectangle roomTopHalf = new Rectangle(0, 0, 4092, 2382);
        private Vector2 roomTopHalfPosition = new Vector2();

        private Rectangle roomBottomHalf = new Rectangle(0, 2383, 3717, 2268);
        private Vector2 roomBottomHalfPosition = new Vector2();

        private bool hadesWasPreviouslyAlive = true;

        private FinalItem finalItemSora;
        private FinalItem finalItemRiku;

        private Rectangle doorArea = new Rectangle(1850, 990, 440, 290);

        public Scene_Castle_Room5(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

		public void spawnTraps(Vector2 pos)
		{
			List<Trap> spikeTraps = new List<Trap>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 1 && j == 1 || i == 1 && j == 2 || i == 2 && j == 1 || i == 2 && j == 2)
                    {
                        SpikeTrap spikeTrap = new SpikeTrap(pos + new Vector2(j * 150, i * 130));
                        updateables.Add(spikeTrap);
                        drawables.Add(spikeTrap);
                        spikeTraps.Add(spikeTrap);
                    }
                    else
                    {
                        FireTrap fireTrap = new FireTrap(pos + new Vector2(j * 150, i * 130));
                        updateables.Add(fireTrap);
                        drawables.Add(fireTrap);
                    }
                }
            }


			TrapRemote remote = new TrapRemote(pos + new Vector2(0, 600), spikeTraps);
			updateables.Add(remote);
			drawables.Add(remote);
		}

        public override void OnEnterScene()
		{

			spawnTraps(new Vector2(900, 3000));
			spawnTraps(new Vector2(2600, 3000));
			spawnTraps(new Vector2(900, 1400));
			spawnTraps(new Vector2(2600, 1400));

			base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room5.tm.txt");

            sora.WorldPosition = new Vector2(1800, 4000);
            riku.WorldPosition = new Vector2(2200, 4000);

            shadow = new Shadow(new Vector2(2000, 3000));
            shadow2 = new Shadow(new Vector2(2500, 3000));
            hades = new Hades(new Vector2(2000, 2000));


            updateables.AddRange(new IUpdateable[]
			{
				shadow,
                shadow2,
                hades,
			});

            drawables.AddRange(new IDrawable[]
			{
				shadow,
                shadow2,
                hades,
			});

            MediaPlayer.Play(AssetManager.GetSong(ESong.BOSS));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (InputManager.OnKeyPressed(Keys.M))
            {
                camera.MoveCamera(camera.GetCameraPoint(Screenpoint.CENTER), new Vector2(2000, 4000), 3000);
            }
            else if (InputManager.OnKeyPressed(Keys.N))
            {
                camera.MoveCamera(camera.GetCameraPoint(Screenpoint.CENTER), new Vector2(2000, 1300), 3000);
            }

            // Go to BarrenFallsEntrance
            if (!hades.IsAlive &&
               (CollisionManager.IsPlayerInArea(PlayerIndex.One, doorArea) && InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) ||
                CollisionManager.IsPlayerInArea(PlayerIndex.Two, doorArea) && InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two)))
            {
                sceneManager.SceneTransition(EScene.BARREN_FALLS_ENTRANCE);
            }

            if (hadesWasPreviouslyAlive && !hades.IsAlive)
            {
                finalItemSora = new FinalItem(new Vector2(1000, 1600), GameplayState.Lane.LaneOne);
                finalItemRiku = new FinalItem(new Vector2(2800, 1600), GameplayState.Lane.LaneTwo);

                updateablesToAdd.Add(finalItemSora);
                drawablesToAdd.Add(finalItemSora);

                updateablesToAdd.Add(finalItemRiku);
                drawablesToAdd.Add(finalItemRiku);
            }

            hadesWasPreviouslyAlive = hades.IsAlive;
        }
    }
}
