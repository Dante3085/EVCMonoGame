using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.collision
{
    public interface CombatCollidable
    {
        Rectangle HurtBounds { get; }

        Rectangle AttackBounds { get; }

        bool HasActiveAttackBounds { get; }

        bool HasActiveHurtBounds { get; }

        bool IsAlive { get; }

        CombatArgs CombatArgs { get; }

        void OnCombatCollision(CombatArgs combatArgs);
    }
}
