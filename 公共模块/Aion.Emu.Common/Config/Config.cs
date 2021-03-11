using log4net;

namespace Aion.Emu.Common
{
	public class Config
	{
		protected static ILog log = LogManager.GetLogger(typeof(Config));

		public static void Load()
		{
			string str = "./configs/network/";
			ConfigurableProcessor.Process(new DatabaseConfig(), str + "database.properties");
			ConfigurableProcessor.Process(new FirewallConfig(), str + "firewall.properties");
		}
	}
}
