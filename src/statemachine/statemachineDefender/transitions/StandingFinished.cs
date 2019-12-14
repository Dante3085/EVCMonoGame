using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.defender
{
    class StandingFinished : Transition
    {
        public StateManagerDefender stateManagerDefender;
        public StandingFinished(String nextStateId, StateManagerDefender stateManagerDefender) : base(nextStateId)
        {
            this.stateManagerDefender = stateManagerDefender;
        }
        public override bool checkCondition()
        {
            StateStanding stateStanding = ((StateStanding)stateManagerDefender.states.Find((a) => { return a.stateId == "Standing"; }));
            bool durationCondition = (Game1.totalGametime - stateStanding.enteredStateTimeStamp) > stateStanding.duration;
            return durationCondition;
        }
    }
}