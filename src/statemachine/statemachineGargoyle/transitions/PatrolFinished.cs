using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.gargoyle
{
    class PatrolFinished : Transition
    {
        public StateManagerGargoyle stateManagerGargoyle;
        public Gargoyle gargoyle;
        public PatrolFinished(String nextStateId, StateManagerGargoyle stateManagerGargoyle) : base(nextStateId)
        {
            this.stateManagerGargoyle = stateManagerGargoyle;
            this.gargoyle = stateManagerGargoyle.gargoyle;
        }
        public override bool checkCondition()
        {
            StatePatrol statePatrol = ((StatePatrol)stateManagerGargoyle.states.Find((a) => { return a.stateId == "Patrol"; }));
            bool patrolCondition = statePatrol.stateFinished;
            return patrolCondition;
        }
    }
}