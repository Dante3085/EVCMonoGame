using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.hades
{
    class PatrolFinished : Transition
    {
        public StateManagerHades stateManagerHades;
        public Hades hades;
        public PatrolFinished(String nextStateId, StateManagerHades stateManagerHades) : base(nextStateId)
        {
            this.stateManagerHades = stateManagerHades;
            this.hades = stateManagerHades.hades;
        }
        public override bool checkCondition()
        {
            StatePatrol statePatrol = ((StatePatrol)stateManagerHades.states.Find((a) => { return a.stateId == "Patrol"; }));
            bool patrolCondition = statePatrol.stateFinished;
            return patrolCondition;
        }
    }
}