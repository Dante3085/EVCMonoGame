using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.statemachine
{
    class StateAttacking : State
    {
        public StateAttacking(StateManagerSora stateManager, params Transition[] transitions) : base(stateManager, "Attacking")
        {
            this.stateId = "Attacking";
            this.transitions = transitions.ToList();
            this.transitions.Add(new TransitionFinishedNoMoving(stateManager, "Idle"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // TODO: Angriffslogik
        }
    }
}
