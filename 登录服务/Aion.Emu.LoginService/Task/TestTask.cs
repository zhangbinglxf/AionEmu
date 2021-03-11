using Aion.Emu.Common;
using log4net;

namespace Aion.Emu.LoginService
{
	public class TestTask : StartupHook
	{
		private ILog log;

		private int taskId;

		public TestTask()
		{
			log = LogManager.GetLogger(typeof(TestTask));
			taskId = 0;
		}

		public TestTask(int id)
		{
			log = LogManager.GetLogger(typeof(TestTask));
			taskId = 0;
			taskId = id;
		}

		public void Run()
		{
		}

		void StartupHook.Run()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Run
			this.Run();
		}
	}
}
