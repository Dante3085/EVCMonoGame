using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.shadow
{
    class StandingFinished : Transition
    {
        public StateManagerShadow stateManagerShadow;
        public StandingFinished(String nextStateId, StateManagerShadow stateManagerShadow) : base(nextStateId)
        {
            this.stateManagerShadow = stateManagerShadow;
        }
        public override bool checkCondition()
        {
            StateStanding stateStanding = ((StateStanding)stateManagerShadow.states.Find((a) => { return a.stateId == "Standing"; }));
            bool durationCondition = (Game1.totalGametime - stateStanding.enteredStateTimeStamp) > stateStanding.duration;
            return durationCondition;
        }
    }
}