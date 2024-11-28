namespace SharpRayTracer
{
    class Ray
    {
        public Vector3 _Origin;
        public Vector3 _Direction;

        public Ray(Vector3 Origin, Vector3 Direction)
        {
            _Origin = Origin;
            _Direction = Direction;
        }

        public Vector3 At(double t)
        {
            return _Origin + _Direction * t;
        }
    }
}
