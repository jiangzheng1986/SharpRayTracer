using System;

namespace SharpRayTracer
{
    class TestScene
    {
        public int _ImageWidth;
        public int _ImageHeight;
        public int _TotalSamples;
        public int _MaxDepth;
        public Hittable _World;
        public Camera _Camera;
        public Color _Background;

        private double _AspectRatio;
        private double _VerticalFOV;
        private Vector3 _LookFrom;
        private Vector3 _LookAt;
        private Vector3 _Up;

        public TestScene(int SceneID)
        {
            _ImageWidth = 400;
            _ImageHeight = 400;
            _TotalSamples = 100;
            _MaxDepth = 50;

            _Background = new Color(0.0, 0.0, 0.0);

            _AspectRatio = 3.0 / 2.0;
            _VerticalFOV = 40.0;

            _LookFrom = new Vector3(13.0, 2.0, 3.0);
            _LookAt = new Vector3(0.0, 0.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            
            Random Random = new Random();
            switch (SceneID)
            {
                case 1:
                    CreateTestScene1(Random);
                    break;

                case 2:
                    CreateTestScene2(Random);
                    break;

                case 3:
                    CreateTestScene3(Random);                    
                    break;

                case 4:
                    CreateTestScene4(Random);
                    break;

                case 5:
                    CreateTestScene5(Random);
                    break;

                case 6:
                    CreateTestScene6(Random);
                    break;

                case 7:
                    CreateTestScene7(Random);
                    break;

                default:
                    int i = 0;
                    i = i / i;
                    break;
            }

            _ImageHeight = (int)(_ImageWidth / _AspectRatio);
            _Camera = new Camera(_LookFrom, _LookAt, _Up, _VerticalFOV, _AspectRatio);
        }

        void CreateTestScene1(Random Random)
        {
            HittableList World = new HittableList();
            CheckerTexture CheckerTexture = new CheckerTexture(new Color(0.2, 0.3, 0.1), new Color(0.9, 0.9, 0.9));
            Material MaterialGround = new Lambertian(CheckerTexture);
            World.Add(new Sphere(new Vector3(0.0, -1000.0, 0.0), 1000.0, MaterialGround));

            for (int A = -11; A < 11; A++)
            {
                for (int B = -11; B < 11; B++)
                {
                    double X = A + 0.9 * Random.NextDouble();
                    double Y = 0.2;
                    double Z = B + 0.9 * Random.NextDouble();
                    Vector3 Center = new Vector3(X, Y, Z);
                    Material SphereMaterial = null;
                    double ChooseMaterial = Random.NextDouble();
                    if (ChooseMaterial < 0.8)
                    {
                        Color Albedo = RandomHelper.RandomColor(Random) * RandomHelper.RandomColor(Random);
                        SphereMaterial = new Lambertian(Albedo);
                    }
                    else if (ChooseMaterial < 0.95)
                    {
                        Color Albedo = RandomHelper.RandomColorRange(Random, 0.5, 1.0);
                        double Fuzz = RandomHelper.RandomRange(Random, 0.0, 0.5);
                        SphereMaterial = new Metal(Albedo, Fuzz);
                    }
                    else
                    {
                        SphereMaterial = new Dielectric(1.5);
                    }
                    World.Add(new Sphere(Center, 0.2, SphereMaterial));
                }
            }

            Material Material1 = new Dielectric(1.5);
            World.Add(new Sphere(new Vector3(0.0, 1.0, 0.0), 1.0, Material1));

            Material Material2 = new Lambertian(new Color(0.4, 0.2, 0.1));
            World.Add(new Sphere(new Vector3(-4.0, 1.0, 0.0), 1.0, Material2));

            Material Material3 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
            World.Add(new Sphere(new Vector3(4.0, 1.0, 0.0), 1.0, Material3));

            _World = World;

            _LookFrom = new Vector3(13.0, 2.0, 3.0);
            _LookAt = new Vector3(0.0, 0.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            _AspectRatio = 16.0 / 9.0;
            _VerticalFOV = 20.0;
            _Background = new Color(0.7, 0.8, 1.0);
        }

        void CreateTestScene2(Random Random)
        {
            HittableList World = new HittableList();

            CheckerTexture CheckerTexture = new CheckerTexture(new Color(0.2, 0.3, 0.1), new Color(0.9, 0.9, 0.9));
            Material MaterialGround = new Lambertian(CheckerTexture);
            World.Add(new Sphere(new Vector3(0.0, -1000.0, 0.0), 1000.0, MaterialGround));

            Material Material1 = new Lambertian(new ImageTexture("earthmap.tex"));
            World.Add(new Sphere(new Vector3(0.0, 2.0, 0.0), 2.0, Material1));

            Material Material2 = new DiffuseLight(new Color(4.0, 4.0, 4.0));
            World.Add(new Sphere(new Vector3(0.0, 5.0, 0.0), 2.0, Material2));

            World.Add(new XYRect(3, 5, 1, 3, -2, Material2));

            _World = World;

            _LookFrom = new Vector3(26.0, 3.0, 6.0);
            _LookAt = new Vector3(0.0, 0.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            _AspectRatio = 16.0 / 9.0;
            _VerticalFOV = 20.0;
            _Background = new Color(0.0, 0.0, 0.0);
        }

        void CreateTestScene3(Random Random)
        {
            HittableList World = new HittableList();

            Material MaterialRed = new Lambertian(new Color(0.65, 0.05, 0.05));
            Material MaterialWhite = new Lambertian(new Color(0.73, 0.73, 0.73));
            Material MaterialGreen = new Lambertian(new Color(0.12, 0.45, 0.15));
            Material MaterialLight = new DiffuseLight(new Color(15, 15, 15));

            World.Add(new YZRect(0, 555, 0, 555, 555, MaterialGreen));
            World.Add(new YZRect(0, 555, 0, 555, 0, MaterialRed));
            World.Add(new XZRect(213, 343, 227, 332, 554, MaterialLight));
            World.Add(new XZRect(0, 555, 0, 555, 0, MaterialWhite));
            World.Add(new XZRect(0, 555, 0, 555, 555, MaterialWhite));
            World.Add(new XYRect(0, 555, 0, 555, 555, MaterialWhite));

            Hittable Box1 = new Box(new Vector3(0, 0, 0), new Vector3(165, 330, 165), MaterialWhite);
            Box1 = new RotateY(Box1, 15);
            Box1 = new Translate(Box1, new Vector3(265, 0, 295));
            World.Add(Box1);

            Hittable Box2 = new Box(new Vector3(0, 0, 0), new Vector3(165, 165, 165), MaterialWhite);
            Box2 = new RotateY(Box2, -18);
            Box2 = new Translate(Box2, new Vector3(130, 0, 65));
            World.Add(Box2);

            _World = World;

            _ImageWidth = 600;
            _AspectRatio = 1.0;
            _TotalSamples = 200;
            _LookFrom = new Vector3(278.0, 278.0, -800.0);
            _LookAt = new Vector3(278.0, 278.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            _VerticalFOV = 40.0;
            _Background = new Color(0.0, 0.0, 0.0);
        }

        void CreateTestScene4(Random Random)
        {
            HittableList World = new HittableList();

            Material MaterialRed = new Lambertian(new Color(0.65, 0.05, 0.05));
            Material MaterialWhite = new Lambertian(new Color(0.73, 0.73, 0.73));
            Material MaterialGreen = new Lambertian(new Color(0.12, 0.45, 0.15));
            Material MaterialLight = new DiffuseLight(new Color(15, 15, 15));

            World.Add(new YZRect(0, 555, 0, 555, 555, MaterialGreen));
            World.Add(new YZRect(0, 555, 0, 555, 0, MaterialRed));
            World.Add(new XZRect(213, 343, 227, 332, 554, MaterialLight));
            World.Add(new XZRect(0, 555, 0, 555, 0, MaterialWhite));
            World.Add(new XZRect(0, 555, 0, 555, 555, MaterialWhite));
            World.Add(new XYRect(0, 555, 0, 555, 555, MaterialWhite));

            Hittable Box1 = new Box(new Vector3(0, 0, 0), new Vector3(165, 330, 165), MaterialWhite);
            Box1 = new RotateY(Box1, 15);
            Box1 = new Translate(Box1, new Vector3(265, 0, 295));
            Box1 = new ConstantMedium(Box1, 0.01, new Color(0.0, 0.0, 0.0));
            World.Add(Box1);

            Hittable Box2 = new Box(new Vector3(0, 0, 0), new Vector3(165, 165, 165), MaterialWhite);
            Box2 = new RotateY(Box2, -18);
            Box2 = new Translate(Box2, new Vector3(130, 0, 65));
            Box2 = new ConstantMedium(Box2, 0.01, new Color(1.0, 1.0, 1.0));
            World.Add(Box2);

            _World = World;

            _ImageWidth = 600;
            _AspectRatio = 1.0;
            _TotalSamples = 200;
            _LookFrom = new Vector3(278.0, 278.0, -800.0);
            _LookAt = new Vector3(278.0, 278.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            _VerticalFOV = 40.0;
            _Background = new Color(0.0, 0.0, 0.0);
        }

        void CreateTestScene5(Random Random)
        {
            HittableList World = new HittableList();

            Material MaterialGround = new Lambertian(new Color(0.48, 0.83, 0.53));

            HittableList Objects = new HittableList();
            int BoxesPerSide = 20;
            for (int i = 0; i < BoxesPerSide; i++)
            {
                for (int j = 0; j < BoxesPerSide; j++)
                {
                    double W = 100.0;
                    double X0 = -1000.0 + i * W;
                    double Y0 = 0.0;
                    double Z0 = -1000.0 + j * W;
                    double X1 = X0 + W;
                    double Y1 = RandomHelper.RandomRange(Random, 1.0, 101.0);
                    double Z1 = Z0 + W;

                    Objects.Add(new Box(new Vector3(X0, Y0, Z0), new Vector3(X1, Y1, Z1), MaterialGround));
                }
            }

            Material MaterialLight = new DiffuseLight(new Color(7, 7, 7));
            Objects.Add(new XZRect(123, 423, 147, 412, 554, MaterialLight));

            Objects.Add(new Sphere(new Vector3(400, 400, 200), 50, new Lambertian(new Color(0.7, 0.3, 0.1))));

            Objects.Add(new Sphere(new Vector3(260, 150, 45), 50, new Dielectric(1.5)));

            Objects.Add(new Sphere(new Vector3(0, 150, 145), 50, new Metal(new Color(0.8, 0.8, 0.9), 1.0)));

            Hittable Boundary1 = new Sphere(new Vector3(360, 150, 145), 70, new Dielectric(1.5));
            ConstantMedium Medium1 = new ConstantMedium(Boundary1, 0.2, new Color(0.2, 0.4, 0.9));
            Objects.Add(Boundary1);
            Objects.Add(Medium1);

            Hittable Boundary2 = new Sphere(new Vector3(0, 0, 0), 5000, new Dielectric(1.5));
            ConstantMedium Medium2 = new ConstantMedium(Boundary2, 0.0001, new Color(1.0, 1.0, 1.0));
            Objects.Add(Medium2);

            ImageTexture ImageTexture1 = new ImageTexture("earthmap.tex");
            Objects.Add(new Sphere(new Vector3(400, 200, 400), 100, new Lambertian(ImageTexture1)));

            Material MaterialWhite = new Lambertian(new Color(0.73, 0.73, 0.73));
            int SphereCount = 1000;
            for (int k = 0; k < SphereCount; k++)
            {
                Vector3 Position = new Vector3(-100.0, 270.0, 395.0) + RandomHelper.RandomVector3(Random, 0.0, 165.0);
                Objects.Add(new Sphere(Position, 10.0, MaterialWhite));
            }

            //World.Add(Objects);
            World.Add(new BVHNode(Objects, Random));

            _World = World;

            _ImageWidth = 800;
            _AspectRatio = 1.0;
            _TotalSamples = 1000;
            _LookFrom = new Vector3(478.0, 278.0, -600.0);
            _LookAt = new Vector3(278.0, 278.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            _VerticalFOV = 40.0;
            _Background = new Color(0.0, 0.0, 0.0);
        }

        void CreateTestScene6(Random Random)
        {
            HittableList World = new HittableList();

            CheckerTexture CheckerTexture = new CheckerTexture(new Color(0.2, 0.3, 0.1), new Color(0.9, 0.9, 0.9));
            Material MaterialGround = new Lambertian(CheckerTexture);
            World.Add(new Sphere(new Vector3(0.0, -1000.0, 0.0), 1000.0, MaterialGround));

            Material MaterialLight = new DiffuseLight(new Color(4.0, 4.0, 4.0));

            Mesh Mesh1 = new Mesh(MaterialLight);
            Mesh1.AddTriangle(new Vector3(0.0, 1.0, 0.0), new Vector3(0.0, 2.0, 0.0), new Vector3(0.0, 1.0, 1.0));
            Mesh1.UpdateBoundingBox();
            World.Add(Mesh1);

            World.Add(new Sphere(new Vector3(0.0, 5.0, 0.0), 2.0, MaterialLight));

            ImageTexture ImageTexture1 = new ImageTexture("Knight.simpleimage");
            Material MaterialTexture1 = new Lambertian(ImageTexture1);
            World.Add(new XYRect(3, 8, 1, 6, -2, MaterialTexture1));

            _World = World;

            _ImageWidth = 800;
            _LookFrom = new Vector3(26.0, 3.0, 6.0);
            _LookAt = new Vector3(0.0, 0.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            _AspectRatio = 16.0 / 9.0;
            _VerticalFOV = 20.0;
            _Background = new Color(0.0, 0.0, 0.0);
        }

        void CreateTestScene7(Random Random)
        {
            HittableList World = new HittableList();

            Material MaterialLight = new DiffuseLight(new Color(100.0, 100.0, 100.0));

            World.Add(new Sphere(new Vector3(0.0, 15.0, 0.0), 2.0, MaterialLight));

            ImageTexture ImageTexture1 = new ImageTexture("Knight.simpleimage");
            Material MaterialTexture1 = new Lambertian(ImageTexture1);
            Mesh Mesh1 = new Mesh(MaterialTexture1);
            Mesh1.LoadMesh("Knight.simplemodel");
            World.Add(Mesh1);

            _World = World;

            _ImageWidth = 800;
            _TotalSamples = 10000;
            _LookFrom = new Vector3(46.0, 46.0, 46.0);
            _LookAt = new Vector3(0.0, 0.0, 0.0);
            _Up = new Vector3(0.0, 1.0, 0.0);
            _AspectRatio = 16.0 / 9.0;
            _VerticalFOV = 20.0;
            _Background = new Color(0.0, 0.0, 0.0);
        }
    }
}
