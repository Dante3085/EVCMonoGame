using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.scenes
{
    public abstract class Updateable
    {
        public bool DoUpdate
        {
            get; set;
        } = true;

        public abstract void Update(GameTime gameTime);
    }
}
