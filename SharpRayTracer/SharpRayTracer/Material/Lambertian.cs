using System;

namespace SharpRayTracer
{
    class Lambertian : Material
    {
        public Texture _Albedo;

        public Lambertian(Color Albedo)
        {
            _Albedo = new SolidColorTexture(Albedo);
        }

        public Lambertian(Texture Albedo)
        {
            _Albedo = Albedo;
        }

        public override bool Scatter(Random Random, Ray Ray, HitRecord HitRecord, ref Color Attenuation, ref Ray Scattered)
        {
            //Vector3 ScatterDirection = HitRecord._Normal + RandomHelper.RandomInUnitSphere(Random);
            Vector3 ScatterDirection = HitRecord._Normal + RandomHelper.RandomUnitVector(Random);
            //Vector3 ScatterDirection = RandomHelper.RandomInHemisphere(Random, HitRecord._Normal);

            if (ScatterDirection.IsNearZero())
            {
                ScatterDirection = HitRecord._Normal;
            }

            Scattered = new Ray(HitRecord._Position, ScatterDirection);
            Attenuation = _Albedo.GetValue(HitRecord._U, HitRecord._V, HitRecord._Position);
            return true;
        }
    }
}
