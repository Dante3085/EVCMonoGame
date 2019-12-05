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
        }

    }
}
