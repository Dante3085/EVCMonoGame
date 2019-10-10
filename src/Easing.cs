using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src
{
    public static class Easing
    {
        public delegate float EasingFunction(float t, float b, float c, float d);

        /// <summary>
        /// </summary>
        /// <param name="t">elapsedTime (same unit as duration)</param>
        /// <param name="b">startValue</param>
        /// <param name="c">Difference between endValue and startValue</param>
        /// <param name="d">duration</param>
        /// <returns></returns>
        public static float BackEaseIn(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            float postFix = t /= d;
            return c * (postFix) * t * ((s + 1) * t - s) + b;
        }

        public static float BackEaseOut(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
        }

        public static float BackEaseInOut(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            float postFix = t -= 2;
            return c / 2 * ((postFix) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }

        public static float BounceEaseIn(float t, float b, float c, float d)
        {
            return c - BounceEaseOut(d - t, 0, c, d) + b;
        }
        public static float BounceEaseOut(float t, float b, float c, float d)
        {
            if ((t /= d) < (1 / 2.75f))
            {
                return c * (7.5625f * t * t) + b;
            }
            else if (t < (2 / 2.75f))
            {
                float postFix = t -= (1.5f / 2.75f);
                return c * (7.5625f * (postFix) * t + .75f) + b;
            }
            else if (t < (2.5 / 2.75))
            {
                float postFix = t -= (2.25f / 2.75f);
                return c * (7.5625f * (postFix) * t + .9375f) + b;
            }
            else
            {
                float postFix = t -= (2.625f / 2.75f);
                return c * (7.5625f * (postFix) * t + .984375f) + b;
            }
        }

        public static float BounceEaseInOut(float t, float b, float c, float d)
        {
            if (t < d / 2) return BounceEaseIn(t * 2, 0, c, d) * .5f + b;
            else return BounceEaseOut(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
        }

        public static float CircEaseIn(float t, float b, float c, float d)
        {
            return (float)(-c * (Math.Sqrt(1 - (t /= d) * t) - 1) + b);
        }
        public static float CircEaseOut(float t, float b, float c, float d)
        {
            return (float)(c * Math.Sqrt(1 - (t = t / d - 1) * t) + b);
        }

        public static float CircEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return (float)(-c / 2 * (Math.Sqrt(1 - t * t) - 1) + b);
            return (float)(c / 2 * (Math.Sqrt(1 - t * (t -= 2)) + 1) + b);
        }

        public static float CubicEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t + b;
        }
        public static float CubicEaseOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        public static float CubicEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        public static float ElasticEaseIn(float t, float b, float c, float d)
        {
            if (t == 0) return b; if ((t /= d) == 1) return b + c;
            float p = d * .3f;
            float a = c;
            float s = p / 4;
            float postFix = (float)(a * Math.Pow(2, 10 * (t -= 1))); // this is a fix, again, with post-increment operators
            return (float)(-(postFix * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b);
        }

        public static float ElasticEaseOut(float t, float b, float c, float d)
        {
            if (t == 0) return b; if ((t /= d) == 1) return b + c;
            float p = d * .3f;
            float a = c;
            float s = p / 4;
            return (float)((a * Math.Pow(2, -10 * t) * Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b));
        }

        public static float ElasticEaseInOut(float t, float b, float c, float d)
        {
            if (t == 0) return b; if ((t /= d / 2) == 2) return b + c;
            float p = d * (.3f * 1.5f);
            float a = c;
            float s = p / 4;

            float postFix;
            if (t < 1)
            {
                postFix = (float)(a * Math.Pow(2, 10 * (t -= 1))); // postIncrement is evil
                return (float)(-.5f * (postFix * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b);
            }
            postFix = (float)(a * Math.Pow(2, -10 * (t -= 1))); // postIncrement is evil
            return (float)(postFix * Math.Sin((t * d - s) * (2 * Math.PI) / p) * .5f + c + b);
        }

        public static float ExpoEaseIn(float t, float b, float c, float d)
        {
            return (float)((t == 0) ? b : c * Math.Pow(2, 10 * (t / d - 1)) + b);
        }

        public static float ExpoEaseOut(float t, float b, float c, float d)
        {
            return (float)((t == d) ? b + c : c * (-Math.Pow(2, -10 * t / d) + 1) + b);
        }

        public static float ExpoEaseInOut(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if (t == d) return b + c;
            if ((t /= d / 2) < 1) return (float)(c / 2 * Math.Pow(2, 10 * (t - 1)) + b);
            return (float)(c / 2 * (-Math.Pow(2, -10 * --t) + 2) + b);
        }

        public static float LinearEaseNone(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public static float LinearEaseIn(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public static float LinearEaseOut(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public static float LinearEaseInOut(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public static float QuadEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t + b;
        }
        public static float QuadEaseOut(float t, float b, float c, float d)
        {
            return -c * (t /= d) * (t - 2) + b;
        }

        public static float QuadEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return ((c / 2) * (t * t)) + b;
            return -c / 2 * (((t - 2) * (--t)) - 1) + b;
            /*
            originally return -c/2 * (((t-2)*(--t)) - 1) + b;

            I've had to swap (--t)*(t-2) due to diffence in behaviour in 
            pre-increment operators between java and c++, after hours
            of joy
            */

        }

        public static float QuartEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t + b;
        }
        public static float QuartEaseOut(float t, float b, float c, float d)
        {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        public static float QuartEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        public static float QuintEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t * t + b;
        }
        public static float QuintEaseOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        public static float QuintEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        public static float SineEaseIn(float t, float b, float c, float d)
        {
            return (float)(-c * Math.Cos(t / d * (Math.PI / 2)) + c + b);
        }
        public static float SineEaseOut(float t, float b, float c, float d)
        {
            return (float)(c * Math.Sin(t / d * (Math.PI / 2)) + b);
        }

        public static float SineEaseInOut(float t, float b, float c, float d)
        {
            return (float)(-c / 2 * (Math.Cos(Math.PI * t / d) - 1) + b);
        }
    }
}
