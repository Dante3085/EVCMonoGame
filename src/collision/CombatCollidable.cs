using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.characters;

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

        bool FlaggedForRemove { get; set; }

        void OnCombatCollision(CombatArgs combatArgs);

        CombatantType Combatant{ get; }
    }
}
