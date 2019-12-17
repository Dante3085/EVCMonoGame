using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.hadesRage
{
    class IsDying : Transition
    {
        public StateManagerHadesRage stateManagerHadesRage;
        public IsDying(String nextStateId, StateManagerHadesRage stateManagerHadesRage) : base(nextStateId)
        {
            this.stateManagerHadesRage = stateManagerHadesRage;
        }
        public override bool checkCondition()
        {
            return !stateManagerHadesRage.hades.IsAlive;
        }
    }
}
