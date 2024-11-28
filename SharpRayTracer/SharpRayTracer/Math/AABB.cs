using System;

namespace SharpRayTracer
{
    struct AABB
    {
        public Vector3 _Min;
        public Vector3 _Max;

        public AABB(Vector3 Min, Vector3 Max)
        {
            _Min = Min;
            _Max = Max;
        }

        public static AABB CreateDefaultAABB()
        {
            return new AABB(
                new Vector3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity), 
                new Vector3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity));
        }

        public bool Hit(Ray Ray, double TMin, double TMax)
        {
            Vector3 Origin = Ray._Origin;
            Vector3 Direction = Ray._Direction;
            {
                double InvDirectionX = 1.0 / Direction.X;
                double D0 = (_Min.X - Origin.X) * InvDirectionX;
                double D1 = (_Max.X - Origin.X) * InvDirectionX;
                double T0 = Math.Min(D0, D1);
                double T1 = Math.Max(D0, D1);
                TMin = Math.Max(T0, TMin);
                TMax = Math.Min(T1, TMax);
                if (TMax <= TMin)
                {
                    return false;
                }
            }
            {
                double InvDirectionY = 1.0 / Direction.Y;
                double D0 = (_Min.Y - Origin.Y) * InvDirectionY;
                double D1 = (_Max.Y - Origin.Y) * InvDirectionY;
                double T0 = Math.Min(D0, D1);
                double T1 = Math.Max(D0, D1);
                TMin = Math.Max(T0, TMin);
                TMax = Math.Min(T1, TMax);
                if (TMax <= TMin)
                {
                    return false;
                }
            }
            {
                double InvDirectionZ = 1.0 / Direction.Z;
                double D0 = (_Min.Z - Origin.Z) * InvDirectionZ;
                double D1 = (_Max.Z - Origin.Z) * InvDirectionZ;
                double T0 = Math.Min(D0, D1);
                double T1 = Math.Max(D0, D1);
                TMin = Math.Max(T0, TMin);
                TMax = Math.Min(T1, TMax);
                if (TMax <= TMin)
                {
                    return false;
                }
            }
            return true;
        }

        public AABB Combine(AABB Other)
        {
            double MinX = Math.Min(_Min.X, Other._Min.X);
            double MinY = Math.Min(_Min.Y, Other._Min.Y);
            double MinZ = Math.Min(_Min.Z, Other._Min.Z);
            Vector3 Min = new Vector3(MinX, MinY, MinZ);

            double MaxX = Math.Max(_Max.X, Other._Max.X);
            double MaxY = Math.Max(_Max.Y, Other._Max.Y);
            double MaxZ = Math.Max(_Max.Z, Other._Max.Z);
            Vector3 Max = new Vector3(MaxX, MaxY, MaxZ);

            return new AABB(Min, Max);
        }
    }
}
