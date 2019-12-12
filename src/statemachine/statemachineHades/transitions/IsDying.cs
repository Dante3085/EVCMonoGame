using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.hades
{
    class IsDying : Transition
    {
        public StateManagerHades stateManagerHades;
        public IsDying(String nextStateId, StateManagerHades stateManagerHades) : base(nextStateId)
        {
            this.stateManagerHades = stateManagerHades;
        }
        public override bool checkCondition()
        {
            return !stateManagerHades.hades.IsAlive;
        }
    }
}
