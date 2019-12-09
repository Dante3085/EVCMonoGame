using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.input;
using EVCMonoGame.src.Items.usableItems;
using EVCMonoGame.src.collision;

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

        private FinalItem finalItemSora;
        private FinalItem finalItemRiku;

        private Rectangle doorArea = new Rectangle(1850, 990, 440, 290);

        public Scene_Castle_Room5(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes3_castle/room5.tm.txt");

            sora.WorldPosition = new Vector2(1700, 2000);
            riku.WorldPosition = new Vector2(2500, 2000);

            shadow = new Shadow(new Vector2(2000, 3000));
            shadow2 = new Shadow(new Vector2(2500, 3000));
            hades = new Hades(new Vector2(2000, 2000));

            finalItemSora = new FinalItem(new Vector2(1000, 1600), GameplayState.Lane.LaneOne);
            finalItemRiku = new FinalItem(new Vector2(2800, 1600), GameplayState.Lane.LaneTwo);

            updateables.AddRange(new IUpdateable[]
            {
                shadow,
                shadow2,
                hades,
                finalItemSora,
                finalItemRiku,
            });

            drawables.AddRange(new IDrawable[]
            {
                shadow,
                shadow2,
                hades,
                finalItemSora,
                finalItemRiku,
            });
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

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            //// RoomTopHalf
            //if (roomTopHalf.Contains(sora.WorldPosition) && !roomTopHalf.Contains(sora.PreviousWorldPosition))
            //{
            //    camera.MoveCamera(roomBottomHalfPosition, roomTopHalfPosition, 1000);
            //}

            //// RoomBottomHalf
            //else if (roomBottomHalf.Contains(sora.WorldPosition) && !roomBottomHalf.Contains(sora.PreviousWorldPosition))
            //{
            //    camera.MoveCamera(roomTopHalfPosition, roomBottomHalfPosition, 1000);
            //}
        }
    }
}
