namespace SharpRayTracer
{
    class SolidColorTexture : Texture
    {
        public Color _ColorValue;

        public SolidColorTexture(Color ColorValue)
        {
            _ColorValue = ColorValue;
        }

        public SolidColorTexture(double R, double G, double B)
        {
            _ColorValue = new Color(R, G, B);
        }

        public override Color GetValue(double U, double V, Vector3 P)
        {
            return _ColorValue;
        }
    }
}
