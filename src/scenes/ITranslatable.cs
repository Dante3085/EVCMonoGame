using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.scenes
{
    public interface ITranslatable
    {
        Vector2 Position { get; set; }
    }
}
