using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.hadesRage
{
    class StandingFinished : Transition
    {
        public StateManagerHadesRage stateManagerHadesRage;
        public StandingFinished(String nextStateId, StateManagerHadesRage stateManagerHadesRage) : base(nextStateId)
        {
            this.stateManagerHadesRage = stateManagerHadesRage;
        }
        public override bool checkCondition()
        {
            StateStanding stateStanding = ((StateStanding)stateManagerHadesRage.states.Find((a) => { return a.stateId == "Standing"; }));
            bool durationCondition = (Game1.totalGametime - stateStanding.enteredStateTimeStamp) > stateStanding.duration;
            return durationCondition;
        }
    }
}