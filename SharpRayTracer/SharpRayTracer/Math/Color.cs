namespace SharpRayTracer
{
    struct Color
    {
        public double R;
        public double G;
        public double B;

        public Color(double R, double G, double B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public static Color operator +(Color Color1, Color Color2)
        {
            return new Color(Color1.R + Color2.R, Color1.G + Color2.G, Color1.B + Color2.B);
        }

        public static Color operator -(Color Color1, Color Color2)
        {
            return new Color(Color1.R - Color2.R, Color1.G - Color2.G, Color1.B - Color2.B);
        }

        public static Color operator *(Color Color1, Color Color2)
        {
            return new Color(Color1.R * Color2.R, Color1.G * Color2.G, Color1.B * Color2.B);
        }

        public static Color operator *(Color Color1, double F)
        {
            return new Color(Color1.R * F, Color1.G * F, Color1.B * F);
        }

        public static Color operator *(double F, Color Color1)
        {
            return new Color(Color1.R * F, Color1.G * F, Color1.B * F);
        }

        public static Color operator /(Color Color1, double F)
        {
            return new Color(Color1.R / F, Color1.G / F, Color1.B / F);
        }

        public static Color Lerp(Color Color1, Color Color2, double T)
        {
            return (1.0 - T) * Color1 + T * Color2;
        }
    }
}
