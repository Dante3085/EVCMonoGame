﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.scenes
{
    public interface IUpdateable
    {
        bool DoUpdate
        {
            get; set;
        }

        void Update(GameTime gameTime);
    }
}
