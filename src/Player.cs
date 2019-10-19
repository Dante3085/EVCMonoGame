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
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src
{
    public enum Orientation
    {
        LEFT,
        UP_LEFT,
        UP,
        UP_RIGHT,
        RIGHT,
        DOWN_RIGHT,
        DOWN,
        DOWN_LEFT
    }

    public class Player : Updateable, scenes.IDrawable, CombatCollidable
    {
        #region Fields
        private AnimatedSprite playerSprite;
        private Healthbar playerHealthbar;
        private float playerSpeed;
        private bool isAttacking;
        private float runThreshold;

        private Keys[] controls;

        private Vector2 movementVector;
        private Vector2 previousMovementVector;

        private Orientation orientation;

        #endregion
        #region Properties
        public AnimatedSprite Sprite
        {
            get { return playerSprite; }
        }

        public Healthbar Healthbar
        {
            get { return playerHealthbar; }
        }

        public Rectangle HurtBounds
        {
            get { return playerSprite.CurrentHurtBounds; }
        }

        public Rectangle AttackBounds
        {
            get { return playerSprite.CurrentAttackBounds; }
        }

        public bool HasActiveAttackBounds
        {
            get; private set;
        }

        public bool IsAlive
        {
            get { return playerHealthbar.CurrentHp > 0; }
        }

        public int CurrentDamage
        {
            get { return 10; }
        }

        public Rectangle Bounds
        {
            get { return playerSprite.Bounds; }
        }

        /// <summary>
        /// ONLY FOR DEBUGGING PURPOSES. REMOVE LATER.
        /// </summary>
        public bool DoesUpdateMovement
        {
            get; set;
        }
        #endregion

        #region Constructors
        public Player(Vector2 position, Keys[] controls)
        {
            isAttacking = false;
            runThreshold = 0.65f;

            playerSprite = new AnimatedSprite(position, 5.0f);
            playerSprite.LoadFromFile("Content/rsrc/spritesheets/configFiles/sora.txt");
            playerSprite.SetAnimation("IDLE_DOWN");

            orientation = Orientation.DOWN;

            playerHealthbar = new Healthbar(9999, 9999, new Vector2(300, 100), new Vector2(100, 10));
            playerSpeed = 8;

            // Der Parameter controls ist nicht final. Nur, um mehrere Player Instanzen anders steuern zu können.
            if (controls.Length != 4)
            {
                throw new ArgumentException("Nur 4 Bewegungstasten");
            }
            this.controls = controls;

            movementVector = Vector2.Zero;
            previousMovementVector = movementVector;
            DoesUpdateMovement = true;
        }
        #endregion
        #region IDrawable
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
        #endregion
        #region Updateable
        public override void Update(GameTime gameTime)
        {
            UpdateMovement();
            UpdateAttacks();

            playerHealthbar.Position = playerSprite.Position - new Vector2(0, playerHealthbar.Size.Y);


            playerSprite.Update(gameTime);
        }

        public void UpdateAttacks()
        {

            if (playerSprite.AnimationFinished) HasActiveAttackBounds = false;

            if (InputManager.OnButtonPressed(Buttons.X)
                || InputManager.OnKeyPressed(Keys.A))
            {
                HasActiveAttackBounds = true;

                if (orientation == Orientation.LEFT)
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_LEFT_0");
                }
                else if (orientation == Orientation.RIGHT)
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_RIGHT_0");
                }
            }
            else if (InputManager.OnButtonPressed(Buttons.Y))
            {
                HasActiveAttackBounds = true;

                if (orientation == Orientation.LEFT)
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_LEFT_1");
                }
                else if (orientation == Orientation.RIGHT)
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_RIGHT_1");
                }
            }
            else if (InputManager.OnButtonPressed(Buttons.B))
            {
                HasActiveAttackBounds = true;

                if (orientation == Orientation.LEFT)
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_LEFT_2");
                }
                else if (orientation == Orientation.RIGHT)
                {
                    playerSprite.SetAnimation("ATTACK_STD_COMBO_RIGHT_2");
                }
            }
        }

        public void UpdateMovement()
        {
            if (!DoesUpdateMovement)
                return;

            Vector2 currentPosition = playerSprite.Position;
            Vector2 directionVector = new Vector2(0, 0);
            previousMovementVector = movementVector;

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

            if (movementVector != previousMovementVector)
            {
                if (movementVector == Vector2.Zero)
                {
                    String currentAnimation = playerSprite.CurrentAnimation;

                    if (currentAnimation == "WALK_DOWN_LEFT" || currentAnimation == "RUN_DOWN_LEFT"
                        || currentAnimation == "IDLE_DOWN_LEFT")
                    {
                        playerSprite.SetAnimation("IDLE_DOWN_LEFT");
                        orientation = Orientation.DOWN_LEFT;
                    }
                    else if (currentAnimation == "RUN_DOWN" || currentAnimation == "IDLE_DOWN")
                    {
                        playerSprite.SetAnimation("IDLE_DOWN");
                        orientation = Orientation.DOWN;
                    }
                    else if (currentAnimation == "WALK_DOWN_RIGHT" || currentAnimation == "RUN_DOWN_RIGHT"
                        || currentAnimation == "IDLE_DOWN_RIGHT")
                    {
                        playerSprite.SetAnimation("IDLE_DOWN_RIGHT");
                        orientation = Orientation.DOWN_RIGHT;
                    }
                    else if (currentAnimation == "WALK_UP_LEFT" || currentAnimation == "RUN_UP_LEFT"
                        || currentAnimation == "IDLE_UP_LEFT")
                    {
                        playerSprite.SetAnimation("IDLE_UP_LEFT");
                        orientation = Orientation.UP_LEFT;
                    }
                    else if (currentAnimation == "RUN_UP" || currentAnimation == "IDLE_UP")
                    {
                        playerSprite.SetAnimation("IDLE_UP");
                        orientation = Orientation.UP;
                    }
                    else if (currentAnimation == "WALK_UP_RIGHT" || currentAnimation == "RUN_UP_RIGHT"
                        || currentAnimation == "IDLE_UP_RIGHT")
                    {
                        playerSprite.SetAnimation("IDLE_UP_RIGHT");
                        orientation = Orientation.UP_RIGHT;
                    }
                    else if (currentAnimation == "RUN_LEFT" || currentAnimation == "IDLE_LEFT")
                    {
                        playerSprite.SetAnimation("IDLE_LEFT");
                        orientation = Orientation.LEFT;
                    }
                    else if (currentAnimation == "RUN_RIGHT" || currentAnimation == "IDLE_RIGHT")
                    {
                        playerSprite.SetAnimation("IDLE_RIGHT");
                        orientation = Orientation.RIGHT;
                    }
                    else
                    {
                        playerSprite.SetAnimation("IDLE_UP");
                        orientation = Orientation.UP;
                    }
                }
                else
                {
                    if (mvAngle > (-22.5) && mvAngle <= (22.5))
                    {
                        playerSprite.SetAnimation("RUN_RIGHT");
                        orientation = Orientation.RIGHT;
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
                        orientation = Orientation.UP_RIGHT;
                    }
                    if (mvAngle > (77.5) && mvAngle <= (112.5))
                    {
                        playerSprite.SetAnimation("RUN_UP");
                        orientation = Orientation.UP;
                    }
                    if (mvAngle > (112.5) && mvAngle <= (157.5))
                    {
                        if (directionVectorLength <= runThreshold)
                        {
                            playerSprite.SetAnimation("WALK_UP_LEFT");
                        }
                        else
                        {
                            playerSprite.SetAnimation("RUN_UP_LEFT");
                        }
                        orientation = Orientation.UP_LEFT;
                    }
                    if ((mvAngle > (157.5) && mvAngle <= (180)) || (mvAngle >= (-180) && mvAngle <= (-157.5)))
                    {
                        playerSprite.SetAnimation("RUN_LEFT");
                        orientation = Orientation.LEFT;
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
                        orientation = Orientation.DOWN_LEFT;
                    }
                    if (mvAngle > (-112.5) && mvAngle <= (-77.5))
                    {
                        playerSprite.SetAnimation("RUN_DOWN");
                        orientation = Orientation.DOWN;
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
                        orientation = Orientation.DOWN_RIGHT;
                    }
                }
            }
        }

        #endregion

        public void ReceiveDamage(int amount)
        {
            playerHealthbar.CurrentHp -= amount;
            // Update Character Hp as well.
        }
    }
}
