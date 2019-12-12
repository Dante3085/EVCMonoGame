using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.shadow
{
    class IsDying : Transition
    {
        public StateManagerShadow stateManagerShadow;
        public IsDying(String nextStateId, StateManagerShadow stateManagerShadow) : base(nextStateId)
        {
            this.stateManagerShadow = stateManagerShadow;
        }
        public override bool checkCondition()
        {
            return !stateManagerShadow.shadow.IsAlive;
        }
    }
}
