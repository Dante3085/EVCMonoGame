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

        public static Vector2 rotateVectorAntiClockwise(Vector2 originalVector, float angelInDegree)
        {
            Vector2 rotatedVector = new Vector2(0, 0);
            float r = (float)Math.Sqrt(Math.Pow(originalVector.X, 2) + Math.Pow(originalVector.Y, 2));
            float phi = (float)Math.Atan2(originalVector.Y, originalVector.X);
            float rotationScale = (float)Math.PI * (1 / (180 / angelInDegree));
            rotatedVector.X = (float)(r * Math.Cos(phi + rotationScale));
            rotatedVector.Y = (float)(r * Math.Sin(phi + rotationScale));
            return rotatedVector;
        }

        public static float getAngelOfVectorInDegrees(Vector2 originalVector)//noch nicht sicher ob es funktioniert
        {   
            float phi = (float)(180*(1/(Math.PI/Math.Atan2(originalVector.Y, originalVector.X))));
            return 0;
        }


    }
}
