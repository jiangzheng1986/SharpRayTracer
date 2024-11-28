using System;

namespace SharpRayTracer
{
    class MathHelper
    {
        public static double DegreesToRadians(double Degrees)
        {
            return Degrees * (Math.PI / 180.0);
        }

        public static double RadiansTpDegrees(double Radians)
        {
            return Radians * (180.0 / Math.PI);
        }

        public static double Clamp(double Value, double Min, double Max)
        {
            if (Value < Min)
            {
                return Min;
            }
            if (Value > Max)
            {
                return Max;
            }
            return Value;
        }

        public static double Saturate(double Value)
        {
            if (Value < 0.0)
            {
                return 0.0;
            }
            if (Value > 1.0)
            {
                return 1.0;
            }
            return Value;
        }
    }
}
