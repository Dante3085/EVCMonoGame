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
    class PlayerOutOfSightRange : Transition
    {
        public StateManagerGargoyle stateManagerGargoyle;
        public Gargoyle gargoyle;
        public PlayerOutOfSightRange(String nextStateId, StateManagerGargoyle stateManagerGargoyle) : base(nextStateId)
        {
            this.stateManagerGargoyle = stateManagerGargoyle;
            this.gargoyle = stateManagerGargoyle.gargoyle;
        }
        public override bool checkCondition()
        {
            bool rangeCondition = CollisionManager.GetNearestPlayerInRange(gargoyle, gargoyle.sightRange) == null;//lane mit einbringen
            return rangeCondition;
        }
    }
}