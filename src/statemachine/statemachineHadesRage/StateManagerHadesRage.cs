using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.characters;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.statemachine.hadesRage
{
    class StateManagerHadesRage : StateManager
    {
        public Hades hades;
        public StateManagerHadesRage(Hades hades)
        {
            this.hades = hades;
            this.states.Add(new StateStanding(hades,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new StandingFinished("Patrol", this),
                new CanAttackPlayer("Attack", this)
            ));
            this.states.Add(new StatePatrol(hades,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new PatrolFinished("Standing", this),
                new CanAttackPlayer("Attack", this)
            ));
            this.states.Add(new StateCharge(hades,
                new IsDying("Dying", this),
                new PlayerOutOfSightRange("Standing", this),
                new CanAttackPlayer("Attack", this)
            ));
            this.states.Add(new StateAttack(hades,
                new IsDying("Dying", this),
                new AttackFinished("Standing", this)
            ));
            this.states.Add(new StateDying(hades));
            this.currentState = states.Find((a) => { return a.stateId.Equals("Standing"); });
        }

    }
}
