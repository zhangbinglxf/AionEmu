using System.Threading;

namespace Aion.Emu.Common
{
	public abstract class AbstractPeriodicTaskManager : AbstractLockManager
	{
		private Timer _timer;

		public AbstractPeriodicTaskManager(int dueTime, int period)
		{
			_timer = new Timer(Run, null, dueTime, period);
		}

		public abstract void Run(object o);
	}
}
