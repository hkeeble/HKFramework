﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HKFramework
{
    #region Helper Structs
    /// <summary>
    /// Represents a range between two float values.
    /// </summary>
    public struct Range
    {
        public float Minimum;
        public float Maximum;

        public Range(float min, float max)
        {
            Minimum = min;
            Maximum = max;
        }
    }
    #endregion
}

namespace HKFramework.MathLib
{
    class MathLibrary
    {
        /// <summary>
        /// Finds a linear interpolation between two float values, given a weight.
        /// </summary>
        public static float LinearInterpolate(float a, float b, double weight)
        {
            return (float)(a * (1-weight) + b * weight);
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
    }
}