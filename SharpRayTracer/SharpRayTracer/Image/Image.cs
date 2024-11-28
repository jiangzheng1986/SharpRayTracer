using System.IO;
using System.Text;

namespace SharpRayTracer
{
    class Image
    {
        public int _Width;
        public int _Height;
        public Color[] _Data;

        public Image(int Width, int Height)
        {
            _Width = Width;
            _Height = Height;
            _Data = new Color[_Width * _Height];
        }

        public void Add(Image Image)
        {
            if (Image._Width == _Width && Image._Height == _Height)
            {
                int Count = _Width *_Height;
                Color[] ImageData = Image._Data;
                for (int i = 0; i < Count; i++)
                {
                    _Data[i] += ImageData[i];
                }
            }
        }

        public void GammaCorrection(int Samples)
        {
            double InverseSample = 1.0 / Samples;
            int Count = _Width * _Height;
            for (int i = 0; i < Count; i++)
            {
                _Data[i] = Gamma.GammaCorrection(_Data[i] * InverseSample);
            }
        }

        public void Save(string Filename)
        {
            StringBuilder StringBuilder = new StringBuilder();
            StringBuilder.Append(string.Format("P3\n{0} {1}\n255\n", _Width, _Height));
            for (int j = _Height - 1; j >= 0; --j)
            {
                for (int i = 0; i < _Width; i++)
                {
                    Color Color = _Data[j * _Width + i];
                    int R = (int)(Color.R * 256.0);
                    int G = (int)(Color.G * 256.0);
                    int B = (int)(Color.B * 256.0);
                    StringBuilder.Append(string.Format("{0} {1} {2}\n", R, G, B));
                }
            }
            File.WriteAllText(Filename, StringBuilder.ToString());
        }
    }
}
