using System;

namespace SharpRayTracer
{
    abstract class Material
    {
        public abstract bool Scatter(Random Random, Ray Ray, HitRecord HitRecord, ref Color Attenuation, ref Ray Scattered);

        public virtual Color Emitted(double U, double V, Vector3 P)
        {
            return new Color(0.0, 0.0, 0.0);
        }
    }
}
