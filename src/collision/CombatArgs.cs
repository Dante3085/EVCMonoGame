using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.collision
{
    public class CombatArgs
    {
        public CombatCollidable attacker;
        public CombatCollidable victim;

        public int damage        = 0;
        public Vector2 knockBack = Vector2.Zero;
        public bool causesFlinch = false;

        public CombatArgs(CombatCollidable attacker, CombatCollidable victim)
        {
            this.attacker = attacker;
            this.victim = victim;
        }
    }
}
