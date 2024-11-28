using System;

namespace SharpRayTracer
{
    class ConstantMedium : Hittable
    {
        Hittable _Boundary;
        Material _PhaseFunction;
        double _NegInvDensity;

        public ConstantMedium(Hittable Boundary, double Density, Texture Albedo)
        {
            _Boundary = Boundary;
            _NegInvDensity = -1.0 / Density;
            _PhaseFunction = new Isotropic(Albedo);
        }

        public ConstantMedium(Hittable Boundary, double Density, Color Albedo)
        {
            _Boundary = Boundary;
            _NegInvDensity = -1.0 / Density;
            _PhaseFunction = new Isotropic(Albedo);
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            HitRecord HitRecord1 = new HitRecord();
            HitRecord HitRecord2 = new HitRecord();

            if (!_Boundary.Hit(Ray, double.NegativeInfinity, double.PositiveInfinity, HitRecord1, Random))
            {
                return false;
            }

            if (!_Boundary.Hit(Ray, HitRecord1._T + 0.0001, double.PositiveInfinity, HitRecord2, Random))
            {
                return false;
            }

            if (HitRecord1._T < TMin)
            {
                HitRecord1._T = TMin;
            }
            if (HitRecord2._T > TMax)
            {
                HitRecord2._T = TMax;
            }

            if (HitRecord1._T >= HitRecord2._T)
            {
                return false;
            }

            if (HitRecord1._T < 0.0)
            {
                HitRecord1._T = 0.0;
            }

            double RayLength = Ray._Direction.GetLength();
            double DistanceInsideBoundary = (HitRecord2._T - HitRecord1._T) * RayLength;
            double HitDistance = _NegInvDensity * Math.Log(Random.NextDouble());

            if (HitDistance > DistanceInsideBoundary)
            {
                return false;
            }

            HitRecord._T = HitRecord1._T + HitDistance / RayLength;
            HitRecord._Position = Ray.At(HitRecord._T);

            HitRecord._Normal = new Vector3(1.0, 0.0, 0.0);  //arbitrary
            HitRecord._bFrontFace = true;                    //also arbitrary
            HitRecord._Material = _PhaseFunction;

            return true;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            return _Boundary.BoundingBox(ref AABB);
        }
    }
}
