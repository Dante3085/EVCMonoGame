using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.collision
{
    public struct CombatArgs
    {
        // Who Attacker and Victim of the CombatCollision was is always provided.
        public CombatCollidable attacker;
        public CombatCollidable victim;

        // Various attributes of a CombatCollision.
        public int damage;
        public Vector2 knockBack;

        public CombatArgs(CombatCollidable attacker, CombatCollidable victim, Vector2 knockBack, int damage = 0)
        {
            this.attacker = attacker;
            this.victim = victim;

            this.knockBack = knockBack;
            this.damage = damage;
        }
    }
}
