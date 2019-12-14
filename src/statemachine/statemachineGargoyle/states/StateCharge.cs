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

namespace EVCMonoGame.src.statemachine.gargoyle
{
    class StateCharge : State
    {

        public StateManagerGargoyle stateManagerGargoyle;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Gargoyle gargoyle;
        public StateCharge(Gargoyle gargoyle, params Transition[] transitions)
            : base("Charge", transitions)
        {
            this.gargoyle = gargoyle;
        }
        public override void Enter(GameTime gameTime)
        {
            switch (Utility.GetOrientationDiagonal(gargoyle.movementDirection))
            {
                case Orientation.DOWN_LEFT:
                    gargoyle.Sprite.SetAnimation("FLYING_LEFT");
                    break;
                case Orientation.DOWN_RIGHT:
                    gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
                    break;
                case Orientation.UP_LEFT:
                    gargoyle.Sprite.SetAnimation("FLYING_LEFT");
                    break;
                case Orientation.UP_RIGHT:
                    gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
                    break;
                default:
                    gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
                    break;
            }

            gargoyle.target = CollisionManager.GetNearestPlayerInRange(gargoyle, gargoyle.sightRange);

            base.Enter(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gargoyle.target != null) gargoyle.MoveToCharacter(gameTime, gargoyle.target);
            if (Utility.GetOrientationDiagonal(gargoyle.previousMovementDirection) != Utility.GetOrientationDiagonal(gargoyle.movementDirection))
            {
                switch (Utility.GetOrientationDiagonal(gargoyle.movementDirection))
                {
                    case Orientation.DOWN_LEFT:
                        gargoyle.Sprite.SetAnimation("FLYING_LEFT");
                        break;
                    case Orientation.DOWN_RIGHT:
                        gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
                        break;
                    case Orientation.UP_LEFT:
                        gargoyle.Sprite.SetAnimation("FLYING_LEFT");
                        break;
                    case Orientation.UP_RIGHT:
                        gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
                        break;
                    default:
                        gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
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
