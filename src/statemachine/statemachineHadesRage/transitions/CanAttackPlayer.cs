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
    class CanAttackPlayer : Transition
    {
        public StateManagerHadesRage stateManagerHadesRage;
        public Hades hades;
        public CanAttackPlayer(String nextStateId, StateManagerHadesRage stateManagerHadesRage) : base(nextStateId)
        {
            this.stateManagerHadesRage = stateManagerHadesRage;
            this.hades = stateManagerHadesRage.hades;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(hades, hades.attackRange) != null;//lane mit einbringen
            StateAttack stateAttack = ((StateAttack)stateManagerHadesRage.states.Find((a) => { return a.stateId == "Attack"; }));
            bool cooldownCondition = (Game1.totalGametime - stateAttack.lastAttack) > stateAttack.cooldown;
            return rangeCondition && cooldownCondition;
        }
    }
}

