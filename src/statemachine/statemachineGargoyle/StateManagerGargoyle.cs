using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.characters;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.statemachine.gargoyle
{
    class StateManagerGargoyle : StateManager
    {
        public Gargoyle gargoyle;
        public StateManagerGargoyle(Gargoyle gargoyle)
        {
            this.gargoyle = gargoyle;
            this.states.Add(new StateStanding(gargoyle,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new StandingFinished("Patrol", this)
            ));
            this.states.Add(new StatePatrol(gargoyle,
                new IsDying("Dying", this),
                new PlayerInSightRange("Charge", this),
                new PatrolFinished("Standing", this)
            ));
            this.states.Add(new StateCharge(gargoyle,
                new IsDying("Dying", this),
                new PlayerOutOfSightRange("Standing", this),
                new CanAttackPlayer("Attack", this)
            ));
            this.states.Add(new StateAttack(gargoyle,
                new IsDying("Dying", this),
                new AttackFinished("Standing", this)
            ));
            this.states.Add(new StateDying(gargoyle));
            this.currentState = states.Find((a) => { return a.stateId.Equals("Standing"); });
        }

    }
}
