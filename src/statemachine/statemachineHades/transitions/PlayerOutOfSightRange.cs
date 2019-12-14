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
    class PlayerOutOfSightRange : Transition
    {
        public StateManagerHades stateManagerHades;
        public Hades hades;
        public PlayerOutOfSightRange(String nextStateId, StateManagerHades stateManagerHades) : base(nextStateId)
        {
            this.stateManagerHades = stateManagerHades;
            this.hades = stateManagerHades.hades;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(hades, hades.sightRange) == null;//lane mit einbringen
            return rangeCondition;
        }
    }
}