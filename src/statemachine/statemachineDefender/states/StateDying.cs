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
using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src.statemachine.defender
{
    class StateDying : State
    {
        public StateManagerDefender stateManagerDefender;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Defender defender;
        public StateDying(Defender defender, params Transition[] transitions)
            : base("Dying", transitions)
        {
            this.defender = defender;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            switch (Utility.GetOrientationDiagonal(defender.movementDirection))
            {
                case Orientation.DOWN_LEFT:
                    defender.Sprite.SetAnimation("FLINCH_LEFT");
                    break;
                case Orientation.DOWN_RIGHT:
                    defender.Sprite.SetAnimation("FLINCH_RIGHT");
                    break;
                case Orientation.UP_LEFT:
                    defender.Sprite.SetAnimation("FLINCH_LEFT");
                    break;
                case Orientation.UP_RIGHT:
                    defender.Sprite.SetAnimation("FLINCH_RIGHT");
                    break;
                default:
                    defender.Sprite.SetAnimation("FLINCH_RIGHT");
                    break;
            }

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (defender.Sprite.AnimationFinished) Exit(gameTime);
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
            defender.FlaggedForRemove = true;
            Scene.updateablesToRemove.Add(defender);
            Scene.drawablesToRemove.Add(defender);
        }
    }
}
