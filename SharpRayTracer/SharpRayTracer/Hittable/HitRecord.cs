namespace SharpRayTracer
{
    class HitRecord
    {
        public Vector3 _Position;
        public Vector3 _Normal;
        public double _T;
        public double _U;
        public double _V;
        public bool _bFrontFace;
        public Material _Material;

        public void SetFaceNormal(Ray Ray, Vector3 OutwardNormal)
        {
            _bFrontFace = Ray._Direction.Dot(OutwardNormal) < 0.0;
            _Normal = _bFrontFace ? OutwardNormal : -OutwardNormal;
        }

        public void CopyData(HitRecord HitRecord)
        {
            _Position = HitRecord._Position;
            _Normal = HitRecord._Normal;
            _T = HitRecord._T;
            _U = HitRecord._U;
            _V = HitRecord._V;
            _bFrontFace = HitRecord._bFrontFace;
            _Material = HitRecord._Material;
        }
    }
}
