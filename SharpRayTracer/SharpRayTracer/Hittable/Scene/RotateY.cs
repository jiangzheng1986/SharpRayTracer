using System;

namespace SharpRayTracer
{
    class RotateY : Hittable
    {
        Hittable _Hittable;
        double _SinTheta;
        double _CosTheta;

        public RotateY(Hittable Hittable, double Angle)
        {
            _Hittable = Hittable;
            double Radians = MathHelper.DegreesToRadians(Angle);
            _SinTheta = Math.Sin(Radians);
            _CosTheta = Math.Cos(Radians);
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            Vector3 Origin = Ray._Origin;
            Vector3 Direction = Ray._Direction;

            Origin.X = _CosTheta * Ray._Origin.X - _SinTheta * Ray._Origin.Z;
            Origin.Z = _SinTheta * Ray._Origin.X + _CosTheta * Ray._Origin.Z;

            Direction.X = _CosTheta * Ray._Direction.X - _SinTheta * Ray._Direction.Z;
            Direction.Z = _SinTheta * Ray._Direction.X + _CosTheta * Ray._Direction.Z;

            Ray RotatedRay = new Ray(Origin, Direction);

            if (!_Hittable.Hit(RotatedRay, TMin, TMax, HitRecord, Random))
            {
                return false;
            }

            Vector3 Position = HitRecord._Position;
            Vector3 Normal = HitRecord._Normal;

            Position.X = _CosTheta * HitRecord._Position.X + _SinTheta * HitRecord._Position.Z;
            Position.Z = -_SinTheta * HitRecord._Position.X + _CosTheta * HitRecord._Position.Z;

            Normal.X = _CosTheta * HitRecord._Normal.X + _SinTheta * HitRecord._Normal.Z;
            Normal.Z = -_SinTheta * HitRecord._Normal.X + _CosTheta * HitRecord._Normal.Z;

            HitRecord._Position = Position;
            HitRecord.SetFaceNormal(RotatedRay, Normal);

            return true;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            if (!_Hittable.BoundingBox(ref AABB))
            {
                return false;
            }

            double AbsMinX = Math.Abs(AABB._Min.X);
            double AbsMaxX = Math.Abs(AABB._Max.X);
            double MaxAbsX = Math.Max(AbsMinX, AbsMaxX);

            double AbsMinZ = Math.Abs(AABB._Min.Z);
            double AbsMaxZ = Math.Abs(AABB._Max.Z);
            double MaxAbsZ = Math.Max(AbsMinZ, AbsMaxZ);

            double MaxXZ = Math.Sqrt(MaxAbsX * MaxAbsX + MaxAbsZ * MaxAbsZ);

            AABB._Min = new Vector3(MaxXZ, AABB._Min.Y, MaxXZ);
            AABB._Max = new Vector3(MaxXZ, AABB._Max.Y, MaxXZ);
            return true;
        }
    }
}
