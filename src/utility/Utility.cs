using Microsoft.Xna.Framework;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;


namespace EVCMonoGame.src.utility
{
    public static class Utility
    {
        public delegate Vector2 PathModificationFunction(Vector2 from, Vector2 to, Vector2 current, List<float> factors);

        // RegularExpression for ReplaceWhitespace()
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        private static Random random = new Random();

        /// <summary>
        /// Returns a new string which is equal to input except that all occurences of whitespace
        /// are replaced with replacement.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }

        /// <summary>
        /// Returns Vector2 with absolute coordinates.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 AbsoluteVector(Vector2 v)
        {
            return new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
        }

        public static Vector2 ScaleVectorTo(Vector2 originalVector, float toVectorLength)
        {
            Vector2 scaledVector = new Vector2(0, 0);
            if (!(originalVector.X == 0 && originalVector.Y == 0))
            {
                double phi = Math.Atan2(originalVector.Y, originalVector.X); //--calculates the direction of the vector as an angel in degrees
                scaledVector.X = (float)(toVectorLength * Math.Cos(phi));    //--takes the angel and the wished length of the vector
                scaledVector.Y = (float)(toVectorLength * Math.Sin(phi));    //  and calculates the new X and the new Y for the scaled vector
            }
            return scaledVector;
        }

        public static Vector2 RotateVectorAntiClockwise(Vector2 originalVector, float angleInDegree)
        {
            Vector2 rotatedVector = new Vector2(0, 0);
            float r = (float)Math.Sqrt(Math.Pow(originalVector.X, 2) + Math.Pow(originalVector.Y, 2));
            float phi = (float)Math.Atan2(originalVector.Y, originalVector.X);
            float rotationScale = (float)Math.PI * (1 / (180 / angleInDegree));
            rotatedVector.X = (float)(r * Math.Cos(phi + rotationScale));
            rotatedVector.Y = (float)(r * Math.Sin(phi + rotationScale));
            return rotatedVector;
        }

        public static float GetAngleOfVectorInDegrees(Vector2 originalVector)//noch nicht sicher ob es funktioniert
        {
            float phi = (float)(180 * (1 / (Math.PI / Math.Atan2(-originalVector.Y, originalVector.X))));// -Y um berechnung in normales mathematisches Koordinatensystem umzusetzen
            //Console.WriteLine(phi);
            return phi;
        }

        /// <summary>
        /// Converts a String of format "(x, y)" to a Vector2 instance.
        /// </summary>
        /// <param name="vecString"></param>
        /// <returns></returns>
        public static Vector2 StringToVector2(String vecString)
        {
            int indexComma = vecString.IndexOf(',');

            Vector2 vector = Vector2.Zero;
            vector.X = int.Parse(vecString.Substring(1, indexComma - 1));
            vector.Y = int.Parse(vecString.Substring(indexComma + 1,
                                      (vecString.Length - 2) - (indexComma)));

            return vector;
        }

        /// <summary>
        /// Converts a String of format "(x, y, width, height)" to a Rectangle instance.
        /// </summary>
        /// <param name="recString"></param>
        /// <returns></returns>
        public static Rectangle StringToRectangle(String recString)
        {
            Rectangle rectangle = new Rectangle();

            int indexFirstComma = recString.IndexOf(',');
            int indexSecondComma = recString.IndexOf(',', indexFirstComma + 1);
            int indexThirdComma = recString.IndexOf(',', indexSecondComma + 1);

            rectangle.X = int.Parse(recString.Substring(1, indexFirstComma - 1));
            rectangle.Y = int.Parse(recString.Substring(indexFirstComma + 1, (indexSecondComma - 1) - indexFirstComma));
            rectangle.Width = int.Parse(recString.Substring(indexSecondComma + 1, (indexThirdComma - 1) - indexSecondComma));
            rectangle.Height = int.Parse(recString.Substring(indexThirdComma + 1, recString.Length - (indexThirdComma + 2)));

            return rectangle;
        }

        public static String RectangleToString(Rectangle rectangle)
        {
            return "(" + rectangle.X + ", " + rectangle.Y + ", " + rectangle.Width + ", " + rectangle.Height + ")";
        }

