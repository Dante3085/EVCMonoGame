using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.collision
{
    public interface GeometryCollidable : Collidable
    {
		void OnGeometryCollision(GeometryCollidable collider);
    }
}
