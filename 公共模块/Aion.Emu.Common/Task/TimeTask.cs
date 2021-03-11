using System;
using System.Threading;

namespace Aion.Emu.Common
{
	public class TimeTask : IDisposable
	{
		private Timer _time;

		public TimeTask(TimerCallback cl, long dueTime)
		{
			_time = new Timer(cl, null, dueTime, 0L);
		}

		public void Dispose()
		{
			_time.Dispose();
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}
	}
}
