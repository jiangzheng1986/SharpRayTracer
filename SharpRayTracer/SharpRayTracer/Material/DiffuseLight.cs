using System;

namespace SharpRayTracer
{
    class DiffuseLight : Material
    {
        public Texture _Emit;

        public DiffuseLight(Texture Emit)
        {
            _Emit = Emit;
        }
        public DiffuseLight(Color Emit)
        {
            _Emit = new SolidColorTexture(Emit);
        }

        public override bool Scatter(Random Random, Ray Ray, HitRecord HitRecord, ref Color Attenuation, ref Ray Scattered)
        {
            return false;
        }

        public override Color Emitted(double U, double V, Vector3 P)
        {
            return _Emit.GetValue(U, V, P);
        }
    }
}
