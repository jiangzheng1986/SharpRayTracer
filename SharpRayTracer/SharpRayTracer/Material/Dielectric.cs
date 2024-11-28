using System;

namespace SharpRayTracer
{
    class Dielectric : Material
    {
        public double _IndexOfRefraction;

        public Dielectric(double IndexOfRefraction)
        {
            _IndexOfRefraction = IndexOfRefraction;
        }

        public override bool Scatter(Random Random, Ray Ray, HitRecord HitRecord, ref Color Attenuation, ref Ray Scattered)
        {
            Attenuation = new Color(1.0, 1.0, 1.0);

            double RefractionRatio = HitRecord._bFrontFace ? (1.0 / _IndexOfRefraction) : _IndexOfRefraction;
            Vector3 UnitDirection = Ray._Direction.GetNormalized();

            double CosTheta = Math.Min(-UnitDirection.Dot(HitRecord._Normal), 1.0);
            double SinTheta = Math.Sqrt(1.0 - CosTheta * CosTheta);

            bool CannotRefract = RefractionRatio * SinTheta > 1.0;
            Vector3 Direction;
            if (CannotRefract || Reflectance(CosTheta, RefractionRatio) > Random.NextDouble())
            {
                Direction = Vector3.Reflect(UnitDirection, HitRecord._Normal);
            }
            else
            {
                Direction = Vector3.Refract(UnitDirection, HitRecord._Normal, RefractionRatio);
            }

            Scattered = new Ray(HitRecord._Position, Direction);
            return true;
        }

        static double Reflectance(double Cosine, double RefractionRatio)
        {
            double R0 = (1.0 - RefractionRatio) / (1.0 + RefractionRatio);
            R0 = R0 * R0;
            return R0 + (1.0 - R0) * Math.Pow((1.0 - Cosine), 5.0);
        }
    }
}
