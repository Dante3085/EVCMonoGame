using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.shadow
{
    class PatrolFinished : Transition
    {
        public StateManagerShadow stateManagerShadow;
        public Shadow shadow;
        public PatrolFinished(String nextStateId, StateManagerShadow stateManagerShadow) : base(nextStateId)
        {
            this.stateManagerShadow = stateManagerShadow;
            this.shadow = stateManagerShadow.shadow;
        }
        public override bool checkCondition()
        {
            StatePatrol statePatrol = ((StatePatrol)stateManagerShadow.states.Find((a) => { return a.stateId == "Patrol"; }));
            bool patrolCondition = statePatrol.stateFinished;
            return patrolCondition;
        }
    }
}