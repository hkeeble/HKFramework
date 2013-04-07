using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HKFramework
{
    public class Maths
    {
        /// <summary>
        /// Finds a linear interpolation between two float values, given a weight.
        /// </summary>
        public static float LinearInterpolate(float a, float b, double weight)
        {
            return (float)(a * (1-weight) + b * weight);
        }

        /// <summary>
        /// Finds a linear interpolation between two colors, given a weight.
        /// </summary>
        public static Color LinearInterpolate(Color a, Color b, double weight)
        {
            return new Color(LinearInterpolate(a.R, b.R, weight),
                                     LinearInterpolate(a.G, b.G, weight),
                                     LinearInterpolate(a.B, b.B, weight),
                                     LinearInterpolate(a.A, b.A, weight));
        }

        /// <summary>
        /// Returns the mid-point between two given vectors.
        /// </summary>
        public static Vector2 MidPoint(Vector2 a, Vector2 b)
        {
            return new Vector2((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }

        /// <summary>
        /// Returns the angle between two given vectors.
        /// </summary>
        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            a = a - b;
            b = new Vector2(0, -1);
            return MathHelper.ToDegrees(-(float)Math.Atan2((a.X - b.X), (a.Y - b.Y)));
        }

        /// <summary>
        /// Rotates a given vector by radians.
        /// </summary>
        public static void RotateVector(float radians, ref Vector2 vector)
        {
            Matrix matrix = Matrix.CreateRotationY(radians);
            vector = Vector2.Transform(vector, matrix);
        }
    }
}