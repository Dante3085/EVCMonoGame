using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.collision
{
    public interface Collidable
    {
        Vector2 WorldPosition { get; set; }
        Vector2 PreviousWorldPosition { get; }
        Rectangle CollisionBox { get; }
    }
}
