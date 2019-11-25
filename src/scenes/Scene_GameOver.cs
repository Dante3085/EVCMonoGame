using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.states;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.input;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.scenes
{
    public class Scene_GameOver : Scene
    {
        private Easer easerPlayerOne = new Easer(Vector2.Zero, Vector2.Zero, 1500, Easing.SineEaseInOut);
        private Easer easerPlayerTwo = new Easer(Vector2.Zero, Vector2.Zero, 1500, Easing.SineEaseInOut);

        private AnimatedSprite selectionSprite = new AnimatedSprite(new Vector2(650, 100), 4);
        private AnimatedSprite heartSprite = new AnimatedSprite(new Vector2(860, 300), 4);

        public Scene_GameOver(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            selectionSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/gameOverSelection.anm.txt");
            selectionSprite.SetAnimation("CONTINUE");

            heartSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/gameOverHeart.anm.txt");
            heartSprite.SetAnimation("IDLE");

            PlayerOne playerOne = GameplayState.PlayerOne;
            PlayerTwo playerTwo = GameplayState.PlayerTwo;

            playerOne.Sprite.SetAnimation("DEAD");
            playerOne.DrawHealthbar = false;
            playerOne.WorldPosition = sceneManager.GetViewportCenter() + new Vector2(150, -120);
            playerOne.BlockInput = true;
            easerPlayerOne.From = playerOne.WorldPosition;
            easerPlayerOne.To = easerPlayerOne.From - new Vector2(0, -50);

            GameplayState.PlayerTwo.Sprite.SetAnimation("DEAD");
            playerTwo.DrawHealthbar = false;
            playerTwo.WorldPosition = new Vector2(500, 400);
            playerTwo.BlockInput = true;
            easerPlayerTwo.From = playerTwo.WorldPosition;
            easerPlayerTwo.To = easerPlayerTwo.From - new Vector2(0, -50);

            // camera.SetCameraToFocusObject(playerOne.Sprite, Screenpoint.CENTER);
            camera.SetCameraToPosition(new Vector2(920, 350), Screenpoint.CENTER);
            camera.Zoom = 1.5f;

            updateables.AddRange(new IUpdateable[]
            {
                easerPlayerOne,
                easerPlayerTwo,
                selectionSprite,
                heartSprite,
            });

            drawables.AddRange(new IDrawable[]
            {
                selectionSprite,
                heartSprite,
            });

            easerPlayerOne.Start();
            easerPlayerTwo.Start();

            sceneManager.GlobalDebugTexts.Entries.Clear();
        }

        public override void OnExitScene()
        {
            base.OnExitScene();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GameplayState.PlayerOne.WorldPosition = easerPlayerOne.CurrentValue;

            if (easerPlayerOne.IsFinished)
            {
                easerPlayerOne.Reverse();
                easerPlayerOne.Start();
            }

            GameplayState.PlayerTwo.WorldPosition = easerPlayerTwo.CurrentValue;

            if(easerPlayerTwo.IsFinished)
            {
                easerPlayerTwo.Reverse();
                easerPlayerTwo.Start();
            }


            if (InputManager.OnButtonPressed(Buttons.DPadUp, PlayerIndex.One) ||
                InputManager.OnKeyPressed(Keys.Up))
            {
                selectionSprite.SetAnimation(selectionSprite.CurrentAnimation == "CONTINUE" ?
                                        "RETURN_TO_TITLE" : "CONTINUE");
            }
            else if (InputManager.OnButtonPressed(Buttons.DPadDown, PlayerIndex.One) ||
                     InputManager.OnKeyPressed(Keys.Down))
            {
                selectionSprite.SetAnimation(selectionSprite.CurrentAnimation == "CONTINUE" ?
                                        "RETURN_TO_TITLE" : "CONTINUE");
            }

            if (InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One) ||
                InputManager.OnKeyPressed(Keys.Enter))
            {
                if (selectionSprite.CurrentAnimation == "CONTINUE")
                {
                    sceneManager.StartNewGame();
                }
                else
                {
                    sceneManager.GoToMainMenu();
                }
            }

            if (InputManager.OnKeyPressed(Keys.L))
            {
                heartSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/gameOverHeart.anm.txt");
            }
        }
    }
}
