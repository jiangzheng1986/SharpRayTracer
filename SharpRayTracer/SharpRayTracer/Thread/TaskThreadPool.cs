using System.Collections.Generic;
using System.Threading;

namespace SharpRayTracer
{
    class TaskThreadPool
    {
		static TaskThreadPool _Instance = new TaskThreadPool();

		TaskQueue _TaskQueue;
        List<Thread> _ThreadList;
        bool _bToStop;

        static void TaskThreadPool_TaskThread(object Parameter)
		{
			TaskThreadPool TaskThreadPool = (TaskThreadPool)Parameter;
			TaskThreadPool.RealTaskThread();
			return;
		}

		public static TaskThreadPool GetInstance()
		{			
			return _Instance;
		}

		public TaskThreadPool()
		{
			_TaskQueue = new TaskQueue();
			_ThreadList = new List<Thread>();
			_bToStop = false;
		}

		public void Start(int ThreadCount)
		{
			if (ThreadCount <= 0)
			{
				return;
			}
			for (int i = 0; i < ThreadCount; i++)
			{
				Thread Thread = new Thread(TaskThreadPool_TaskThread);
				Thread.Start(this);
				_ThreadList.Add(Thread);
			}
		}

		public void Update(int MaxFetch)
		{
			int FetchCount = 0;
			while (true)
			{
				Task Task = _TaskQueue.FetchCompletedTask();
				if (Task != null)
				{
					Task.OnTaskDone();
					FetchCount++;
					if (FetchCount >= MaxFetch)
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
		}

		public void Stop()
		{
			_bToStop = true;

			int ThreadCount = _ThreadList.Count;
			for (int i = 0; i < ThreadCount; i++)
			{
				Thread Thread = _ThreadList[i];
				Thread.Join();
			}
		}

		public void EmitTask(Task Task)
		{
			_TaskQueue.AddTask(Task);
		}

		public void ClearTasks()
		{
			_TaskQueue.ClearTasks();
		}

		public bool HasTask()
		{
			return _TaskQueue.HasTask();
		}

		void RealTaskThread()
		{
			while (true)
			{
				if (_bToStop)
				{
					break;
				}
				while (true)
				{
					if (_bToStop)
					{
						break;
					}
					Task Task = _TaskQueue.FetchTask();
					if (Task != null)
					{
						Task.Execute();
						_TaskQueue.AddCompletedTask(Task);
					}
					else
					{
						break;
					}
				}
				Thread.Sleep(1);
			}
		}
    }
}
