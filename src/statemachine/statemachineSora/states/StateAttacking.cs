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
        public StateAttacking(params Transition[] transitions) : base("Attacking")
        {
            this.stateId = "Attacking";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // TODO: Angriffslogik
        }
    }
}
