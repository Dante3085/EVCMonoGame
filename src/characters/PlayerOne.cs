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
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.characters
{
    public class PlayerOne : Player
    {
        #region Fields

        private AnimatedSprite playerPortrait;

        private bool isAttacking;
        private float runThreshold;

        private Keys[] keyboardControls;

        private Vector2 movementVector;
        private Vector2 previousMovementVector;

        private Orientation playerOrientation;

        private bool flinching;

        #endregion
        #region Properties

        /// <summary>
        /// ONLY FOR DEBUGGING PURPOSES. REMOVE LATER.
        /// </summary>
        public bool DoesUpdateMovement
        {
            get; set;
        }

        public bool BlockInput
        {
            get; set;
        } = false;

        #endregion

        #region Constructors
        public PlayerOne(Vector2 position, Keys[] controls)
             : base
            (
                  name: "Sora",
                  maxHp: 800,
                  currentHp: 800,
                  maxMp: 30,
                  currentMp: 30,
                  strength: 5,
                  defense: 3,
                  intelligence: 5,
                  agility: 4,
                  movementSpeed: 7,
                  position: position
            )
        {

            isAttacking = false;
            runThreshold = 0.65f;

			movementSpeed = 7.5f;

			CollisionBox = new Rectangle(position.ToPoint(), new Point(140, 230));

            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/sora.anm.txt");
            sprite.SetAnimation("RUN_RIGHT");
            playerOrientation = Orientation.RIGHT;

            playerPortrait = new AnimatedSprite(Vector2.Zero, 4.0f);
            playerPortrait.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/sora_portrait.anm.txt");
            playerPortrait.SetAnimation("TALKING_HAPPY_RIGHT");

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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // playerPortrait.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            // playerPortrait.LoadContent(content);
        }
        #endregion
        #region Updateable
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // flinch = zurückweichen
            if (flinching)
            {
                if (sprite.AnimationFinished)
                {
                    flinching = false;
                    switch(playerOrientation)
                    {
                        case Orientation.LEFT: sprite.SetAnimation("IDLE_LEFT"); break;
                        case Orientation.UP_LEFT: sprite.SetAnimation("IDLE_UP_LEFT"); break;
                        case Orientation.UP: sprite.SetAnimation("IDLE_UP"); break;
                        case Orientation.UP_RIGHT: sprite.SetAnimation("IDLE_UP_RIGHT"); break;
                        case Orientation.RIGHT: sprite.SetAnimation("IDLE_RIGHT"); break;
                        case Orientation.DOWN_RIGHT: sprite.SetAnimation("IDLE_DOWN_RIGHT"); break;
                        case Orientation.DOWN: sprite.SetAnimation("IDLE_DOWN"); break;
                        case Orientation.DOWN_LEFT: sprite.SetAnimation("IDLE_DOWN_LEFT"); break;
                    }
                }
            }
            else
            {
                if (!BlockInput)
                {
                    UpdateMovement();
                    UpdateAttacks();
                }
            }

            playerPortrait.Update(gameTime);
        }

        public void UpdateAttacks()
        {

            if (sprite.AnimationFinished) HasActiveAttackBounds = false;

            String nextAttackAnimation = "UNKNOWN";

            if (InputManager.OnButtonPressed(Buttons.X, PlayerIndex.One)
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

                sprite.SetAnimation(nextAttackAnimation);
            }
            else if (InputManager.OnButtonPressed(Buttons.Y, PlayerIndex.One))
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

                sprite.SetAnimation(nextAttackAnimation);
            }
            else if (InputManager.OnButtonPressed(Buttons.B, PlayerIndex.One))
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

                sprite.SetAnimation(nextAttackAnimation);
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
            PreviousWorldPosition = WorldPosition;

            // Differentiate between Keyboard and GamePad controls.
            if (InputManager.InputByKeyboard)
            {
                if (InputManager.IsKeyPressed(keyboardControls[0])) directionVector.Y -= 100; //up
                if (InputManager.IsKeyPressed(keyboardControls[2])) directionVector.X += 100; //right
                if (InputManager.IsKeyPressed(keyboardControls[1])) directionVector.Y += 100; //down
                if (InputManager.IsKeyPressed(keyboardControls[3])) directionVector.X -= 100; //left

                movementVector = Utility.ScaleVectorTo(directionVector, movementSpeed);
                if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                {
                    movementVector *= 2;
                }
            }
            else
            {
                GamePadThumbSticks currentThumbSticks = InputManager.CurrentThumbSticks(PlayerIndex.One);

                directionVector.X = currentThumbSticks.Left.X;
                directionVector.Y = currentThumbSticks.Left.Y * -1;

                movementVector = directionVector * 
                                 (movementSpeed  * (1 + InputManager.CurrentTriggers(PlayerIndex.One).Right));
            }

            WorldPosition += movementVector;

            CollisionManager.IsCollisionAfterMove(this, true, true);

            sprite.Position = WorldPosition;

            // Has a movement happened ?
            if (movementVector != previousMovementVector)
            {
                // Has the moving stopped ?
                if (movementVector == Vector2.Zero)
                {
                    String currentAnimation = sprite.CurrentAnimation;

                    if (currentAnimation == "WALK_DOWN_LEFT" || currentAnimation == "RUN_DOWN_LEFT"
                        || currentAnimation == "IDLE_DOWN_LEFT")
                    {
                        sprite.SetAnimation("IDLE_DOWN_LEFT");
                        playerOrientation = Orientation.DOWN_LEFT;
                    }
                    else if (currentAnimation == "RUN_DOWN" || currentAnimation == "IDLE_DOWN")
                    {
                        sprite.SetAnimation("IDLE_DOWN");
                        playerOrientation = Orientation.DOWN;
                    }
                    else if (currentAnimation == "WALK_DOWN_RIGHT" || currentAnimation == "RUN_DOWN_RIGHT"
                        || currentAnimation == "IDLE_DOWN_RIGHT")
                    {
                        sprite.SetAnimation("IDLE_DOWN_RIGHT");
                        playerOrientation = Orientation.DOWN_RIGHT;
                    }
                    else if (currentAnimation == "WALK_UP_LEFT" || currentAnimation == "RUN_UP_LEFT"
                        || currentAnimation == "IDLE_UP_LEFT")
                    {
                        sprite.SetAnimation("IDLE_UP_LEFT");
                        playerOrientation = Orientation.UP_LEFT;
                    }
                    else if (currentAnimation == "RUN_UP" || currentAnimation == "IDLE_UP")
                    {
                        sprite.SetAnimation("IDLE_UP");
                        playerOrientation = Orientation.UP;
                    }
                    else if (currentAnimation == "WALK_UP_RIGHT" || currentAnimation == "RUN_UP_RIGHT"
                        || currentAnimation == "IDLE_UP_RIGHT")
                    {
                        sprite.SetAnimation("IDLE_UP_RIGHT");
                        playerOrientation = Orientation.UP_RIGHT;
                    }
                    else if (currentAnimation == "RUN_LEFT" || currentAnimation == "IDLE_LEFT")
                    {
                        sprite.SetAnimation("IDLE_LEFT");
                        playerOrientation = Orientation.LEFT;
                    }
                    else if (currentAnimation == "RUN_RIGHT" || currentAnimation == "IDLE_RIGHT")
                    {
                        sprite.SetAnimation("IDLE_RIGHT");
                        playerOrientation = Orientation.RIGHT;
                    }
                    else
                    {
                        sprite.SetAnimation("IDLE_UP");
                        playerOrientation = Orientation.UP;
                    }
                }
                else
                {
                    float mvAngle = Utility.GetAngleOfVectorInDegrees(movementVector);
                    float directionVectorLength = directionVector.Length();

                    if (mvAngle > (-22.5) && mvAngle <= (22.5))
                    {
                        sprite.SetAnimation("RUN_RIGHT");
                        playerOrientation = Orientation.RIGHT;
                    }
                    if (mvAngle > (22.5) && mvAngle <= (77.5))
                    {
                        if (directionVectorLength <= runThreshold)
                        {
                            sprite.SetAnimation("WALK_UP_RIGHT");
                        }
                        else
                        {
                            sprite.SetAnimation("RUN_UP_RIGHT");
                        }
                        playerOrientation = Orientation.UP_RIGHT;
                    }
                    if (mvAngle > (77.5) && mvAngle <= (112.5))
                    {
                        sprite.SetAnimation("RUN_UP");
                        playerOrientation = Orientation.UP;
                    }
                    if (mvAngle > (112.5) && mvAngle <= (157.5))
                    {
                        if (directionVectorLength <= runThreshold)
                        {
                            sprite.SetAnimation("WALK_UP_LEFT");
                        }
                        else
                        {
                            sprite.SetAnimation("RUN_UP_LEFT");
                        }
                        playerOrientation = Orientation.UP_LEFT;
                    }
                    if ((mvAngle > (157.5) && mvAngle <= (180)) || (mvAngle >= (-180) && mvAngle <= (-157.5)))
                    {
                        sprite.SetAnimation("RUN_LEFT");
                        playerOrientation = Orientation.LEFT;
                    }
                    if (mvAngle > (-157.5) && mvAngle <= (-112.5))
                    {
                        if (directionVectorLength <= runThreshold)
                        {
                            sprite.SetAnimation("WALK_DOWN_LEFT");
                        }
                        else
                        {
                            sprite.SetAnimation("RUN_DOWN_LEFT");
                        }
                        playerOrientation = Orientation.DOWN_LEFT;
                    }
                    if (mvAngle > (-112.5) && mvAngle <= (-77.5))
                    {
                        sprite.SetAnimation("RUN_DOWN");
                        playerOrientation = Orientation.DOWN;
                    }
                    if (mvAngle > (-77.5) && mvAngle <= (-22.5))
                    {
                        if (directionVectorLength <= runThreshold)
                        {
                            sprite.SetAnimation("WALK_DOWN_RIGHT");
                        }
                        else
                        {
                            sprite.SetAnimation("RUN_DOWN_RIGHT");
                        }
                        playerOrientation = Orientation.DOWN_RIGHT;
                    }
                }
            }

			OnMove();
        }

        #endregion
        #region CombatCollidable
        

        public /* override */ void OnCombatCollision(CombatArgs combatArgs)
        {
            sprite.Position += combatArgs.knockBack;

            switch(playerOrientation)
            {
                case Orientation.DOWN: sprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.DOWN_LEFT: sprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.LEFT: sprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.UP_LEFT: sprite.SetAnimation("FLINCH_LEFT"); break;
                case Orientation.UP: sprite.SetAnimation("FLINCH_LEFT"); break;

                case Orientation.UP_RIGHT: sprite.SetAnimation("FLINCH_RIGHT"); break;
                case Orientation.RIGHT: sprite.SetAnimation("FLINCH_RIGHT"); break;
                case Orientation.DOWN_RIGHT: sprite.SetAnimation("FLINCH_RIGHT"); break;
            }
            flinching = true;
        }
        #endregion
    }
}
