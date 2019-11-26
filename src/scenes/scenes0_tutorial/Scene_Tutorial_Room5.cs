﻿using System;
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

namespace EVCMonoGame.src.scenes.tutorial
{
    class Scene_Tutorial_Room5 : Scene
    {
        private Lever leverBit0;
        private Lever leverBit1;
        private Lever leverBit2;
        private LeverManager leverManagerPlayerTwo;

        private Lever leverBit3;
        private Lever leverBit4;
        private Lever leverBit5;
        private LeverManager leverManagerPlayerOne;

        private int elapsedMillisAfterBothDoorsOpened = 0;

        public Scene_Tutorial_Room5(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            tilemap = new Tilemap(Vector2.Zero,
                "Content/rsrc/tilesets/configFiles/tilemaps/scenes0_tutorial/room5.tm.txt");

            PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.WorldPosition = new Vector2(865, 1695);

            playerTwo.WorldPosition = new Vector2(1780, 1690);

            doorPlayerOne = new Door(new Vector2(832, 127));
            doorPlayerOne.BlockPlayerInteraction = true;

            doorPlayerTwo = new Door(new Vector2(1860, 120));
            doorPlayerTwo.BlockPlayerInteraction = true;

            leverBit0 = new Lever(new Vector2(1986, 130));
            leverBit1 = new Lever(new Vector2(1727, 133));
            leverBit2 = new Lever(new Vector2(1601, 130));
            leverManagerPlayerTwo = new LeverManager(true, leverBit2, leverBit1, leverBit0);

            leverBit3 = new Lever(new Vector2(1090, 132));
            leverBit4 = new Lever(new Vector2(960, 130));
            leverBit5 = new Lever(new Vector2(700, 130));
            leverManagerPlayerOne = new LeverManager(true, leverBit5, leverBit4, leverBit3);

            updateables.AddRange(new IUpdateable[]
            {
                doorPlayerOne,
                doorPlayerTwo,

                leverBit0,
                leverBit1,
                leverBit2,
                leverBit3,
                leverBit4,
                leverBit5,
                leverManagerPlayerOne,
                leverManagerPlayerTwo,
            });

            drawables.AddRange(new IDrawable[]
            {
                doorPlayerOne,
                doorPlayerTwo,

                leverBit0,
                leverBit1,
                leverBit2,
                leverBit3,
                leverBit4,
                leverBit5,
                leverManagerPlayerOne,
                leverManagerPlayerTwo,
            });

            base.OnEnterScene();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (doorPlayerOne.Open && doorPlayerTwo.Open)
            {
                elapsedMillisAfterBothDoorsOpened += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedMillisAfterBothDoorsOpened >= 2000)
                {
                    sceneManager.SceneTransition(EScene.GAME_OVER);
                }
            }

            if (!leverBit0.Activated && leverBit1.Activated && !leverBit2.Activated &&
                 leverBit3.Activated && !leverBit4.Activated && leverBit5.Activated)
            {
                doorPlayerOne.Open = true;
                doorPlayerTwo.Open = true;
            }
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