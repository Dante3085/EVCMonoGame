using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.hades
{
    class StandingFinished : Transition
    {
        public StateManagerHades stateManagerHades;
        public StandingFinished(String nextStateId, StateManagerHades stateManagerHades) : base(nextStateId)
        {
            this.stateManagerHades = stateManagerHades;
        }
        public override bool checkCondition()
        {
            StateStanding stateStanding = ((StateStanding)stateManagerHades.states.Find((a) => { return a.stateId == "Standing"; }));
            bool durationCondition = (Game1.totalGametime - stateStanding.enteredStateTimeStamp) > stateStanding.duration;
            return durationCondition;
        }
    }
}