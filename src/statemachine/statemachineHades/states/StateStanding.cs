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

namespace EVCMonoGame.src.statemachine.hades
{
    class StateStanding : State
    {

        public StateManagerHades stateManagerHades;
        public TimeSpan enteredStateTimeStamp = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, Utility.random.Next(3, 6));
        public Hades hades;
        public StateStanding(/*StateManagerHades stateManager*/ Hades hades, params Transition[] transitions)
            : base("Standing", transitions)
        {
            this.hades = /*stateManager.*/hades;
            //this.stateManagerHades = stateManagerHades;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            duration = new TimeSpan(0, 0, Utility.random.Next(3, 6));
            Console.WriteLine("Hades duration:" + duration.Seconds);
            enteredStateTimeStamp = gameTime.TotalGameTime;
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(hades.movementDirection);
            hades.Sprite.SetAnimation("IDLE_DOWN_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                hades.Sprite.SetAnimation("IDLE_UP_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                hades.Sprite.SetAnimation("IDLE_UP_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                hades.Sprite.SetAnimation("IDLE_DOWN_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
            {
                hades.Sprite.SetAnimation("IDLE_DOWN_RIGHT");
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
