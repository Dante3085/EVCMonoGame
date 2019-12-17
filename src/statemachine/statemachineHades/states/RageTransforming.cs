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
using EVCMonoGame.src.statemachine.hadesRage;

namespace EVCMonoGame.src.statemachine.hades
{
    class RageTransforming : State
    {
        public StateManagerHades stateManagerHades;
        public Hades hades;
        public RageTransforming(Hades hades, params Transition[] transitions)
            : base("RageTransform", transitions)
        {
            //this.stateManagerHades = (StateManagerHades)hades.stateManager;
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
            ((StateManagerHades)hades.stateManager).shouldUpdate = false;
            hades.stateManager = new StateManagerHadesRage(hades);
        }
    }
}
