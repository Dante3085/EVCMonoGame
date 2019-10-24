
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.enemies
{
    public class Shadow : Enemy
    {

        public Shadow(Vector2 position)
            : base(position)
        {
            enemySprite.LoadFromFile("Content/rsrc/spritesheets/configFiles/shadow.txt");
            enemySprite.SetAnimation("IDLE_LEFT");
        }
    }
}
