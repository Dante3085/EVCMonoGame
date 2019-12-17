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

namespace EVCMonoGame.src.statemachine.hadesRage
{
    class StateDying : State
    {
        public StateManagerHadesRage stateManagerHadesRage;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Hades hades;
        public StateDying(Hades hades, params Transition[] transitions)
            : base("Dying", transitions)
        {
            this.hades = hades;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            switch (Utility.GetOrientationDiagonal(hades.movementDirection))
            {
                case Orientation.DOWN_LEFT:
                    hades.Sprite.SetAnimation("RAGE_TRANSFORMATION_LEFT");
                    break;
                case Orientation.DOWN_RIGHT:
                    hades.Sprite.SetAnimation("RAGE_TRANSFORMATION_RIGHT");
                    break;
                case Orientation.UP_LEFT:
                    hades.Sprite.SetAnimation("RAGE_TRANSFORMATION_LEFT");
                    break;
                case Orientation.UP_RIGHT:
                    hades.Sprite.SetAnimation("RAGE_TRANSFORMATION_RIGHT");
                    break;
                default:
                    hades.Sprite.SetAnimation("RAGE_TRANSFORMATION_RIGHT");
                    break;
            }

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (hades.Sprite.AnimationFinished) Exit(gameTime);
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
            hades.FlaggedForRemove = true;
            Scene.updateablesToRemove.Add(hades);
            Scene.drawablesToRemove.Add(hades);
        }
    }
}
