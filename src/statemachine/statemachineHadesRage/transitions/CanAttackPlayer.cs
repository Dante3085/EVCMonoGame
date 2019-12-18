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
            
            bool rangeStrikeCondition = CollisionManager.GetNearestPlayerInRange(hades, hades.attackRange) != null;
            bool rangeFireBlastCondition = CollisionManager.GetNearestPlayerInRange(hades, hades.innerAttackRangeFireBlast) == null
                && CollisionManager.GetNearestPlayerInRange(hades, hades.outerAttackRangeFireBlast) != null;

            StateAttack stateAttack = ((StateAttack)stateManagerHadesRage.states.Find((a) => { return a.stateId == "Attack"; }));

            bool cooldownStrikeCondition = (Game1.totalGametime - stateAttack.lastStrikeAttack) > stateAttack.cooldownStrike;
            bool cooldownFireBlastCondition = (Game1.totalGametime - stateAttack.lastFireBlastAttack) > stateAttack.cooldownFireBlast;
            bool cooldownMeteorCondition = (Game1.totalGametime - stateAttack.lastMeteorAttack) > stateAttack.cooldownMeteor;
            
            if (rangeStrikeCondition && cooldownStrikeCondition)
            {
                stateAttack.nextAttack = NEXTATTACK.STRIKE;
                return true;
            }
            else if (rangeFireBlastCondition && cooldownFireBlastCondition)
            {
                stateAttack.nextAttack = NEXTATTACK.FIREBLAST;
                return true;
            }
            else if (cooldownMeteorCondition)
            {
                stateAttack.nextAttack = NEXTATTACK.METEOR;
                return true;
            }
            return false;
        }
    }
}

