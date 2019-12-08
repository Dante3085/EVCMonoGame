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


namespace EVCMonoGame.src.statemachine.riku
{
    class StateMoving : State
    {
        private PlayerTwo riku = GameplayState.PlayerTwo;
        public StateMoving(params Transition[] transitions)
            : base("Moving", transitions)
        {

        }
        public override void Update(GameTime gameTime)
        {
            Vector2 directionVector = Vector2.Zero;
            riku.previousMovementVector = riku.movementVector;
            riku.PreviousWorldPosition = riku.WorldPosition;

            // Differentiate between Keyboard and GamePad controls.
            if (InputManager.InputByKeyboard)
            {
                if (InputManager.IsKeyPressed(riku.keyboardControls[0])) directionVector.Y -= 100; //up
                if (InputManager.IsKeyPressed(riku.keyboardControls[2])) directionVector.X += 100; //right
                if (InputManager.IsKeyPressed(riku.keyboardControls[1])) directionVector.Y += 100; //down
                if (InputManager.IsKeyPressed(riku.keyboardControls[3])) directionVector.X -= 100; //left

                riku.movementVector = Utility.ScaleVectorTo(directionVector, riku.movementSpeed);
                
                if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                {
                    riku.movementVector *= 2;
                }
            }
            else
            {
                GamePadThumbSticks currentThumbSticks = InputManager.CurrentThumbSticks(PlayerIndex.Two);

                directionVector.X = currentThumbSticks.Left.X;
                directionVector.Y = currentThumbSticks.Left.Y * -1;

                riku.movementVector = directionVector *
                                 (riku.movementSpeed * (1 + InputManager.CurrentTriggers(PlayerIndex.Two).Right));

                // Ignore LeftStickInput Axis Value if it is below 1
                if (Math.Abs(riku.movementVector.X) < 0.7f)
                    riku.movementVector.X = 0;
                if (Math.Abs(riku.movementVector.Y) < 0.7f)
                    riku.movementVector.Y = 0;
            }

            riku.WorldPosition += riku.movementVector;

            CollisionManager.IsCollisionAfterMove(riku, true, true);

            riku.Sprite.Position = riku.WorldPosition;

            // Has a movement happened ?


            float mvAngle = Utility.GetAngleOfVectorInDegrees(riku.movementVector);
            float directionVectorLength = directionVector.Length();

            // Soll verhindern, dass linker Stick 
            if (directionVectorLength < 0.15f)
                return;

            //Console.WriteLine(directionVectorLength);

            if (mvAngle > (-22.5) && mvAngle <= (22.5))
            {
                riku.Sprite.SetAnimation("RUN_RIGHT");
                riku.playerOrientation = Orientation.RIGHT;
            }
            if (mvAngle > (22.5) && mvAngle <= (77.5))
            {
                if (directionVectorLength <= riku.runThreshold)
                {
                    riku.Sprite.SetAnimation("WALK_UP_RIGHT");
                }
                else
                {
                    riku.Sprite.SetAnimation("RUN_UP_RIGHT");
                }
                riku.playerOrientation = Orientation.UP_RIGHT;
            }
            if (mvAngle > (77.5) && mvAngle <= (112.5))
            {
                riku.Sprite.SetAnimation("RUN_UP");
                riku.playerOrientation = Orientation.UP;
            }
            if (mvAngle > (112.5) && mvAngle <= (157.5))
            {
                if (directionVectorLength <= riku.runThreshold)
                {
                    riku.Sprite.SetAnimation("WALK_UP_LEFT");
                }
                else
                {
                    riku.Sprite.SetAnimation("RUN_UP_LEFT");
                }
                riku.playerOrientation = Orientation.UP_LEFT;
            }
            if ((mvAngle > (157.5) && mvAngle <= (180)) || (mvAngle >= (-180) && mvAngle <= (-157.5)))
            {
                riku.Sprite.SetAnimation("RUN_LEFT");
                riku.playerOrientation = Orientation.LEFT;
            }
            if (mvAngle > (-157.5) && mvAngle <= (-112.5))
            {
                if (directionVectorLength <= riku.runThreshold)
                {
                    riku.Sprite.SetAnimation("WALK_DOWN_LEFT");
                }
                else
                {
                    riku.Sprite.SetAnimation("RUN_DOWN_LEFT");
                }
                riku.playerOrientation = Orientation.DOWN_LEFT;
            }
            if (mvAngle > (-112.5) && mvAngle <= (-77.5))
            {
                riku.Sprite.SetAnimation("RUN_DOWN");
                riku.playerOrientation = Orientation.DOWN;
            }
            if (mvAngle > (-77.5) && mvAngle <= (-22.5))
            {
                if (directionVectorLength <= riku.runThreshold)
                {
                    riku.Sprite.SetAnimation("WALK_DOWN_RIGHT");
                }
                else
                {
                    riku.Sprite.SetAnimation("RUN_DOWN_RIGHT");
                }
                riku.playerOrientation = Orientation.DOWN_RIGHT;
            }
        }
    }
}
