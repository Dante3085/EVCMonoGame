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

namespace EVCMonoGame.src.statemachine.hadesRage
{
    class StateCharge : State
    {

        public StateManagerHadesRage stateManagerHadesRage;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Hades hades;
        public StateCharge(Hades hades, params Transition[] transitions)
            : base("Charge", transitions)
        {
            this.hades = hades;
        }
        public override void Enter(GameTime gameTime)
        {
            switch (Utility.GetOrientationDiagonal(hades.movementDirection))
            {
                case Orientation.DOWN_LEFT:
                    hades.Sprite.SetAnimation("MOVE_LEFT");
                    break;
                case Orientation.DOWN_RIGHT:
                    hades.Sprite.SetAnimation("MOVE_RIGHT");
                    break;
                case Orientation.UP_LEFT:
                    hades.Sprite.SetAnimation("MOVE_LEFT");
                    break;
                case Orientation.UP_RIGHT:
                    hades.Sprite.SetAnimation("MOVE_RIGHT");
                    break;
                default:
                    hades.Sprite.SetAnimation("MOVE_RIGHT");
                    break;
            }

            hades.target = CollisionManager.GetNearestPlayerInRange(hades, hades.sightRange);

            base.Enter(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (hades.target != null) hades.MoveToCharacter(gameTime, hades.target);
            if (Utility.GetOrientationDiagonal(hades.previousMovementDirection) != Utility.GetOrientationDiagonal(hades.movementDirection))
            {
                switch (Utility.GetOrientationDiagonal(hades.movementDirection))
                {
                    case Orientation.DOWN_LEFT:
                        hades.Sprite.SetAnimation("MOVE_LEFT");
                        break;
                    case Orientation.DOWN_RIGHT:
                        hades.Sprite.SetAnimation("MOVE_RIGHT");
                        break;
                    case Orientation.UP_LEFT:
                        hades.Sprite.SetAnimation("MOVE_LEFT");
                        break;
                    case Orientation.UP_RIGHT:
                        hades.Sprite.SetAnimation("MOVE_RIGHT");
                        break;
                    default:
                        hades.Sprite.SetAnimation("MOVE_RIGHT");
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
