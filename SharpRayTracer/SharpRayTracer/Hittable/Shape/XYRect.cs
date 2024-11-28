using System;

namespace SharpRayTracer
{
    class XYRect : Hittable
    {
        public double _X0;
        public double _X1;
        public double _Y0;
        public double _Y1;
        public double _K;
        public Material _Material;

        public XYRect(double X0, double X1, double Y0, double Y1, double K, Material Material)
        {
            _X0 = X0;
            _X1 = X1;
            _Y0 = Y0;
            _Y1 = Y1;
            _K = K;
            _Material = Material;
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            double T = (_K - Ray._Origin.Z) / Ray._Direction.Z;
            if (T < TMin || T > TMax)
            {
                return false;
            }

            double X = Ray._Origin.X + T * Ray._Direction.X;
            double Y = Ray._Origin.Y + T * Ray._Direction.Y;
            if (X < _X0 || X > _X1 || Y < _Y0 || Y > _Y1)
            {
                return false;
            }

            HitRecord._U = (X - _X0) / (_X1 - _X0);
            HitRecord._V = (Y - _Y0) / (_Y1 - _Y0);
            HitRecord._T = T;

            Vector3 OutwardNormal = new Vector3(0.0, 0.0, 1.0);
            HitRecord.SetFaceNormal(Ray, OutwardNormal);
            HitRecord._Material = _Material;
            HitRecord._Position = Ray.At(T);
            return true;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            AABB._Min = new Vector3(_X0, _Y0, _K - 0.0001);
            AABB._Max = new Vector3(_X1, _Y1, _K + 0.0001);
            return true;
        }
    }
}
