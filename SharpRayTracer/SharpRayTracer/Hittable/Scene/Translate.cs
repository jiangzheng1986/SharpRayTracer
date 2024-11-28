using System;

namespace SharpRayTracer
{
    class Translate : Hittable
    {
        Hittable _Hittable;
        Vector3 _Offset;

        public Translate(Hittable Hittable, Vector3 Offset)
        {
            _Hittable = Hittable;
            _Offset = Offset;
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            Ray MovedRay = new Ray(Ray._Origin - _Offset, Ray._Direction);
            if (!_Hittable.Hit(MovedRay, TMin, TMax, HitRecord, Random))
            {
                return false;
            }

            HitRecord._Position += _Offset;
            HitRecord.SetFaceNormal(MovedRay, HitRecord._Normal);

            return true;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            if (!_Hittable.BoundingBox(ref AABB))
            {
                return false;
            }

            AABB._Min += _Offset;
            AABB._Max += _Offset;
            return true;
        }
    }
}
