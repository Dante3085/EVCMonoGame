using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src.scenes
{
    class ITranslatablePosition : ITranslatable
    {
        float X { get; set; }
        float Y { get; set; }
        public ITranslatablePosition()
        {
            X = 0;
            Y = 0;
        }
        public ITranslatablePosition(float x, float y)
        {
            Position = new Vector2(x, y);
        }
        public ITranslatablePosition(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }

            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
    }
}
