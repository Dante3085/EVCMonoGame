using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.characters;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.statemachine.shadow
{
    class StateManagerShadow : StateManager
    {
        public Shadow shadow;
        public StateManagerShadow(Shadow shadow)
        {
            this.shadow = shadow;
            this.states.Add(new StateStanding(shadow,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new StandingFinished("Patrol", this)
            ));
            this.states.Add(new StatePatrol(shadow,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new PatrolFinished("Standing", this)
            ));
            this.states.Add(new StateCharge(shadow,
                new IsDying("Dying", this),
                new PlayerOutOfSightRange("Standing", this),
                new CanAttackPlayer("Attack", this)
            ));
            this.states.Add(new StateAttack(shadow,
                new IsDying("Dying", this),
                new AttackFinished("Standing", this)
            ));
            this.states.Add(new StateDying(shadow));
            this.currentState = states.Find((a) => { return a.stateId.Equals("Standing"); });
        }

    }
}
