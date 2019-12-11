using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.defender
{
    class AttackFinished : Transition
    {
        public StateManagerDefender stateManagerDefender;
        public Defender defender;
        public AttackFinished(String nextStateId, StateManagerDefender stateManagerDefender) : base(nextStateId)
        {
            this.stateManagerDefender = stateManagerDefender;
            this.defender = stateManagerDefender.defender;
        }
        public override bool checkCondition()
        {
            bool attackCondition = defender.Sprite.AnimationFinished;
            return attackCondition;
        }
    }
}