using System.Collections.Generic;

namespace Aion.Emu.Common
{
	public abstract class AbstractFIFOPeriodicTaskManager<T> : AbstractPeriodicTaskManager where T : StartupHook
	{
		private Queue<T> tasks;

		public AbstractFIFOPeriodicTaskManager(int dueTime, int period)
			: base(dueTime, period)
		{
			tasks = new Queue<T>();
		}

		public void Add(T t)
		{
			tasks.Enqueue(t);
		}

		public override void Run(object o)
		{
			ReadLock();
			try
			{
				while (tasks.Count > 0)
				{
					T t = tasks.Dequeue();
					callBack(t);
				}
			}
			finally
			{
				UnReadLock();
			}
		}

		public abstract void callBack(T t);
	}
}
