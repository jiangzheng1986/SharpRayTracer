using System;

namespace SharpRayTracer
{
    class Metal : Material
    {
        public Color _Albedo;
        public double _Fuzz;

        public Metal(Color Albedo, double Fuzz)
        {
            _Albedo = Albedo;
            _Fuzz = Fuzz;
        }

        public override bool Scatter(Random Random, Ray Ray, HitRecord HitRecord, ref Color Attenuation, ref Ray Scattered)
        {
            Vector3 Reflected = Vector3.Reflect(Ray._Direction.GetNormalized(), HitRecord._Normal);
            Scattered = new Ray(HitRecord._Position, Reflected + _Fuzz * RandomHelper.RandomInUnitSphere(Random));
            Attenuation = _Albedo;
            return Scattered._Direction.Dot(HitRecord._Normal) > 0.0;
        }
    }
}
