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
        /// <summary>
        /// Area on or around a CombatCollidable that can be hit by AttackBounds to cause a
        /// CombaCollision.
        /// </summary>
        Rectangle HurtBounds { get; }

        /// <summary>
        /// Area on or around CombatCollidable that can hit HurtBounds of another CombatCollidable.
        /// to cause a CombatCollision.
        /// </summary>
        Rectangle AttackBounds { get; }

        /// <summary>
        /// Determines if the CombatCollidable currently has an active AttackBounds. 
        /// It is possible that no Attack is currently happening and no AttackBounds are active.
        /// In that case it wouldn't make sense to query the AttackBounds.
        /// </summary>
        bool HasActiveAttackBounds { get; }

        bool IsAlive { get; }

        int CurrentDamage { get; }

        void ReceiveDamage(int amount);

        void OnCombatCollision();
    }
}
