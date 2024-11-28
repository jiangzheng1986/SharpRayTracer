using System;
using System.Collections.Generic;

namespace SharpRayTracer
{
    class HittableList : Hittable
    {
        public List<Hittable> _Objects;

        public HittableList()
        {
            _Objects = new List<Hittable>();
        }

        public HittableList(List<Hittable> Objects)
        {
            _Objects = Objects;
        }

        public void Clear()
        {
            _Objects.Clear();
        }

        public void Add(Hittable Object)
        {
            _Objects.Add(Object);
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            HitRecord TempHitRecord = new HitRecord();
            bool bHitAnything = false;
            double ClosestSoFar = TMax;

            foreach (Hittable Object in _Objects)
            {
                if (Object.Hit(Ray, TMin, ClosestSoFar, TempHitRecord, Random))
                {
                    bHitAnything = true;
                    ClosestSoFar = TempHitRecord._T;
                    HitRecord.CopyData(TempHitRecord);
                }
            }

            return bHitAnything;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            if (_Objects.Count == 0)
            {
                return false;
            }

            bool bFirst = true;
            foreach (Hittable Object in _Objects)
            {
                AABB AABB1 = new AABB();
                if (!Object.BoundingBox(ref AABB1))
                {
                    return false;
                }

                if (bFirst)
                {
                    AABB = AABB1;
                }
                else
                {
                    AABB = AABB.Combine(AABB1);
                }
                bFirst = false;
            }
            
            return true;
        }
    }
}
