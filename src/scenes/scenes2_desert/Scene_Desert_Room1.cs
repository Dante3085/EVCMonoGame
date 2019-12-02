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

namespace EVCMonoGame.src.scenes.desert
{
    public class Scene_Desert_Room1 : Scene
    {
        private Shadow shadow1;
        private Shadow shadow2;

        public Scene_Desert_Room1(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes2_desert/room1.tm.txt");

            sora.WorldPosition = new Vector2(700, 4100);
            riku.WorldPosition = new Vector2(2200, 4100);

            shadow1 = new Shadow(new Vector2(800, 4100));
            shadow2 = new Shadow(new Vector2(2300, 4100));

            doorPlayerOne = new Door(new Vector2(959, 1280));
            doorPlayerTwo = new Door(new Vector2(2240, 1280));

            updateables.AddRange(new IUpdateable[]
            {
                shadow1,
                shadow2,
                doorPlayerOne,
                doorPlayerTwo,
            });

            drawables.AddRange(new IDrawable[]
            {
                shadow1,
                shadow2,
                doorPlayerOne,
                doorPlayerTwo,
            });

            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!shadow1.IsAlive && !shadow2.IsAlive &&
                doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                sceneManager.SceneTransitionNextRoom();
            }
        }
    }
}
