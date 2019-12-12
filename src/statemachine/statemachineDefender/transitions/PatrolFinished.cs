using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.defender
{
    class PatrolFinished : Transition
    {
        public StateManagerDefender stateManagerDefender;
        public Defender defender;
        public PatrolFinished(String nextStateId, StateManagerDefender stateManagerDefender) : base(nextStateId)
        {
            this.stateManagerDefender = stateManagerDefender;
            this.defender = stateManagerDefender.defender;
        }
        public override bool checkCondition()
        {
            StatePatrol statePatrol = ((StatePatrol)stateManagerDefender.states.Find((a) => { return a.stateId == "Patrol"; }));
            bool patrolCondition = statePatrol.stateFinished;
            return patrolCondition;
        }
    }
}