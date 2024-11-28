using System;
using System.Collections.Generic;
using System.IO;

namespace SharpRayTracer
{
    struct Vertex
    {
        public Vector3 _Position;
        public Vector3 _Normal;
        public double _U;
        public double _V;

        public Vertex(Vector3 Position, Vector3 Normal, double U, double V)
        {
            _Position = Position;
            _Normal = Normal;
            _U = U;
            _V = V;
        }
    }

    class Mesh : Hittable
    {
        public List<Vertex> _VertexList;
        public List<int> _IndexList;
        public AABB _AABB;
        public Material _Material;

        public Mesh(Material Material)
        {
            _VertexList = new List<Vertex>();
            _IndexList = new List<int>();
            _Material = Material;
            _AABB = AABB.CreateDefaultAABB();
        }

        Vector3 ReadVector3(BinaryReader BinaryReader)
        {
            double X = BinaryReader.ReadSingle();
            double Y = BinaryReader.ReadSingle();
            double Z = BinaryReader.ReadSingle();
            return new Vector3(X, Y, Z);
        }

        public void LoadMesh(string Filename)
        {
            _VertexList.Clear();
            _IndexList.Clear();
            if (!File.Exists(Filename))
            {
                return;
            }
            FileStream FileStream = File.Open(Filename, FileMode.Open);
            BinaryReader BinaryReader = new BinaryReader(FileStream);
            int VertexCount = BinaryReader.ReadInt32();
            for (int i = 0; i < VertexCount; i++)
            {
                Vertex Vertex = new Vertex();
                Vertex._Position = ReadVector3(BinaryReader);
                Vertex._Normal = ReadVector3(BinaryReader);
                Vertex._U = BinaryReader.ReadSingle();
                Vertex._V = 1.0 - BinaryReader.ReadSingle();
                _VertexList.Add(Vertex);
            }
            int IndexCount = BinaryReader.ReadInt32();
            for (int j = 0; j < IndexCount; j++)
            {
                int Index = BinaryReader.ReadInt16();
                _IndexList.Add(Index);
            }
            BinaryReader.Close();
            FileStream.Close();
            UpdateBoundingBox();
        }

        public void AddTriangle(Vector3 A, Vector3 B, Vector3 C)
        {
            Vector3 Normal = (B - A).Cross(C - A);
            double U = 0.0;
            double V = 0.0;
            int Index1 = _VertexList.Count;
            int Index2 = Index1 + 1;
            int Index3 = Index2 + 1;
            _VertexList.Add(new Vertex(A, Normal, U, V));
            _VertexList.Add(new Vertex(B, Normal, U, V));
            _VertexList.Add(new Vertex(C, Normal, U, V));
            _IndexList.Add(Index1);
            _IndexList.Add(Index2);
            _IndexList.Add(Index3);
        }

        public void UpdateBoundingBox(ref Vector3 V)
        {
            _AABB._Min.X = Math.Min(_AABB._Min.X, V.X);
            _AABB._Min.Y = Math.Min(_AABB._Min.Y, V.Y);
            _AABB._Min.Z = Math.Min(_AABB._Min.Z, V.Z);

            _AABB._Max.X = Math.Max(_AABB._Max.X, V.X);
            _AABB._Max.Y = Math.Max(_AABB._Max.Y, V.Y);
            _AABB._Max.Z = Math.Max(_AABB._Max.Z, V.Z);
        }

        public void UpdateBoundingBox()
        {
            _AABB = AABB.CreateDefaultAABB();
            int Count = _VertexList.Count;
            for (int i = 0; i < Count; i++)
            {
                Vector3 V = _VertexList[i]._Position;
                UpdateBoundingBox(ref V);
            }
            _AABB._Min -= new Vector3(0.0001, 0.0001, 0.0001);
            _AABB._Max += new Vector3(0.0001, 0.0001, 0.0001);
        }

        public override bool Hit(Ray Ray, double TMin, double TMax, HitRecord HitRecord, Random Random)
        {
            if (!_AABB.Hit(Ray, TMin, TMax))
            {
                return false;
            }

            bool bHit = false;
            double T = double.PositiveInfinity;
            Vector3 Normal = new Vector3();
            double TexCoordU = 0.0;
            double TexCoordV = 0.0;

            int TriangleCount = _IndexList.Count / 3;
            for (int i = 0; i < TriangleCount; i++)
            {
                int i1 = i * 3;
                int Index1 = _IndexList[i1];
                int Index2 = _IndexList[i1 + 1];
                int Index3 = _IndexList[i1 + 2];
                Vector3 A = _VertexList[Index1]._Position;
                Vector3 B = _VertexList[Index2]._Position;
                Vector3 C = _VertexList[Index3]._Position;
                double T1;
                double U;
                double V;
                double W;
                if (Triangle.Hit(Ray, ref A, ref B, ref C, out T1, out U, out V, out W))
                {
                    if (T1 >= TMin && T1 <= TMax && T1 < T)
                    {
                        bHit = true;
                        T = T1;
                        Normal = _VertexList[Index1]._Normal * U + _VertexList[Index2]._Normal * V + _VertexList[Index3]._Normal * W;
                        TexCoordU = _VertexList[Index1]._U * U + _VertexList[Index2]._U * V + _VertexList[Index3]._U * W;
                        TexCoordV = _VertexList[Index1]._V * U + _VertexList[Index2]._V * V + _VertexList[Index3]._V * W;
                    }
                }
            }

            if (!bHit)
            {
                return false;
            }

            HitRecord._T = T;
            HitRecord._Position = Ray.At(HitRecord._T);
            HitRecord.SetFaceNormal(Ray, Normal);
            HitRecord._U = TexCoordU;
            HitRecord._V = TexCoordV;
            HitRecord._Material = _Material;

            return true;
        }

        public override bool BoundingBox(ref AABB AABB)
        {
            AABB = _AABB;
            return true;
        }
    }
}
