using Aion.Emu.Common;
using log4net;

namespace Aion.Emu.LoginService
{
	public class TaskManager : AbstractFIFOPeriodicTaskManager<TestTask>
	{
		private ILog log;


		public TaskManager()
			: base(100, 100)
		{
			log = LogManager.GetLogger(typeof(TaskManager));
		}

		public override void callBack(TestTask t)
		{
			t.Run();
		}
	}
}
