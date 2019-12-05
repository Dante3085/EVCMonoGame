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
    class StateStanding : State
    {

        public StateManagerShadow stateManagerShadow;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan cooldown = new TimeSpan(0, 0, 10);
        public Shadow shadow;
        public StateStanding(/*StateManagerShadow stateManager*/ Shadow shadow, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.shadow = /*stateManager.*/shadow;
            //this.stateManagerShadow = stateManagerShadow;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            enteredState = gameTime.TotalGameTime;
            float mvAngle = Utility.GetAngleOfVectorInDegrees(shadow.PreviousWorldPosition - shadow.worldPosition);
            
            if (mvAngle > (0) && mvAngle <= (90))
            {
                shadow.Sprite.SetAnimation("IDLE_UP_RIGHT");
            }
            if (mvAngle > (90) && mvAngle <= (180))
            {
                shadow.Sprite.SetAnimation("IDLE_UP_LEFT");
            }
            if (mvAngle > (-180) && mvAngle <= (-90))
            {
                shadow.Sprite.SetAnimation("IDLE_DOWN_LEFT");
            }
            if (mvAngle > (-90) && mvAngle <= (0))
            {
                shadow.Sprite.SetAnimation("IDLE_DOWN_RIGHT");
            }
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