        // C# Mod rechnet nicht wie gewünscht
        public static int Mod(int a, int n)
        {
            if (n != 0)
            {
                int result = a % n;
                if ((result < 0 && n > 0) || (result > 0 && n < 0))
                {
                    result += n;
                }
                return result;
            }
            else
                return 0;
        }

        /// <summary>
        /// Takes a directionVector and returns the Orientation.
        /// If the directionVector equals Vector.zero it returns Orientation.NONE
        /// </summary>
        public static Orientation GetOrientation(Vector2 directionVector)
        {
            if (directionVector == Vector2.Zero) return Orientation.NONE;
            float vectorAngle = GetAngleOfVectorInDegrees(directionVector);
            if (vectorAngle > (-22.5) && vectorAngle <= (22.5))
            {
                return Orientation.RIGHT;
            }
            if (vectorAngle > (22.5) && vectorAngle <= (77.5))
            {
                return Orientation.UP_RIGHT;
            }
            if (vectorAngle > (77.5) && vectorAngle <= (112.5))
            {
                return Orientation.UP;
            }
            if (vectorAngle > (112.5) && vectorAngle <= (157.5))
            {
                return Orientation.UP_LEFT;
            }
            if ((vectorAngle > (157.5) && vectorAngle <= (180)) || (vectorAngle >= (-180) && vectorAngle <= (-157.5)))
            {
                return Orientation.LEFT;
            }
            if (vectorAngle > (-157.5) && vectorAngle <= (-112.5))
            {
                return Orientation.DOWN_LEFT;
            }
            if (vectorAngle > (-112.5) && vectorAngle <= (-77.5))
            {
                return Orientation.DOWN;
            }
            if (vectorAngle > (-77.5) && vectorAngle <= (-22.5))
            {
                return Orientation.DOWN_RIGHT;
            }
            return Orientation.NONE;
        }

        public static Rectangle CalcMinimalBoundingBox(List<Vector2> points)
        {
            Vector2 upperLeft = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 lowerRight = new Vector2(float.MinValue, float.MinValue);

            foreach (Vector2 point in points)
            {
                // upperLeft
                if (point.X < upperLeft.X)
                    upperLeft.X = point.X;
                if (point.Y < upperLeft.Y)
                    upperLeft.Y = point.Y;

                // lowerRight
                if (point.X > lowerRight.X)
                    lowerRight.X = point.X;
                if (point.Y > lowerRight.Y)
                    lowerRight.Y = point.Y;
            }

            return new Rectangle(upperLeft.ToPoint(), (lowerRight - upperLeft).ToPoint());
        }

        public static float CalcSurfaceArea(Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }

		/// <summary>
		/// factors needs one Element stating the flatness of the curve
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="current"></param>
		/// <param name="factors"></param>
		/// <returns></returns>
		public static Vector2 HalfCircle(Vector2 from, Vector2 to, Vector2 current, List<float> factors)//Vector2 from, Vector2 to, Vector2 current, float flatnessFactor
        {
            float flatnessFactor = factors.ElementAt(0);
            float radius = (to - from).Length() * 0.5f;
            float x = (current - from).Length();
            float y = (float)Math.Sqrt(Math.Pow(radius, 2) - Math.Pow((x - radius), 2));
            y *= Math.Abs(flatnessFactor);
            Console.WriteLine("X = " + x + " Y = " + y + " radius = " + radius);
            Vector2 a = new Vector2(0, y);
            a = Utility.RotateVectorAntiClockwise(a, 90 + Utility.GetAngleOfVectorInDegrees(to - from));

            return a;
        }

        public static Vector2 None(Vector2 from, Vector2 to, Vector2 current, List<float> factors)
        {
            return Vector2.Zero;
        }

        /// <summary>
        /// Randomizes all Elements fo the given list from index startIndex(including) to index endIndex(including).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public static void RandomizeList<T>(List<T> list, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex >= list.Count)
            {
                throw new ArgumentException("StartIndex was smaller 0 or endIndex was bigger,equal list.Count.");
            }

            int n = endIndex;
            while (n > startIndex)
            {
                n--;
                int k = random.Next(startIndex, n + 2);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}