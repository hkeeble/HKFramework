using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
