using System;

namespace SharpRayTracer
{
    struct Vector3
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public static Vector3 operator -(Vector3 Vector1)
        {
            return new Vector3(-Vector1.X, -Vector1.Y, -Vector1.Z);
        }

        public static Vector3 operator +(Vector3 Vector1, Vector3 Vector2)
        {
            return new Vector3(Vector1.X + Vector2.X, Vector1.Y + Vector2.Y, Vector1.Z + Vector2.Z);
        }

        public static Vector3 operator -(Vector3 Vector1, Vector3 Vector2)
        {
            return new Vector3(Vector1.X - Vector2.X, Vector1.Y - Vector2.Y, Vector1.Z - Vector2.Z);
        }

        public static Vector3 operator *(Vector3 Vector1, double F)
        {
            return new Vector3(Vector1.X * F, Vector1.Y * F, Vector1.Z * F);
        }

        public static Vector3 operator *(double F, Vector3 Vector1)
        {
            return new Vector3(Vector1.X * F, Vector1.Y * F, Vector1.Z * F);
        }

        public static Vector3 operator /(Vector3 Vector1, double F)
        {
            return new Vector3(Vector1.X / F, Vector1.Y / F, Vector1.Z / F);
        }

        public double GetLengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public double GetLength()
        {
            return Math.Sqrt(GetLengthSquared());
        }

        public Vector3 GetNormalized()
        {
            double Length = this.GetLength();
            if (Length == 0.0)
            {
                return new Vector3();
            }
            double InverseLength = 1.0 / Length;
            double X = this.X * InverseLength;
            double Y = this.Y * InverseLength;
            double Z = this.Z * InverseLength;
            return new Vector3(X, Y, Z);
        }

        public double Dot(Vector3 Other)
        {
            return X * Other.X + Y * Other.Y + Z * Other.Z;
        }

        public Vector3 Cross(Vector3 Other)
        {
            double X1 = Y * Other.Z - Z * Other.Y;
            double Y1 = Z * Other.X - X * Other.Z;
            double Z1 = X * Other.Y - Y * Other.X;
            return new Vector3(X1, Y1, Z1);
        }

        public bool IsNearZero()
        {
            const double S = 1e-8;
            return Math.Abs(X) < S &&
                   Math.Abs(Y) < S &&
                   Math.Abs(Z) < S;
        }

        public static Vector3 Reflect(Vector3 V, Vector3 N)
        {
            return V - 2 * V.Dot(N) * N;
        }

        public static Vector3 Refract(Vector3 UV, Vector3 N, double EtaiOverEtat)
        {
            double CosTheta = Math.Min(-UV.Dot(N), 1.0);
            Vector3 RayOutPerp = EtaiOverEtat * (UV + CosTheta * N);
            Vector3 RayOutParallel = -Math.Sqrt(Math.Abs(1.0 - RayOutPerp.GetLengthSquared())) * N;
            return RayOutPerp + RayOutParallel;
        }
    }
}
