using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.collision
{
    public interface ICombatCollidable : ICollidable
    {
        int Damage { get; }

        void HandleCombatCollision(ICombatCollidable partner);
    }
}
