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

namespace EVCMonoGame.src.statemachine.defender
{
    class StateCharge : State
    {

        public StateManagerDefender stateManagerDefender;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Defender defender;
        public StateCharge(Defender defender, params Transition[] transitions)
            : base("Charge", transitions)
        {
            this.defender = defender;
        }
        public override void Enter(GameTime gameTime)
        {
            switch (Utility.GetOrientationDiagonal(defender.movementDirection))
            {
                case Orientation.DOWN_LEFT:
                    defender.Sprite.SetAnimation("RUN_LEFT");
                    break;
                case Orientation.DOWN_RIGHT:
                    defender.Sprite.SetAnimation("RUN_RIGHT");
                    break;
                case Orientation.UP_LEFT:
                    defender.Sprite.SetAnimation("RUN_LEFT");
                    break;
                case Orientation.UP_RIGHT:
                    defender.Sprite.SetAnimation("RUN_RIGHT");
                    break;
                default:
                    defender.Sprite.SetAnimation("RUN_RIGHT");
                    break;
            }

            defender.target = CollisionManager.GetNearestPlayerInRange(defender, defender.sightRange);

            base.Enter(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (defender.target != null) defender.MoveToCharacter(gameTime, defender.target);
            if (Utility.GetOrientationDiagonal(defender.previousMovementDirection) != Utility.GetOrientationDiagonal(defender.movementDirection))
            {
                switch (Utility.GetOrientationDiagonal(defender.movementDirection))
                {
                    case Orientation.DOWN_LEFT:
                        defender.Sprite.SetAnimation("RUN_LEFT");
                        break;
                    case Orientation.DOWN_RIGHT:
                        defender.Sprite.SetAnimation("RUN_RIGHT");
                        break;
                    case Orientation.UP_LEFT:
                        defender.Sprite.SetAnimation("RUN_LEFT");
                        break;
                    case Orientation.UP_RIGHT:
                        defender.Sprite.SetAnimation("RUN_RIGHT");
                        break;
                    default:
                        defender.Sprite.SetAnimation("RUN_RIGHT");
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
