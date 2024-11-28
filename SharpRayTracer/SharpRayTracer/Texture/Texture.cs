namespace SharpRayTracer
{
    abstract class Texture
    {
        public abstract Color GetValue(double U, double V, Vector3 P);
    }
}
