using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using EVCMonoGame.src.screens;

namespace EVCMonoGame.src
{
    public class Easer : Updateable
    {
        private int elapsedMillis;
        private float from;
        private float to;
        private int durationInMillis;
        private Easing.EasingFunction easingFunction;
        private float currentValue;
        private bool isFinished;

        public float CurrentValue
        {
            get { return currentValue; }
        }

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public float From
        {
            get { return from; }
            set
            {
                from = value;
            }
        }

        public float To
        {
            get { return to; }
            set
            {
                to = value;
            }
        }

        public Easer(float from, float to, int durationInMillis, Easing.EasingFunction easingFunction)
        {
            elapsedMillis = 0;
            this.from = from;
            this.to = to;
            this.durationInMillis = durationInMillis;
            this.easingFunction = easingFunction;
            currentValue = from;
            isFinished = false;
            DoUpdate = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!DoUpdate || isFinished)
                return;

            elapsedMillis += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedMillis >= durationInMillis)
            {
                currentValue = to;
                DoUpdate = false;
                isFinished = true;

                return;
            }

            currentValue = easingFunction(elapsedMillis, from, to - from, durationInMillis);
        }

        public void start()
        {
            elapsedMillis = 0;
            currentValue = from;
            isFinished = false;
            DoUpdate = true;
        }

        public void reverse()
        {
            float temp = from;
            from = to;
            to = temp;
        }
    }
}
