using System;

namespace SharpRayTracer
{
    class Box : Hittable
    {
        Vector3 _Min;
        Vector3 _Max;
        HittableList _Sides;

        public Box(Vector3 P0, Vector3 P1, Material Material)
        {
            _Min = P0;
            _Max = P1;

            _Sides = new HittableList();

            _Sides.Add(new XYRect(P0.X, P1.X, P0.Y, P1.Y, P1.Z, Material));
            _Sides.Add(new XYRect(P0.X, P1.X, P0.Y, P1.Y, P0.Z, Material));

            _Sides.Add(new XZRect(P0.X, P1.X, P0.Z, P1.Z, P1.Y, Material));
            _Sides.Add(new XZRect(P0.X, P1.X, P0.Z, P1.Z, P0.Y, Material));

            _Sides.Add(new YZRect(P0.Y, P1.Y, P0.Z, P1.Z, P1.X, Material));
            _Sides.Add(new YZRect(P0.Y, P1.Y, P0.Z, P1.Z, P0.X, Material));
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            return _Sides.Hit(Ray, TMin, TMax, HitRecord, Random);
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            AABB._Min = _Min;
            AABB._Max = _Max;
            return true;
        }
    }
}
