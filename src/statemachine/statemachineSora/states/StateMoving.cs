using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.input;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.collision;


namespace EVCMonoGame.src.statemachine.sora
{
    class StateMoving : State
    {
        private PlayerOne sora = GameplayState.PlayerOne;

        private float previousMovementSpeed;

        public StateMoving(params Transition[] transitions)
            : base("Moving", transitions)
        {

        }

        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);

            if (sora.weapon != null)
            {
                previousMovementSpeed = sora.movementSpeed;
                sora.movementSpeed += sora.weapon.speed;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 directionVector = Vector2.Zero;
            sora.previousMovementVector = sora.movementVector;
            sora.PreviousWorldPosition = sora.WorldPosition;

            // Differentiate between Keyboard and GamePad controls.
            if (InputManager.InputByKeyboard)
            {
                if (InputManager.IsKeyPressed(sora.keyboardControls[0])) directionVector.Y -= 100; //up
                if (InputManager.IsKeyPressed(sora.keyboardControls[2])) directionVector.X += 100; //right
                if (InputManager.IsKeyPressed(sora.keyboardControls[1])) directionVector.Y += 100; //down
                if (InputManager.IsKeyPressed(sora.keyboardControls[3])) directionVector.X -= 100; //left

                sora.movementVector = Utility.ScaleVectorTo(directionVector, sora.movementSpeed);
                
                if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                {
                    sora.movementVector *= 2;
                }
            }
            else
            {
                GamePadThumbSticks currentThumbSticks = InputManager.CurrentThumbSticks(PlayerIndex.One);

                directionVector.X = currentThumbSticks.Left.X;
                directionVector.Y = currentThumbSticks.Left.Y * -1;

                sora.movementVector = directionVector *
                                 (sora.movementSpeed * (1 + InputManager.CurrentTriggers(PlayerIndex.One).Right));

                // Ignore LeftStickInput Axis Value if it is below 1
                if (Math.Abs(sora.movementVector.X) < 0.7f)
                    sora.movementVector.X = 0;
                if (Math.Abs(sora.movementVector.Y) < 0.7f)
                    sora.movementVector.Y = 0;
            }

            sora.WorldPosition += sora.movementVector;

            CollisionManager.IsCollisionAfterMove(sora, true, true);

            sora.Sprite.Position = sora.WorldPosition;

            // Has a movement happened ?


            float mvAngle = Utility.GetAngleOfVectorInDegrees(sora.movementVector);
            float directionVectorLength = directionVector.Length();

            // Soll verhindern, dass linker Stick 
            if (directionVectorLength < 0.15f)
                return;

            //Console.WriteLine(directionVectorLength);

            if (mvAngle > (-22.5) && mvAngle <= (22.5))
            {
                sora.Sprite.SetAnimation("RUN_RIGHT");
                sora.playerOrientation = Orientation.RIGHT;
            }
            if (mvAngle > (22.5) && mvAngle <= (77.5))
            {
                if (directionVectorLength <= sora.runThreshold)
                {
                    sora.Sprite.SetAnimation("WALK_UP_RIGHT");
                }
                else
                {
                    sora.Sprite.SetAnimation("RUN_UP_RIGHT");
                }
                sora.playerOrientation = Orientation.UP_RIGHT;
            }
            if (mvAngle > (77.5) && mvAngle <= (112.5))
            {
                sora.Sprite.SetAnimation("RUN_UP");
                sora.playerOrientation = Orientation.UP;
            }
            if (mvAngle > (112.5) && mvAngle <= (157.5))
            {
                if (directionVectorLength <= sora.runThreshold)
                {
                    sora.Sprite.SetAnimation("WALK_UP_LEFT");
                }
                else
                {
                    sora.Sprite.SetAnimation("RUN_UP_LEFT");
                }
                sora.playerOrientation = Orientation.UP_LEFT;
            }
            if ((mvAngle > (157.5) && mvAngle <= (180)) || (mvAngle >= (-180) && mvAngle <= (-157.5)))
            {
                sora.Sprite.SetAnimation("RUN_LEFT");
                sora.playerOrientation = Orientation.LEFT;
            }
            if (mvAngle > (-157.5) && mvAngle <= (-112.5))
            {
                if (directionVectorLength <= sora.runThreshold)
                {
                    sora.Sprite.SetAnimation("WALK_DOWN_LEFT");
                }
                else
                {
                    sora.Sprite.SetAnimation("RUN_DOWN_LEFT");
                }
                sora.playerOrientation = Orientation.DOWN_LEFT;
            }
            if (mvAngle > (-112.5) && mvAngle <= (-77.5))
            {
                sora.Sprite.SetAnimation("RUN_DOWN");
                sora.playerOrientation = Orientation.DOWN;
            }
            if (mvAngle > (-77.5) && mvAngle <= (-22.5))
            {
                if (directionVectorLength <= sora.runThreshold)
                {
                    sora.Sprite.SetAnimation("WALK_DOWN_RIGHT");
                }
                else
                {
                    sora.Sprite.SetAnimation("RUN_DOWN_RIGHT");
                }
                sora.playerOrientation = Orientation.DOWN_RIGHT;
            }
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);

            if (sora.weapon != null)
            {
                sora.movementSpeed = previousMovementSpeed;
            }
        }
    }
}
