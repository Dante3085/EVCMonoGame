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

namespace EVCMonoGame.src.statemachine.shadow
{
    class StateDying : State
    {
        public StateManagerShadow stateManagerShadow;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Shadow shadow;
        public StateDying(Shadow shadow, params Transition[] transitions)
            : base("Dying", transitions)
        {
            this.shadow = shadow;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(shadow.PreviousWorldPosition - shadow.worldPosition);
            shadow.Sprite.SetAnimation("DESPAWN_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                shadow.Sprite.SetAnimation("DESPAWN_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                shadow.Sprite.SetAnimation("DESPAWN_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                shadow.Sprite.SetAnimation("DESPAWN_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
            {
                shadow.Sprite.SetAnimation("DESPAWN_RIGHT");
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (shadow.Sprite.AnimationFinished) Exit(gameTime);
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
            shadow.FlaggedForRemove = true;
            Scene.updateablesToRemove.Add(shadow);
            Scene.drawablesToRemove.Add(shadow);
        }
    }
}
