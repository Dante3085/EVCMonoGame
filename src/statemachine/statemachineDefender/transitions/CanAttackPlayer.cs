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
    class CanAttackPlayer : Transition
    {
        public StateManagerDefender stateManagerDefender;
        public Defender defender;
        public CanAttackPlayer(String nextStateId, StateManagerDefender stateManagerDefender) : base(nextStateId)
        {
            this.stateManagerDefender = stateManagerDefender;
            this.defender = stateManagerDefender.defender;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(defender, defender.attackRange) != null;//lane mit einbringen
            StateAttack stateAttack = ((StateAttack)stateManagerDefender.states.Find((a) => { return a.stateId == "Attack"; }));
            bool cooldownCondition = (Game1.totalGametime - stateAttack.lastAttack) > stateAttack.cooldown;
            return rangeCondition && cooldownCondition;
        }
    }
}

