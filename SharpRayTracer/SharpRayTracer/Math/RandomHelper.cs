using System;

namespace SharpRayTracer
{
    class RandomHelper
    {
        public static double RandomRange(Random Random, double Min, double Max)
        {
            return Min + Random.NextDouble() * (Max - Min);
        }

        public static Vector3 RandomVector3(Random Random, double Min, double Max)
        {
            return new Vector3(RandomRange(Random, Min, Max), RandomRange(Random, Min, Max), RandomRange(Random, Min, Max));
        }

        public static Vector3 RandomInUnitSphere(Random Random)
        {
            while (true)
            {
                Vector3 Vector = RandomVector3(Random, -1.0, 1.0);
                if (Vector.GetLengthSquared() >= 1.0)
                {
                    continue;
                }
                return Vector;
            }
        }

        public static Vector3 RandomUnitVector(Random Random)
        {
            return RandomInUnitSphere(Random).GetNormalized();
        }

        public static Vector3 RandomInHemisphere(Random Random, Vector3 Normal)
        {
            Vector3 InUnitSphere = RandomInUnitSphere(Random);
            if (InUnitSphere.Dot(Normal) > 0.0)
            {
                return InUnitSphere;
            }
            else 
            {
                return -InUnitSphere;
            }
        }

        public static Color RandomColorRange(Random Random, double Min, double Max)
        {
            return new Color(RandomRange(Random, Min, Max), RandomRange(Random, Min, Max), RandomRange(Random, Min, Max));
        }

        public static Color RandomColor(Random Random)
        {
            return RandomColorRange(Random, 0.0, 1.0);
        }
    }
}
