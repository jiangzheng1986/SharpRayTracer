using System;

namespace SharpRayTracer
{
    class Camera
    {
        Vector3 _Origin;
        Vector3 _Horizontal;
        Vector3 _Vertical;
        Vector3 _LowerLeftCorner;

        public Camera(
            Vector3 LookFrom,
            Vector3 LookAt,
            Vector3 Up,
            double VerticalFOV, 
            double AspectRatio)
        {
            double Theta = MathHelper.DegreesToRadians(VerticalFOV);
            double H = Math.Tan(Theta * 0.5);
            double ViewportHeight = 2.0 * H;
            double ViewportWidth = AspectRatio * ViewportHeight;

            Vector3 W = (LookFrom - LookAt).GetNormalized();
            Vector3 U = (Up.Cross(W)).GetNormalized();
            Vector3 V = W.Cross(U);

            _Origin = LookFrom;
            _Horizontal = ViewportWidth * U;
            _Vertical = ViewportHeight * V;
            _LowerLeftCorner = _Origin - _Horizontal / 2.0 - _Vertical / 2.0 - W;
        }

        public Ray GetRay(double S, double T)
        {
            return new Ray(_Origin, _LowerLeftCorner + S * _Horizontal + T * _Vertical - _Origin);
        }
    }
}
