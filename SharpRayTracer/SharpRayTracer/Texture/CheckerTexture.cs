using System;

namespace SharpRayTracer
{
    class CheckerTexture : Texture
    {
        public Texture _Odd;
        public Texture _Even;

        public CheckerTexture(Texture Odd, Texture Even)
        {
            _Odd = Odd;
            _Even = Even;
        }

        public CheckerTexture(Color Odd, Color Even)
        {
            _Odd = new SolidColorTexture(Odd);
            _Even = new SolidColorTexture(Even);
        }

        public override Color GetValue(double U, double V, Vector3 P)
        {
            double Sines = Math.Sin(10.0 * P.X) * Math.Sin(10.0 * P.Y) * Math.Sin(10.0 * P.Z);
            if (Sines < 0.0)
            {
                return _Odd.GetValue(U, V, P);
            }
            else 
            {
                return _Even.GetValue(U, V, P);
            }
        }
    }
}
