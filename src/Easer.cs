using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src
{
    // TODO: Implement Easer with Vector2 internally, so that Easing of 2D-Positions(i.e. 2 values 
    // at the same time) is natural/easy to implement

    public class Easer : scenes.IUpdateable
    {
        #region Fields
        private int elapsedMillis;
        private int durationInMillis;
        private Easing.EasingFunction easingFunction;
        private bool isFinished;

        private Vector2 from;
        private Vector2 to;
        private Vector2 currentValue;

        #endregion
        #region Properties

        public Vector2 CurrentValue
        {
            get { return currentValue; }
        }

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public Vector2 From
        {
            get { return from; }
            set
            {
                from = value;
            }
        }

        public Vector2 To
        {
            get { return to; }
            set
            {
                to = value;
            }
        }

        public Easing.EasingFunction EasingFunction
        {
            get { return easingFunction; }
            set { easingFunction = value; }
        }

        public int DurationInMillis
        {
            get { return durationInMillis; }
            set { durationInMillis = value; }
        }

        public bool DoUpdate
        {
            get; set;
        } = true;

        #endregion

        #region Constructors
        public Easer(Vector2 from, Vector2 to, int durationInMillis, Easing.EasingFunction easingFunction)
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

        #endregion

        #region Methods
        public void Update(GameTime gameTime)
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

            currentValue.X = easingFunction(elapsedMillis, from.X, to.X - from.X, durationInMillis);
            currentValue.Y = easingFunction(elapsedMillis, from.Y, to.Y - from.Y, durationInMillis);
        }

        public void Start()
        {
            elapsedMillis = 0;
            currentValue = from;
            isFinished = false;
            DoUpdate = true;
        }

        public void Reverse()
        {
            Vector2 temp = from;
            from = to;
            to = temp;
        }

        public void TogglePause()
        {
            DoUpdate = DoUpdate ? false : true;
        }

        #endregion
    }
}
