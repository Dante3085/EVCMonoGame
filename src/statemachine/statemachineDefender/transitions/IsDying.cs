using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.defender
{
    class IsDying : Transition
    {
        public StateManagerDefender stateManagerDefender;
        public IsDying(String nextStateId, StateManagerDefender stateManagerDefender) : base(nextStateId)
        {
            this.stateManagerDefender = stateManagerDefender;
        }
        public override bool checkCondition()
        {
            return !stateManagerDefender.defender.IsAlive;
        }
    }
}
