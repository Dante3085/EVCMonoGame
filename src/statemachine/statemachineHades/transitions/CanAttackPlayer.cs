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
    class CanAttackPlayer : Transition
    {
        public StateManagerHades stateManagerHades;
        public Hades hades;
        public CanAttackPlayer(String nextStateId, StateManagerHades stateManagerHades) : base(nextStateId)
        {
            this.stateManagerHades = stateManagerHades;
            this.hades = stateManagerHades.hades;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(hades, hades.attackRange) != null;//lane mit einbringen
            StateAttack stateAttack = ((StateAttack)stateManagerHades.states.Find((a) => { return a.stateId == "Attack"; }));
            bool cooldownCondition = (Game1.totalGametime - stateAttack.lastAttack) > stateAttack.cooldown;
            return rangeCondition && cooldownCondition;
        }
    }
}

