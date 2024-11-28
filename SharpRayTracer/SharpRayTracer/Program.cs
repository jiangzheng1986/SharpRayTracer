using System;

namespace SharpRayTracer
{
    delegate void TaskDoneHandler(RayTraceTask RayTraceTask);

    class RayTraceTask : Task
    {
        int _ID;
        Hittable _World;
        Camera _Camera;
        int _ImageWidth;
        int _ImageHeight;
        Image _Image;
        Color _Background;
        int _Samples;
        int _MaxDepth;
        Random _Random;
        TaskDoneHandler _TaskDoneHandler;

        public RayTraceTask(int ID, Hittable World, Camera Camera, int ImageWidth, int ImageHeight, Color Background, int Samples, int MaxDepth, TaskDoneHandler TaskDoneHandler)
        {
            _ID = ID;
            _World = World;
            _Camera = Camera;
            _ImageWidth = ImageWidth;
            _ImageHeight = ImageHeight;
            _Background = Background;
            _Samples = Samples;
            _MaxDepth = MaxDepth;
            _TaskDoneHandler = TaskDoneHandler;
            _Random = new Random();
        }

        public int GetID()
        {
            return _ID;
        }

        public Image GetImage()
        {
            return _Image;
        }

        public int GetSamples()
        {
            return _Samples;
        }

        Color RayColor(Ray Ray, int Depth)
        {
            if (Depth <= 0)
            {
                return new Color(0.0, 0.0, 0.0);
            }

            HitRecord HitRecord = new HitRecord();
            if (!_World.Hit(Ray, 0.001, double.PositiveInfinity, HitRecord, _Random))
            {
                return _Background;
            }

            Color Emitted = HitRecord._Material.Emitted(HitRecord._U, HitRecord._V, HitRecord._Position);

            Ray Scattered = null;
            Color Attenuation = new Color();
            if (!HitRecord._Material.Scatter(_Random, Ray, HitRecord, ref Attenuation, ref Scattered))
            {
                return Emitted;
            }

            return Emitted + Attenuation * RayColor(Scattered, Depth - 1);
        }

        public override void Execute()
        {
            _Image = new Image(_ImageWidth, _ImageHeight);

            for (int j = 0; j < _ImageHeight; j++)
            {
                for (int i = 0; i < _ImageWidth; i++)
                {
                    Color PixelColor = new Color();
                    for (int s = 0; s < _Samples; s++)
                    {
                        double U = (i + _Random.NextDouble()) / (_ImageWidth - 1.0);
                        double V = (j + _Random.NextDouble()) / (_ImageHeight - 1.0);
                        Ray Ray = _Camera.GetRay(U, V);
                        PixelColor += RayColor(Ray, _MaxDepth);
                    }
                    _Image._Data[j * _ImageWidth + i] = PixelColor;
                }
            }
        }

        public override void OnTaskDone()
        {
            if (_TaskDoneHandler != null)
            {
                _TaskDoneHandler(this);
            }
        }
    }

    class Program
    {
        static void TrySaveResultImage(string Filename, Image ImageTotal, int CurrentSamples)
        {
            int ImageWidth = ImageTotal._Width;
            int ImageHeight = ImageTotal._Height;
            Image ImageResult = new Image(ImageWidth, ImageHeight);
            ImageResult.Add(ImageTotal);
            ImageResult.GammaCorrection(CurrentSamples);
            try
            {
                ImageResult.Save(Filename);
            }
            catch
            { 
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Sharp Ray Tracer V0.1");

            string Filename = "Test.ppm";

            //TestScene TestScene = new TestScene(7);
            TestScene TestScene = new TestScene(3);
            int ImageWidth = TestScene._ImageWidth;
            int ImageHeight = TestScene._ImageHeight;
            int TotalSamples = TestScene._TotalSamples;
            int MaxDepth = TestScene._MaxDepth;
            Camera Camera = TestScene._Camera;
            Hittable World = TestScene._World;
            Color Background = TestScene._Background;

            Image ImageTotal = new Image(ImageWidth, ImageHeight);
            int CurrentSamples = 0;

            TaskThreadPool TaskThreadPool = TaskThreadPool.GetInstance();

            int ProcessorCount = Environment.ProcessorCount;
            int TaskThreadCount = Math.Max(1, ProcessorCount - 1);
            TaskThreadPool.Start(TaskThreadCount);

            int SamplesPerBatch = 1;
            //int SamplesPerBatch = TotalSamples;
            int Batch = (TotalSamples + SamplesPerBatch - 1) / SamplesPerBatch;

            int SamplesProcessed = 0;
            object SamplesProcessedLock = new object();

            TaskDoneHandler TaskDoneHandler = (RayTraceTask RayTraceTask) =>
            {
                int ID = RayTraceTask.GetID();
                Image Image = RayTraceTask.GetImage();
                ImageTotal.Add(Image);
                int Samples = RayTraceTask.GetSamples();
                CurrentSamples += Samples;
                lock (SamplesProcessedLock)
                {
                    SamplesProcessed += Samples;
                    Console.WriteLine("\rSamples: {0}/{1}", SamplesProcessed, TotalSamples);
                }
            };

            for (int i = 0; i < Batch; i++)
            {
                RayTraceTask RayTraceTask = new RayTraceTask(i, World, Camera, ImageWidth, ImageHeight, Background, SamplesPerBatch, MaxDepth, TaskDoneHandler);
                TaskThreadPool.EmitTask(RayTraceTask);
            }

            long LastSaveTick = Environment.TickCount64;
            while (TaskThreadPool.HasTask())
            {
                TaskThreadPool.Update(1);
                long CurrentTick = Environment.TickCount64;
                if (CurrentTick - LastSaveTick > 10000)
                {
                    TrySaveResultImage(Filename, ImageTotal, CurrentSamples);
                    LastSaveTick = CurrentTick;
                }
            }
            TrySaveResultImage(Filename, ImageTotal, CurrentSamples);

            Console.WriteLine();
            Console.WriteLine("Done.");
        }
    }
}
