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
    class PlayerOutOfSightRange : Transition
    {
        public StateManagerHadesRage stateManagerHadesRage;
        public Hades hades;
        public PlayerOutOfSightRange(String nextStateId, StateManagerHadesRage stateManagerHadesRage) : base(nextStateId)
        {
            this.stateManagerHadesRage = stateManagerHadesRage;
            this.hades = stateManagerHadesRage.hades;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(hades, hades.sightRange) == null;//lane mit einbringen
            return rangeCondition;
        }
    }
}