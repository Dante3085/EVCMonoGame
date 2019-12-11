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
    class StateStanding : State
    {

        public StateManagerGargoyle stateManagerGargoyle;
        public TimeSpan enteredStateTimeStamp = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, Utility.random.Next(3, 6));
        public Gargoyle gargoyle;
        public StateStanding(/*StateManagerGargoyle stateManager*/ Gargoyle gargoyle, params Transition[] transitions)
            : base("Standing", transitions)
        {
            this.gargoyle = /*stateManager.*/gargoyle;
            //this.stateManagerGargoyle = stateManagerGargoyle;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            duration = new TimeSpan(0, 0, Utility.random.Next(3, 6));
            enteredStateTimeStamp = gameTime.TotalGameTime;
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(gargoyle.PreviousWorldPosition - gargoyle.worldPosition);
            gargoyle.Sprite.SetAnimation("IDLE_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                gargoyle.Sprite.SetAnimation("IDLE_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                gargoyle.Sprite.SetAnimation("IDLE_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                gargoyle.Sprite.SetAnimation("IDLE_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
            {
                gargoyle.Sprite.SetAnimation("IDLE_RIGHT");
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
