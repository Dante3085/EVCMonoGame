using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.gargoyle
{
    class StandingFinished : Transition
    {
        public StateManagerGargoyle stateManagerGargoyle;
        public StandingFinished(String nextStateId, StateManagerGargoyle stateManagerGargoyle) : base(nextStateId)
        {
            this.stateManagerGargoyle = stateManagerGargoyle;
        }
        public override bool checkCondition()
        {
            StateStanding stateStanding = ((StateStanding)stateManagerGargoyle.states.Find((a) => { return a.stateId == "Standing"; }));
            bool durationCondition = (Game1.totalGametime - stateStanding.enteredStateTimeStamp) > stateStanding.duration;
            return durationCondition;
        }
    }
}