using Microsoft.Xna.Framework;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EVCMonoGame.src
{
    public static class Utility
    {
        // RegularExpression for ReplaceWhitespace()
        private static readonly Regex sWhitespace = new Regex(@"\s+");

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

        public static Vector2 scaleVectorTo(Vector2 originalVector, float toVectorLength)
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

        public static Vector2 rotateVectorAntiClockwise(Vector2 originalVector, float angleInDegree)
        {
            Vector2 rotatedVector = new Vector2(0, 0);
            float r = (float)Math.Sqrt(Math.Pow(originalVector.X, 2) + Math.Pow(originalVector.Y, 2));
            float phi = (float)Math.Atan2(originalVector.Y, originalVector.X);
            float rotationScale = (float)Math.PI * (1 / (180 / angleInDegree));
            rotatedVector.X = (float)(r * Math.Cos(phi + rotationScale));
            rotatedVector.Y = (float)(r * Math.Sin(phi + rotationScale));
            return rotatedVector;
        }

        public static float getAngleOfVectorInDegrees(Vector2 originalVector)//noch nicht sicher ob es funktioniert
        {   
            float phi = (float)(180*(1/(Math.PI/Math.Atan2(-originalVector.Y, originalVector.X))));// -Y um berechnung in normales mathematisches Koordinatensystem umzusetzen
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

            Vector2 frameOffset = Vector2.Zero;
            frameOffset.X = int.Parse(vecString.Substring(1, indexComma - 1));
            frameOffset.Y = int.Parse(vecString.Substring(indexComma + 1,
                                      (vecString.Length - 2) - (indexComma)));

            return frameOffset;
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
    }
}
