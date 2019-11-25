using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine.sora
{
    class StateMoving : State
    {
        public StateMoving(params Transition[] transitions)
            : base("Moving", transitions)
        {
        }
    }
}
