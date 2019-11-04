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
using EVCMonoGame.src.animation;
using EVCMonoGame.src.utility;

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
        private AnimatedSprite playerPortrait;
        private Healthbar playerHealthbar;
        private float playerSpeed;
        private bool isAttacking;
        private float runThreshold;

        private Keys[] keyboardControls;

        private Vector2 movementVector;
        private Vector2 previousMovementVector;

        private Orientation playerOrientation;

        private bool flinching;

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
            get { return 50; }
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
        public Player(Vector2 position, Keys[] controls, float playerSpeed)
        {
            isAttacking = false;
            runThreshold = 0.65f;

            playerSprite = new AnimatedSprite(position, 5.0f);
            playerSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/sora.anm.txt");
            playerSprite.SetAnimation("IDLE_RIGHT");
            playerOrientation = Orientation.RIGHT;

            playerPortrait = new AnimatedSprite(Vector2.Zero, 4.0f);
            playerPortrait.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/sora_portrait.anm.txt");
            playerPortrait.SetAnimation("TALKING_HAPPY_RIGHT");

            playerHealthbar = new Healthbar(9999, 9999, new Vector2(300, 100), new Vector2(100, 10));
            this.playerSpeed = playerSpeed;

            // Der Parameter controls ist nicht final. Nur, um mehrere Player Instanzen anders steuern zu können.
            if (controls.Length != 4)
            {
                throw new ArgumentException("Nur 4 Bewegungstasten");
            }
            this.keyboardControls = controls;

            movementVector = Vector2.Zero;
            previousMovementVector = movementVector;
            DoesUpdateMovement = true;
            flinching = false;
        }
        #endregion
        #region IDrawable
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerHealthbar.Draw(gameTime, spriteBatch);
            playerSprite.Draw(gameTime, spriteBatch);
            playerPortrait.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            playerSprite.LoadContent(content);
            playerHealthbar.LoadContent(content);
            playerPortrait.LoadContent(content);
        }
        #endregion
        #region Updateable
        public override void Update(GameTime gameTime)
        {
            // flinch = zurückweichen
            if (flinching)
            {
                if (playerSprite.AnimationFinished)
                {
                    flinching = false;
                    switch(playerOrientation)
                    {
                        case Orientation.LEFT: playerSprite.SetAnimation("IDLE_LEFT"); break;
                        case Orientation.UP_LEFT: playerSprite.SetAnimation("IDLE_UP_LEFT"); break;
                        case Orientation.UP: playerSprite.SetAnimation("IDLE_UP"); break;
                        case Orientation.UP_RIGHT: playerSprite.SetAnimation("IDLE_UP_RIGHT"); break;
                        case Orientation.RIGHT: playerSprite.SetAnimation("IDLE_RIGHT"); break;
                        case Orientation.DOWN_RIGHT: playerSprite.SetAnimation("IDLE_DOWN_RIGHT"); break;
                        case Orientation.DOWN: playerSprite.SetAnimation("IDLE_DOWN"); break;
                        case Orientation.DOWN_LEFT: playerSprite.SetAnimation("IDLE_DOWN_LEFT"); break;
                    }
                }
            }
            else
            {
                UpdateMovement();
                UpdateAttacks();
            }

            playerHealthbar.Position = playerSprite.Position - new Vector2(0, playerHealthbar.Size.Y);

            playerSprite.Update(gameTime);
            playerPortrait.Update(gameTime);
        }


        public void UpdateAttacks()
        {

            if (playerSprite.AnimationFinished) HasActiveAttackBounds = false;

            String nextAttackAnimation = "UNKNOWN";

            if (InputManager.OnButtonPressed(Buttons.X)
                || InputManager.OnKeyPressed(Keys.A))
            {
                HasActiveAttackBounds = true;

                switch(playerOrientation)
                {
                    case Orientation.LEFT:       nextAttackAnimation = "ATTACK_STD_COMBO_LEFT_0"; break;
                    case Orientation.UP_LEFT:    nextAttackAnimation = "ATTACK_UP_LEFT"; break;
                    case Orientation.UP:         nextAttackAnimation = "ATTACK_UP"; break;
                    case Orientation.UP_RIGHT:   nextAttackAnimation = "ATTACK_UP_RIGHT"; break;
                    case Orientation.RIGHT:      nextAttackAnimation = "ATTACK_STD_COMBO_RIGHT_0"; break;
                    case Orientation.DOWN_RIGHT: nextAttackAnimation = "ATTACK_DOWN_RIGHT"; break;
                    case Orientation.DOWN:       nextAttackAnimation = "ATTACK_DOWN"; break;
                    case Orientation.DOWN_LEFT:  nextAttackAnimation = "ATTACK_DOWN_LEFT"; break;
                }

                playerSprite.SetAnimation(nextAttackAnimation);
            }
            else if (InputManager.OnButtonPressed(Buttons.Y))
            {
                HasActiveAttackBounds = true;

                switch (playerOrientation)
                {
                    case Orientation.LEFT:       nextAttackAnimation = "ATTACK_STD_COMBO_LEFT_1"; break;
                    case Orientation.UP_LEFT:    nextAttackAnimation = "ATTACK_UP_LEFT"; break;
                    case Orientation.UP:         nextAttackAnimation = "ATTACK_UP"; break;
                    case Orientation.UP_RIGHT:   nextAttackAnimation = "ATTACK_UP_RIGHT"; break;
                    case Orientation.RIGHT:      nextAttackAnimation = "ATTACK_STD_COMBO_RIGHT_1"; break;
                    case Orientation.DOWN_RIGHT: nextAttackAnimation = "ATTACK_DOWN_RIGHT"; break;
                    case Orientation.DOWN:       nextAttackAnimation = "ATTACK_DOWN"; break;
                    case Orientation.DOWN_LEFT:  nextAttackAnimation = "ATTACK_DOWN_LEFT"; break;
                }

                playerSprite.SetAnimation(nextAttackAnimation);
            }
            else if (InputManager.OnButtonPressed(Buttons.B))
            {
                HasActiveAttackBounds = true;

                switch (playerOrientation)
                {
                    case Orientation.LEFT:       nextAttackAnimation = "ATTACK_STD_COMBO_LEFT_2"; break;
                    case Orientation.UP_LEFT:    nextAttackAnimation = "ATTACK_UP_LEFT"; break;
                    case Orientation.UP:         nextAttackAnimation = "ATTACK_UP"; break;
                    case Orientation.UP_RIGHT:   nextAttackAnimation = "ATTACK_UP_RIGHT"; break;
                    case Orientation.RIGHT:      nextAttackAnimation = "ATTACK_STD_COMBO_RIGHT_2"; break;
                    case Orientation.DOWN_RIGHT: nextAttackAnimation = "ATTACK_DOWN_RIGHT"; break;
                    case Orientation.DOWN:       nextAttackAnimation = "ATTACK_DOWN"; break;
                    case Orientation.DOWN_LEFT:  nextAttackAnimation = "ATTACK_DOWN_LEFT"; break;
                }

                playerSprite.SetAnimation(nextAttackAnimation);
            }
        }

        public void UpdateMovement()
        {
            // TODO: Check for collision after moving.
            // TODO: Tidy up this method.

            if (!DoesUpdateMovement)
                return;

            Vector2 directionVector = Vector2.Zero;
            previousMovementVector = movementVector;

            // Differentiate between Keyboard and GamePad controls.
            if (InputManager.InputByKeyboard)
            {
                if (InputManager.IsKeyPressed(keyboardControls[0])) directionVector.Y -= 100; //up
                if (InputManager.IsKeyPressed(keyboardControls[2])) directionVector.X += 100; //right
                if (InputManager.IsKeyPressed(keyboardControls[1])) directionVector.Y += 100; //down
                if (InputManager.IsKeyPressed(keyboardControls[3])) directionVector.X -= 100; //left

                movementVector = Utility.scaleVectorTo(directionVector, playerSpeed);
            }
            else
            {
                GamePadThumbSticks currentThumbSticks = InputManager.CurrentThumbSticks();

                directionVector.X = currentThumbSticks.Left.X;
                directionVector.Y = currentThumbSticks.Left.Y * -1;

                movementVector = directionVector * (playerSpeed * (1 + InputManager.CurrentTriggers().Right));
                Console.WriteLine(movementVector);
            }

            playerSprite.Position += movementVector;

            // Has a movement happened ?
            if (movementVector != previousMovementVector)
            {
                // Has the moving stopped ?
                if (movementVector == Vector2.Zero)
                {
                    String currentAnimation = playerSprite.CurrentAnimation;

                    if (currentAnimation == "WALK_DOWN_LEFT" || currentAnimation == "RUN_DOWN_LEFT"
                        || currentAnimation == "IDLE_DOWN_LEFT")
                    {
                        playerSprite.SetAnimation("IDLE_DOWN_LEFT");
                        playerOrientation = Orientation.DOWN_LEFT;
                    }
                    else if (currentAnimation == "RUN_DOWN" || currentAnimation == "IDLE_DOWN")
                    {
                        playerSprite.SetAnimation("IDLE_DOWN");
                        playerOrientation = Orientation.DOWN;
                    }
                    else if (currentAnimation == "WALK_DOWN_RIGHT" || currentAnimation == "RUN_DOWN_RIGHT"
                        || currentAnimation == "IDLE_DOWN_RIGHT")
                    {
                        playerSprite.SetAnimation("IDLE_DOWN_RIGHT");
                        playerOrientation = Orientation.DOWN_RIGHT;
                    }
                    else if (currentAnimation == "WALK_UP_LEFT" || currentAnimation == "RUN_UP_LEFT"
                        || currentAnimation == "IDLE_UP_LEFT")
                    {
                        playerSprite.SetAnimation("IDLE_UP_LEFT");
                        playerOrientation = Orientation.UP_LEFT;
                    }
                    else if (currentAnimation == "RUN_UP" || currentAnimation == "IDLE_UP")
                    {
                        playerSprite.SetAnimation("IDLE_UP");
                        playerOrientation = Orientation.UP;
                    }
                    else if (currentAnimation == "WALK_UP_RIGHT" || currentAnimation == "RUN_UP_RIGHT"
                        || currentAnimation == "IDLE_UP_RIGHT")
                    {
                        playerSprite.SetAnimation("IDLE_UP_RIGHT");
                        playerOrientation = Orientation.UP_RIGHT;
                    }
                    else if (currentAnimation == "RUN_LEFT" || currentAnimation == "IDLE_LEFT")
                    {
                        playerSprite.SetAnimation("IDLE_LEFT");
                        playerOrientation = Orientation.LEFT;
                    }
                    else if (currentAnimation == "RUN_RIGHT" || currentAnimation == "IDLE_RIGHT")
                    {
                        playerSprite.SetAnimation("IDLE_RIGHT");
                        playerOrientation = Orientation.RIGHT;
                    }
                    else
                    {
                        playerSprite.SetAnimation("IDLE_UP");
                        playerOrientation = Orientation.UP;
                    }
                }
                else
                {
                    float mvAngle = Utility.getAngleOfVectorInDegrees(movementVector);
                    float directionVectorLength = directionVector.Length();

                    if (mvAngle > (-22.5) && mvAngle <= (22.5))
                    {
                        playerSprite.SetAnimation("RUN_RIGHT");
                        playerOrientation = Orientation.RIGHT;
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
                        playerOrientation = Orientation.UP_RIGHT;
                    }
                    if (mvAngle > (77.5) && mvAngle <= (112.5))
                    {
                        playerSprite.SetAnimation("RUN_UP");
                        playerOrientation = Orientation.UP;
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
                        playerOrientation = Orientation.UP_LEFT;
                    }
                    if ((mvAngle > (157.5) && mvAngle <= (180)) || (mvAngle >= (-180) && mvAngle <= (-157.5)))
                    {
                        playerSprite.SetAnimation("RUN_LEFT");
                        playerOrientation = Orientation.LEFT;
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
                        playerOrientation = Orientation.DOWN_LEFT;
                    }
                    if (mvAngle > (-112.5) && mvAngle <= (-77.5))
                    {
                        playerSprite.SetAnimation("RUN_DOWN");
                        playerOrientation = Orientation.DOWN;
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
                        playerOrientation = Orientation.DOWN_RIGHT;
                    }
                }
            }
        }

        #endregion
        #region CombatCollidable
        public void ReceiveDamage(int amount)
        {
            playerHealthbar.CurrentHp -= amount;
            // Update Character Hp as well.
        }

        public void OnCombatCollision(CombatCollidable attacker)
        {
            switch(playerOrientation)
            {
                case Orientation.DOWN: playerSprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.DOWN_LEFT: playerSprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.LEFT: playerSprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.UP_LEFT: playerSprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.UP: playerSprite.SetAnimation("FLINCH_LEFT"); break;

                case Orientation.UP_RIGHT: playerSprite.SetAnimation("FLINCH_RIGHT"); break;
                case Orientation.RIGHT: playerSprite.SetAnimation("FLINCH_RIGHT"); break;
                case Orientation.DOWN_RIGHT: playerSprite.SetAnimation("FLINCH_RIGHT"); break;
            }
            flinching = true;
        }
        #endregion
    }
}
