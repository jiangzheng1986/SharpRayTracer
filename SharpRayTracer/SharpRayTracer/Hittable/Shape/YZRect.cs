using System;

namespace SharpRayTracer
{
    class YZRect : Hittable
    {
        public double _Y0;
        public double _Y1;
        public double _Z0;
        public double _Z1;
        public double _K;
        public Material _Material;

        public YZRect(double Y0, double Y1, double Z0, double Z1, double K, Material Material)
        {
            _Y0 = Y0;
            _Y1 = Y1;
            _Z0 = Z0;
            _Z1 = Z1;
            _K = K;
            _Material = Material;
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            double T = (_K - Ray._Origin.X) / Ray._Direction.X;
            if (T < TMin || T > TMax)
            {
                return false;
            }

            double Y = Ray._Origin.Y + T * Ray._Direction.Y;
            double Z = Ray._Origin.Z + T * Ray._Direction.Z;
            if (Y < _Y0 || Y > _Y1 || Z < _Z0 || Z > _Z1)
            {
                return false;
            }

            HitRecord._U = (Y - _Y0) / (_Y1 - _Y0);
            HitRecord._V = (Z - _Z0) / (_Z1 - _Z0);
            HitRecord._T = T;

            Vector3 OutwardNormal = new Vector3(1.0, 0.0, 0.0);
            HitRecord.SetFaceNormal(Ray, OutwardNormal);
            HitRecord._Material = _Material;
            HitRecord._Position = Ray.At(T);
            return true;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            AABB._Min = new Vector3(_K - 0.0001, _Y0, _Z0);
            AABB._Max = new Vector3(_K + 0.0001, _Y1, _Z1);
            return true;
        }
    }
}
