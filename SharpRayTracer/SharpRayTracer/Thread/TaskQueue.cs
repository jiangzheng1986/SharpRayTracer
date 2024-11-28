using System.Collections.Generic;

namespace SharpRayTracer
{
	class TaskQueue
	{
		List<Task> _TaskQueue;
		object _TaskQueueLock;
		List<Task> _NotCompletedTaskQueue;
		object _NotCompletedTaskQueueLock;
		List<Task> _CompletedTaskQueue;
		object _CompletedTaskQueueLock;

		public TaskQueue()
		{
			_TaskQueue = new List<Task>();
			_TaskQueueLock = new object();
			_NotCompletedTaskQueue = new List<Task>();
			_NotCompletedTaskQueueLock = new object();
			_CompletedTaskQueue = new List<Task>();
			_CompletedTaskQueueLock = new object();
		}

		public void ClearTasks()
		{
			lock (_TaskQueueLock)
			{
				lock (_NotCompletedTaskQueueLock)
				{
					_TaskQueue.Clear();
					_NotCompletedTaskQueue.Clear();
				}
			}
		}

		public void AddTask(Task Task)
		{
			lock (_TaskQueueLock)
			{
				lock (_NotCompletedTaskQueueLock)
				{
					_TaskQueue.Add(Task);
					_NotCompletedTaskQueue.Add(Task);
				}
			}
		}

		public bool HasTask()
		{
			lock (_NotCompletedTaskQueueLock)
			{
				lock (_CompletedTaskQueueLock)
				{
					return _NotCompletedTaskQueue.Count > 0 || _CompletedTaskQueue.Count > 0;
				}
			}
		}

		public Task FetchTask()
		{
			lock (_TaskQueueLock)
			{
				if (_TaskQueue.Count == 0)
				{
					return null;
				}
				Task Head = _TaskQueue[0];
				_TaskQueue.RemoveAt(0);
				return Head;
			}
		}

		public void AddCompletedTask(Task Task)
		{
			lock (_NotCompletedTaskQueueLock)
			{
				lock (_CompletedTaskQueueLock)
				{
					_CompletedTaskQueue.Add(Task);
					_NotCompletedTaskQueue.Remove(Task);
				}
			}
		}

		public Task FetchCompletedTask()
		{
			lock(_CompletedTaskQueueLock)
			{
				if (_CompletedTaskQueue.Count == 0)
				{
					return null;
				}
				Task Head = _CompletedTaskQueue[0];
				_CompletedTaskQueue.RemoveAt(0);
				return Head;
			}
		}
	}
}
