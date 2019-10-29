using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.collision
{
    public struct CombatArgs
    {
        public bool knockBack;

        public CombatArgs(bool knockBack = false)
        {
            this.knockBack = knockBack;
        }
    }
}
