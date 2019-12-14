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
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(gargoyle, gargoyle.attackRange) != null;//lane mit einbringen
            StateAttack stateAttack = ((StateAttack)stateManagerGargoyle.states.Find((a) => { return a.stateId == "Attack"; }));
            bool cooldownCondition = (Game1.totalGametime - stateAttack.lastAttack) > stateAttack.cooldown;
            return rangeCondition && cooldownCondition;
        }
    }
}

