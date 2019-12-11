﻿using System;
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
        public TimeSpan duration = new TimeSpan(0, 0, 10);
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
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(shadow.PreviousWorldPosition - shadow.worldPosition);
            shadow.Sprite.SetAnimation("IDLE_DOWN_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                shadow.Sprite.SetAnimation("IDLE_UP_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                shadow.Sprite.SetAnimation("IDLE_UP_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                shadow.Sprite.SetAnimation("IDLE_DOWN_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
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
