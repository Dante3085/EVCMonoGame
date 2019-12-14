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
    class PlayerOutOfSightRange : Transition
    {
        public StateManagerShadow stateManagerShadow;
        public Shadow shadow;
        public PlayerOutOfSightRange(String nextStateId, StateManagerShadow stateManagerShadow) : base(nextStateId)
        {
            this.stateManagerShadow = stateManagerShadow;
            this.shadow = stateManagerShadow.shadow;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(shadow, shadow.sightRange) == null;//lane mit einbringen
            return rangeCondition;
        }
    }
}