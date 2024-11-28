using System;

namespace SharpRayTracer
{
    abstract class Hittable
    {
        public abstract bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random);
        
        public abstract bool BoundingBox(ref AABB AABB);
    }
}
