using System;

namespace SharpRayTracer
{
    class Triangle
    {

        public const double Epsilon = 1e-8;

        public static bool Hit(Ray Ray, ref Vector3 V0, ref Vector3 V1, ref Vector3 V2, out double T, out double U, out double V, out double W)
        {
            T = 0.0;
            U = 0.0;
            V = 0.0;
            W = 0.0;

            Vector3 V0V1 = V1 - V0;
            Vector3 V0V2 = V2 - V0;

            Vector3 N = V0V1.Cross(V0V2);
            double Denom = N.Dot(N);

            double NDotRayDirection = N.Dot(Ray._Direction);
            if (Math.Abs(NDotRayDirection) < Epsilon)
            {
                return false;
            }

            double D = -N.Dot(V0);

            T = -(N.Dot(Ray._Origin) + D) / NDotRayDirection;
            if (T < 0.0)
            {
                return false;
            }

            Vector3 P = Ray._Origin + T * Ray._Direction;

            Vector3 Edge0 = V1 - V0;
            Vector3 V0P = P - V0;
            Vector3 C = Edge0.Cross(V0P);
            if (N.Dot(C) < 0.0)
            {
                return false;
            }

            Vector3 Edge1 = V2 - V1;
            Vector3 V1P = P - V1;
            C = Edge1.Cross(V1P);
            U = N.Dot(C);
            if (U < 0.0)
            {
                return false;
            }

            Vector3 Edge2 = V0 - V2;
            Vector3 V2P = P - V2;
            C = Edge2.Cross(V2P);
            V = N.Dot(C);
            if (V < 0.0)
            {
                return false;
            }

            U /= Denom;
            V /= Denom;
            W = 1.0 - U - V;

            return true;
        }
    }
}
