using Aion.Emu.Common;
using log4net;

namespace Aion.Emu.LoginService
{
	public class Configs
	{
		private static ILog log = LogManager.GetLogger(typeof(Configs));

		public static void LoadConfig()
		{
			Config.Load();
			string str = "./configs/";
			ConfigurableProcessor.Process(new LoginConfig(), str + "loginserver.properties");
			ConfigurableProcessor.Process(new ExpirationConfig(), str + "expiration.properties");
		}
	}
}
