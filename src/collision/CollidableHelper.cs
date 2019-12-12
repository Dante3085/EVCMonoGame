using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.collision
{
    class CollidableHelper : Collidable
    {
        public Vector2 WorldPosition { get ; set ; }

        public Vector2 PreviousWorldPosition { get { return WorldPosition; }  }

        public Rectangle CollisionBox { get; set; }

        public bool FlaggedForRemove { get; set; } = false;

        public CollidableHelper(Point position, Point size)
        {
            WorldPosition = position.ToVector2();
            CollisionBox = new Rectangle(position, size);
        }

    }
}
