using System;

namespace SharpRayTracer
{
    class XZRect : Hittable
    {
        public double _X0;
        public double _X1;
        public double _Z0;
        public double _Z1;
        public double _K;
        public Material _Material;

        public XZRect(double X0, double X1, double Z0, double Z1, double K, Material Material)
        {
            _X0 = X0;
            _X1 = X1;
            _Z0 = Z0;
            _Z1 = Z1;
            _K = K;
            _Material = Material;
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            double T = (_K - Ray._Origin.Y) / Ray._Direction.Y;
            if (T < TMin || T > TMax)
            {
                return false;
            }

            double X = Ray._Origin.X + T * Ray._Direction.X;
            double Z = Ray._Origin.Z + T * Ray._Direction.Z;
            if (X < _X0 || X > _X1 || Z < _Z0 || Z > _Z1)
            {
                return false;
            }

            HitRecord._U = (X - _X0) / (_X1 - _X0);
            HitRecord._V = (Z - _Z0) / (_Z1 - _Z0);
            HitRecord._T = T;

            Vector3 OutwardNormal = new Vector3(0.0, 1.0, 0.0);
            HitRecord.SetFaceNormal(Ray, OutwardNormal);
            HitRecord._Material = _Material;
            HitRecord._Position = Ray.At(T);
            return true;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            AABB._Min = new Vector3(_X0, _K - 0.0001, _Z0);
            AABB._Max = new Vector3(_X1, _K + 0.0001, _Z1);
            return true;
        }
    }
}
