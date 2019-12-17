using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.hadesRage
{
    class PatrolFinished : Transition
    {
        public StateManagerHadesRage stateManagerHadesRage;
        public Hades hades;
        public PatrolFinished(String nextStateId, StateManagerHadesRage stateManagerHadesRage) : base(nextStateId)
        {
            this.stateManagerHadesRage = stateManagerHadesRage;
            this.hades = stateManagerHadesRage.hades;
        }
        public override bool checkCondition()
        {
            StatePatrol statePatrol = ((StatePatrol)stateManagerHadesRage.states.Find((a) => { return a.stateId == "Patrol"; }));
            bool patrolCondition = statePatrol.stateFinished;
            return patrolCondition;
        }
    }
}