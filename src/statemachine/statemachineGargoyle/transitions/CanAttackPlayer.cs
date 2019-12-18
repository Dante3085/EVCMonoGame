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
    class CanAttackPlayer : Transition
    {
        public StateManagerGargoyle stateManagerGargoyle;
        public Gargoyle gargoyle;
        public CanAttackPlayer(String nextStateId, StateManagerGargoyle stateManagerGargoyle) : base(nextStateId)
        {
            this.stateManagerGargoyle = stateManagerGargoyle;
            this.gargoyle = stateManagerGargoyle.gargoyle;
        }
        public override bool checkCondition()
        {
            bool rangeCloseCondition = CollisionManager.GetNearestPlayerInRange(gargoyle, gargoyle.attackRange / 2) != null;//lane mit einbringen
            bool rangeFarCondition = CollisionManager.GetNearestPlayerInRange(gargoyle, gargoyle.attackRange) != null;

            StateAttack stateAttack = ((StateAttack)stateManagerGargoyle.states.Find((a) => { return a.stateId == "Attack"; }));

            bool cooldownCloseCondition = (Game1.totalGametime - stateAttack.lastCloseAttack) > stateAttack.cooldownClose;
            bool cooldownFarCondition = (Game1.totalGametime - stateAttack.lastCryAttack) > stateAttack.cooldownFar;

            if (rangeCloseCondition && cooldownCloseCondition)
            {
                stateAttack.nextGargoyleAttack = NextGargoyleAttack.CLOSEATTACK;
                return true;
            }

            else if (rangeFarCondition && cooldownFarCondition)
            {
                stateAttack.nextGargoyleAttack = NextGargoyleAttack.CRYATTACK;
                return true;
            }

            return false;
        }
    }
}

