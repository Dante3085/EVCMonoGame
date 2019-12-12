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
    class CanAttackPlayer : Transition
    {
        public StateManagerShadow stateManagerShadow;
        public Shadow shadow;
        public CanAttackPlayer(String nextStateId, StateManagerShadow stateManagerShadow) : base(nextStateId)
        {
            this.stateManagerShadow = stateManagerShadow;
            this.shadow = stateManagerShadow.shadow;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(shadow, shadow.attackRange) != null;//lane mit einbringen
            StateAttack stateAttack = ((StateAttack)stateManagerShadow.states.Find((a) => { return a.stateId == "Attack"; }));
            bool cooldownCondition = (Game1.totalGametime - stateAttack.lastAttack) > stateAttack.cooldown;
            return rangeCondition && cooldownCondition;
        }
    }
}

