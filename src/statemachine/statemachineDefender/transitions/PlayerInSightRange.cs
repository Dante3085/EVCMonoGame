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
    class PlayerInSightRange : Transition
    {
        public StateManagerDefender stateManagerDefender;
        public Defender defender;
        public PlayerInSightRange(String nextStateId, StateManagerDefender stateManagerDefender) : base(nextStateId)
        {
            this.stateManagerDefender = stateManagerDefender;
            this.defender = stateManagerDefender.defender;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(defender, defender.sightRange) != null;//lane mit einbringen
            return rangeCondition;
        }
    }
}