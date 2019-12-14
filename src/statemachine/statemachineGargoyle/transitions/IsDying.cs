using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.gargoyle
{
    class IsDying : Transition
    {
        public StateManagerGargoyle stateManagerGargoyle;
        public IsDying(String nextStateId, StateManagerGargoyle stateManagerGargoyle) : base(nextStateId)
        {
            this.stateManagerGargoyle = stateManagerGargoyle;
        }
        public override bool checkCondition()
        {
            return !stateManagerGargoyle.gargoyle.IsAlive;
        }
    }
}
