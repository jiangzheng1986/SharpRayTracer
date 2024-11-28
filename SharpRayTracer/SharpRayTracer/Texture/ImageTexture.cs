using System.IO;

namespace SharpRayTracer
{
    class ImageTexture : Texture
    {
        int _Width;
        int _Height;
        Color[] _ImageData;

        public ImageTexture(string Filename)
        {
            LoadImage(Filename);
        }

        public override Color GetValue(double U, double V, Vector3 P)
        {
            if (_ImageData == null)
            {
                return new Color(1.0, 0.0, 1.0);
            }

            U = MathHelper.Saturate(U);
            V = 1.0 - MathHelper.Saturate(V);

            int i = (int)(U * _Width);
            int j = (int)(V * _Height);

            if (i >= _Width)
            {
                i = _Width - 1;
            }

            if (j >= _Height)
            {
                j = _Height - 1;
            }

            return _ImageData[j * _Width + i];
        }

        void LoadImage(string Filename)
        {
            if (!File.Exists(Filename))
            {
                return;
            }
            bool bFloat4 = Filename.EndsWith(".simpleimage");
            FileStream FileStream = File.Open(Filename, FileMode.Open);
            BinaryReader BinaryReader = new BinaryReader(FileStream);
            if (bFloat4 == false)
            {
                _Width = BinaryReader.ReadInt32();
                _Height = BinaryReader.ReadInt32();
                int Total = _Width * _Height;
                _ImageData = new Color[Total];
                for (int i = 0; i < Total; i++)
                {
                    double R = BinaryReader.ReadDouble();
                    double G = BinaryReader.ReadDouble();
                    double B = BinaryReader.ReadDouble();
                    _ImageData[i] = new Color(R, G, B);
                }
            }
            else 
            {
                _Width = BinaryReader.ReadInt32();
                _Height = BinaryReader.ReadInt32();
                BinaryReader.ReadInt32();  //BytesPerPixel
                int Total = _Width * _Height;
                _ImageData = new Color[Total];
                double Scale = 1.0 / 255.0;
                for (int i = 0; i < Total; i++)
                {
                    double B = BinaryReader.ReadByte() * Scale;
                    double G = BinaryReader.ReadByte() * Scale;
                    double R = BinaryReader.ReadByte() * Scale;
                    _ImageData[i] = new Color(R, G, B);
                }
            }
            BinaryReader.Close();
            FileStream.Close();
        }
    }
}
