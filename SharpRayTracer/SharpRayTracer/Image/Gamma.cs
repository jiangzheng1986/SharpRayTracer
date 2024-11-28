using System;

namespace SharpRayTracer
{
    class Gamma
    {
        static double GammaCorrection(double ColorComponent)
        {
            return MathHelper.Clamp(Math.Sqrt(ColorComponent), 0.0, 0.999);
        }

        public static Color GammaCorrection(Color PixelColor)
        {
            double R = GammaCorrection(PixelColor.R);
            double G = GammaCorrection(PixelColor.G);
            double B = GammaCorrection(PixelColor.B);
            return new Color(R, G, B);
        }
    }
}
