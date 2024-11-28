using System;

namespace SharpRayTracer
{
    class Isotropic : Material
    {
        public Texture _Albedo;

        public Isotropic(Color Albedo)
        {
            _Albedo = new SolidColorTexture(Albedo);
        }

        public Isotropic(Texture Albedo)
        {
            _Albedo = Albedo;
        }

        public override bool Scatter(Random Random, Ray Ray, HitRecord HitRecord, ref Color Attenuation, ref Ray Scattered)
        {
            Scattered = new Ray(HitRecord._Position, RandomHelper.RandomInUnitSphere(Random));
            Attenuation = _Albedo.GetValue(HitRecord._U, HitRecord._V, HitRecord._Position);
            return true;
        }
    }
}
