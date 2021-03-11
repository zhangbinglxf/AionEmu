using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public class TimeThread
	{
		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static TimeThread instance;

		private static List<Timer> longTask = new List<Timer>();

		private TimeThread()
		{
		}

		public static TimeThread GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new TimeThread();
				}
				return instance;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(@lock);
				}
			}
		}

		public Timer Schedule(TimerCallback cl, long dueTime)
		{
			return new Timer(cl, null, dueTime, -1L);
		}

		public object LongSchedule(TimerCallback cl, long dueTime, long period)
		{
			Timer timer = new Timer(cl, null, dueTime, period);
			longTask.Add(timer);
			return timer;
		}
	}
}
