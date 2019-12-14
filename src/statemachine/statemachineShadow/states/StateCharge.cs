using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.statemachine.shadow
{
    class StateCharge : State
    {

        public StateManagerShadow stateManagerShadow;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Shadow shadow;
        public StateCharge(Shadow shadow, params Transition[] transitions)
            : base("Charge", transitions)
        {
            this.shadow = shadow;
        }
        public override void Enter(GameTime gameTime)
        {
            switch (Utility.GetOrientationDiagonal(shadow.movementDirection))
            {
                case Orientation.DOWN_LEFT:
                    shadow.Sprite.SetAnimation("WALK_DOWN_LEFT");
                    break;
                case Orientation.DOWN_RIGHT:
                    shadow.Sprite.SetAnimation("WALK_DOWN_RIGHT");
                    break;
                case Orientation.UP_LEFT:
                    shadow.Sprite.SetAnimation("WALK_UP_LEFT");
                    break;
                case Orientation.UP_RIGHT:
                    shadow.Sprite.SetAnimation("WALK_UP_RIGHT");
                    break;
                default:
                    shadow.Sprite.SetAnimation("WALK_UP_RIGHT");
                    break;
            }
            
            shadow.target = CollisionManager.GetNearestPlayerInRange(shadow, shadow.sightRange);
            
            base.Enter(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (shadow.target != null) shadow.MoveToCharacter(gameTime, shadow.target);
            if (Utility.GetOrientationDiagonal(shadow.previousMovementDirection) != Utility.GetOrientationDiagonal(shadow.movementDirection))
            {
                switch (Utility.GetOrientationDiagonal(shadow.movementDirection))
                {
                    case Orientation.DOWN_LEFT:
                        shadow.Sprite.SetAnimation("WALK_DOWN_LEFT");
                        break;
                    case Orientation.DOWN_RIGHT:
                        shadow.Sprite.SetAnimation("WALK_DOWN_RIGHT");
                        break;
                    case Orientation.UP_LEFT:
                        shadow.Sprite.SetAnimation("WALK_UP_LEFT");
                        break;
                    case Orientation.UP_RIGHT:
                        shadow.Sprite.SetAnimation("WALK_UP_RIGHT");
                        break;
                    default:
                        shadow.Sprite.SetAnimation("WALK_UP_RIGHT");
                        break;
                }
            }
           
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
        }
    }
}
