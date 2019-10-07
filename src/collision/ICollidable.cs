using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.collision
{
    public interface ICollidable
    {
        Rectangle Bounds { get; }

        Vector2 ShiftVector { get; }

        Vector2 Position { get; set; }

        void HandleCollision(ICollidable partner);
    }
}
