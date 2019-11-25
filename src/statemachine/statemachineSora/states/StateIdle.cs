using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.sora
{
    class StateIdle: State
    {
        public StateIdle(params Transition[] transitions)
            : base("Idle", transitions)
        {
        }
    }
}
