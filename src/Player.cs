using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src
{
    public class Player : Updateable, scenes.IDrawable
    {
        private AnimatedSprite playerSprite;
        private Healthbar playerHealthbar;
        private float playerSpeed;
        private bool isAttacking;
        private float runThreshold;

        public AnimatedSprite Sprite
        {
            get { return playerSprite; }
        }

        public Healthbar Healthbar
        {
            get { return playerHealthbar; }
        }

        private Keys[] controls;

        public Player(Vector2 position, Keys[] controls)
        {
            isAttacking = false;
            runThreshold = 0.65f;

            playerSprite = new AnimatedSprite(position, 5.0f);
            playerSprite.LoadFromFile("Content/rsrc/spritesheets/configFiles/sora.txt");
            playerSprite.SetAnimation("IDLE_UP");

            playerHealthbar = new Healthbar(2345, 1234, new Vector2(300, 100), new Vector2(100, 10));
            playerSpeed = 8;

            // Der Parameter controls ist nicht final. Nur, um mehrere Player Instanzen anders steuern zu können.
            if (controls.Length != 4)
            {
                throw new ArgumentException("Nur 4 Bewegungstasten");
            }
            this.controls = controls;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerHealthbar.Draw(gameTime, spriteBatch);
            playerSprite.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            playerSprite.LoadContent(content);
            playerHealthbar.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isAttacking)
            {
                // UpdateMovement();
            }
            UpdateAttacks();

            playerHealthbar.Position = playerSprite.Position - new Vector2(0, playerHealthbar.Size.Y);


            playerSprite.Update(gameTime);
        }

        public void UpdateAttacks()
        {
            //if (InputManager.OnButtonPressed(Buttons.X)
            //    || InputManager.OnKeyPressed(Keys.A))
            //{
            //    isAttacking = true;

            //    //if (playerSprite.CurrentAnimation == "IDLE_DOWN")
            //    //{
            //    //    playerSprite.SetAnimation("ATTACK_DOWN");
            //    //}
            //    playerSprite.SetAnimation("ATTACK_DOWN");
            //}

            //if (playerSprite.AnimationFinished)
            //{
            //    isAttacking = false;
            //    return;
            //}

            String currentAnim = playerSprite.CurrentAnimation;
            if (playerSprite.AnimationFinished && currentAnim.Contains("ATTACK"))
            {
                if (playerSprite.CurrentAnimation == "ATTACK_STD_COMBO_0")
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_1");
                }
                else if (playerSprite.CurrentAnimation == "ATTACK_STD_COMBO_1")
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_2");
                }
                else
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_0");
                }
            }

            if (InputManager.OnButtonPressed(Buttons.X))
            {
                playerSprite.SetAnimation("ATTACK_STD_COMBO_0");
            }
        }

        public void UpdateMovement()
        {
            Vector2 currentPosition = playerSprite.Position;
            Vector2 directionVector = new Vector2(0, 0);
            Vector2 movementVector = new Vector2(0, 0);
            switch (InputManager.InputByKeyboard)
            {
                case true:
                    if (InputManager.IsKeyPressed(controls[0])) directionVector.Y -= 100; //up
                    if (InputManager.IsKeyPressed(controls[2])) directionVector.X += 100; //right
                    if (InputManager.IsKeyPressed(controls[1])) directionVector.Y += 100; //down
                    if (InputManager.IsKeyPressed(controls[3])) directionVector.X -= 100; //left
                    movementVector = Utility.scaleVectorTo(directionVector, playerSpeed);
                    break;
                case false:
                    directionVector.X = InputManager.CurrentThumbSticks().Left.X;
                    directionVector.Y = (-1) * (InputManager.CurrentThumbSticks().Left.Y);
                    movementVector = directionVector * (playerSpeed * (1 + InputManager.CurrentTriggers().Right));
                    break;
            }

            playerSprite.Position += movementVector;
            float mvAngle = Utility.getAngleOfVectorInDegrees(movementVector);
            float directionVectorLength = directionVector.Length();
            if (movementVector == Vector2.Zero)
            {
                String currentAnimation = playerSprite.CurrentAnimation;

                if (currentAnimation == "WALK_DOWN_LEFT" || currentAnimation == "RUN_DOWN_LEFT"
                    || currentAnimation == "IDLE_DOWN_LEFT")
                {
                    playerSprite.SetAnimation("IDLE_DOWN_LEFT");
                }
                else if (currentAnimation == "RUN_DOWN" || currentAnimation == "IDLE_DOWN")
                {
                    playerSprite.SetAnimation("IDLE_DOWN");
                }
                else if (currentAnimation == "WALK_DOWN_RIGHT" || currentAnimation == "RUN_DOWN_RIGHT"
                    || currentAnimation == "IDLE_DOWN_RIGHT")
                {
                    playerSprite.SetAnimation("IDLE_DOWN_RIGHT");
                }
                else if (currentAnimation == "WALK_UP_LEFT" || currentAnimation == "RUN_UP_LEFT"
                    || currentAnimation == "IDLE_UP_LEFT")
                {
                    playerSprite.SetAnimation("IDLE_UP_LEFT");
                }
                else if (currentAnimation == "RUN_UP" || currentAnimation == "IDLE_UP")
                {
                    playerSprite.SetAnimation("IDLE_UP");
                }
                else if (currentAnimation == "WALK_UP_RIGHT" || currentAnimation == "RUN_UP_RIGHT"
                    || currentAnimation == "IDLE_UP_RIGHT")
                {
                    playerSprite.SetAnimation("IDLE_UP_RIGHT");
                }
                else if (currentAnimation == "RUN_LEFT" || currentAnimation == "IDLE_LEFT")
                {
                    playerSprite.SetAnimation("IDLE_LEFT");
                }
                else if (currentAnimation == "RUN_RIGHT" || currentAnimation == "IDLE_RIGHT")
                {
                    playerSprite.SetAnimation("IDLE_RIGHT");
                }
                else
                {
                    playerSprite.SetAnimation("IDLE_UP");
                }
            }
            else
            {
                if (mvAngle > (-22.5) && mvAngle <= (22.5))
                {
                    //right
                    playerSprite.SetAnimation("RUN_RIGHT");
                }
                if (mvAngle > (22.5) && mvAngle <= (77.5))
                {
                    if (directionVectorLength <= runThreshold)
                    {
                        playerSprite.SetAnimation("WALK_UP_RIGHT");
                    }
                    else
                    {
                        playerSprite.SetAnimation("RUN_UP_RIGHT");
                    }
                }
                if (mvAngle > (77.5) && mvAngle <= (112.5))
                {
                    //up

                    playerSprite.SetAnimation("RUN_UP");
                }
                if (mvAngle > (112.5) && mvAngle <= (157.5))
                {
                    if (directionVectorLength <= runThreshold)
                    {
                        playerSprite.SetAnimation("WALK_UP_LEFT");
                    }
                    else
                    {
                        //up-left
                        playerSprite.SetAnimation("RUN_UP_LEFT");
                        // playerSprite.SetAnimation("WALK_UP_LEFT");
                    }
                }
                if ((mvAngle > (157.5) && mvAngle <= (180)) || (mvAngle >= (-180) && mvAngle <= (-157.5)))
                {
                    //left
                    playerSprite.SetAnimation("RUN_LEFT");
                }
                if (mvAngle > (-157.5) && mvAngle <= (-112.5))
                {
                    if (directionVectorLength <= runThreshold)
                    {
                        playerSprite.SetAnimation("WALK_DOWN_LEFT");
                    }
                    else
                    {
                        playerSprite.SetAnimation("RUN_DOWN_LEFT");
                    }
                }
                if (mvAngle > (-112.5) && mvAngle <= (-77.5))
                {
                    //down
                    playerSprite.SetAnimation("RUN_DOWN");
                }
                if (mvAngle > (-77.5) && mvAngle <= (-22.5))
                {
                    if (directionVectorLength <= runThreshold)
                    {
                        playerSprite.SetAnimation("WALK_DOWN_RIGHT");
                    }
                    else
                    {
                        playerSprite.SetAnimation("RUN_DOWN_RIGHT");
                    }
                }
            }
        }
    }
}
