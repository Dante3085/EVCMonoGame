using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EvcMonogame.src
{
    public static class Utility
    {
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

        public static Vector2 rotateVectorAntiClockwise(Vector2 originalVector, float angelInDegree){
            Vector2 rotatedVector = new Vector2(0,0);
            float r = Math.Sqrt((originalVector.X^2)+(originalVector.Y^2));
            float phi = Math.Atan2(originalVector.Y, originalVector.X);
            float rotationScale = Math.PI*(1/(180/angelInDegree));
            rotatedVector.X = (float)(r * Math.Cos(phi+rotationScale));
            rotatedVector.Y = (float)(r * Math.Sin(phi+rotationScale));
            return rotatedVector;
        }
    }
}
