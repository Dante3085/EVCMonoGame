using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.statemachine
{
    public abstract class Transition
    {
        public String nextStateId;
        public Transition(String nextStateId)
        {
            this.nextStateId = nextStateId;
        }
        public virtual bool checkCondition() { return false; }
    }
}
