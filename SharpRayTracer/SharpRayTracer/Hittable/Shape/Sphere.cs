using System;

namespace SharpRayTracer
{
    class Sphere : Hittable
    {
        public Vector3 _Center;
        public double _Radius;
        public Material _Material;

        public Sphere(Vector3 Center, double Radius, Material Material)
        {
            _Center = Center;
            _Radius = Radius;
            _Material = Material;
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            Vector3 OC = Ray._Origin - _Center;
            double A = Ray._Direction.GetLengthSquared();
            double HalfB = OC.Dot(Ray._Direction);
            double C = OC.GetLengthSquared() - _Radius * _Radius;
            
            double Discriminant = HalfB * HalfB - A * C;
            if (Discriminant < 0.0)
            {
                return false;
            }

            double SqrtDiscriminant = Math.Sqrt(Discriminant);

            double Root = (-HalfB - SqrtDiscriminant) / A;
            if (Root < TMin || Root > TMax)
            {
                Root = (-HalfB + SqrtDiscriminant) / A;
                if (Root < TMin || Root > TMax)
                {
                    return false;
                }
            }

            HitRecord._T = Root;
            HitRecord._Position = Ray.At(HitRecord._T);
            Vector3 OutwardNormal = (HitRecord._Position - _Center) / _Radius;
            HitRecord.SetFaceNormal(Ray, OutwardNormal);
            GetSphereUV(OutwardNormal, out HitRecord._U, out HitRecord._V);
            HitRecord._Material = _Material;

            return true;
        }

        static void GetSphereUV(Vector3 P, out double U, out double V)
        {
            double Theta = Math.Acos(-P.Y);
            double Phi = Math.Atan2(-P.Z, P.X) + Math.PI;

            U = Phi / (2 * Math.PI);
            V = Theta / Math.PI;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            Vector3 RadiusVector = new Vector3(_Radius, _Radius, _Radius);
            AABB._Min = _Center - RadiusVector;
            AABB._Max = _Center + RadiusVector;
            return true;
        }
    }
}
