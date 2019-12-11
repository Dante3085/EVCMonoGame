using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.characters;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.statemachine.defender
{
    class StateManagerDefender : StateManager
    {
        public Defender defender;
        public StateManagerDefender(Defender defender)
        {
            this.defender = defender;
            this.states.Add(new StateStanding(defender,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new StandingFinished("Patrol", this)
            ));
            this.states.Add(new StatePatrol(defender,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new PatrolFinished("Standing", this)
            ));
            this.states.Add(new StateCharge(defender,
                new IsDying("Dying", this),
                new PlayerOutOfSightRange("Standing", this),
                new CanAttackPlayer("Attack", this)
            ));
            this.states.Add(new StateAttack(defender,
                new IsDying("Dying", this),
                new AttackFinished("Standing", this)
            ));
            this.states.Add(new StateDying(defender));
            this.currentState = states.Find((a) => { return a.stateId.Equals("Standing"); });
        }

    }
}
