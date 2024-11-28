using System;
using System.Collections.Generic;

namespace SharpRayTracer
{
    class BVHNode : Hittable
    {
        public Hittable _Left;
        public Hittable _Right;
        public AABB _AABB;

        public BVHNode(HittableList List, Random Random)
            : this(List._Objects, Random)
        {
        }

        public BVHNode(List<Hittable> Objects, Random Random)
        {
            int Count = Objects.Count;
            if (Count >= 2)
            {
                int RandomInt = Random.Next(0, 2);
                if (RandomInt == 0)
                {
                    Objects.Sort(CompareHittableX);
                }
                else if (RandomInt == 1)
                {
                    Objects.Sort(CompareHittableY);
                }
                else
                {
                    Objects.Sort(CompareHittableZ);
                }
            }
            if (Count == 1)
            {
                _Left = Objects[0];
                _Right = null;
            }
            else if (Count == 2)
            {
                _Left = Objects[0];
                _Right = Objects[1];
            }
            else 
            {
                int Middle = Count / 2;
                List<Hittable> LeftList = new List<Hittable>(Middle);
                for (int i = 0; i < Middle; i++)
                {
                    LeftList.Add(Objects[i]);
                }
                _Left = new BVHNode(LeftList, Random);

                List<Hittable> RightList = new List<Hittable>();
                for (int i = Middle; i < Count; i++)
                {
                    RightList.Add(Objects[i]);
                }
                _Right = new BVHNode(RightList, Random);
            }

            AABB AABB1 = new AABB();
            _Left.BoundingBox(ref AABB1);
            if (_Right != null)
            {
                AABB AABB2 = new AABB();
                _Right.BoundingBox(ref AABB2);
                _AABB = AABB1.Combine(AABB2);
            }
            else 
            {
                _AABB = AABB1;
            }
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            if (!_AABB.Hit(Ray, TMin, TMax))
            {
                return false;
            }

            bool bHitLeft = _Left.Hit(Ray, TMin, TMax, HitRecord, Random);
            double TMaxNew = bHitLeft ? HitRecord._T : TMax;
            bool bHitRight = false;
            if (_Right != null)
            {
                bHitRight = _Right.Hit(Ray, TMin, TMaxNew, HitRecord, Random);
            }

            return bHitLeft || bHitRight;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            AABB = _AABB;
            return true;
        }

        static int CompareHittableX(Hittable Hittable1, Hittable Hittable2)
        {
            AABB AABB1 = new AABB();
            Hittable1.BoundingBox(ref AABB1);
            AABB AABB2 = new AABB();
            Hittable2.BoundingBox(ref AABB2);
            if (AABB1._Min.X < AABB2._Min.X)
            {
                return 1;
            }
            else if (AABB1._Min.X > AABB2._Min.X)
            {
                return -1;
            }
            else 
            {
                return 0;
            }
        }

        static int CompareHittableY(Hittable Hittable1, Hittable Hittable2)
        {
            AABB AABB1 = new AABB();
            Hittable1.BoundingBox(ref AABB1);
            AABB AABB2 = new AABB();
            Hittable2.BoundingBox(ref AABB2);
            if (AABB1._Min.Y < AABB2._Min.Y)
            {
                return 1;
            }
            else if (AABB1._Min.Y > AABB2._Min.Y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        static int CompareHittableZ(Hittable Hittable1, Hittable Hittable2)
        {
            AABB AABB1 = new AABB();
            Hittable1.BoundingBox(ref AABB1);
            AABB AABB2 = new AABB();
            Hittable2.BoundingBox(ref AABB2);
            if (AABB1._Min.Z < AABB2._Min.Z)
            {
                return 1;
            }
            else if (AABB1._Min.Z > AABB2._Min.Z)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
