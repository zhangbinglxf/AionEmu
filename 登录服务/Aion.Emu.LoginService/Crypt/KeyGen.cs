using System;
using log4net;
using ManagedOpenSsl.NetCore.Crypto;

namespace Aion.Emu.LoginService
{
	public class KeyGen
	{
		private static ILog log = LogManager.GetLogger(typeof(KeyGen));

		private static Random Rnd = new Random();

		private static RsaCrypt[] rsa = new RsaCrypt[10];

		public static void Initial()
		{
			log.Info((object)"初始化注册机.");
			checked
			{
				int num = rsa.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					rsa[i] = new RsaCrypt(new RSA());
				}
			}
		}

		public static RsaCrypt NextRsaCrypt()
		{
			return rsa[Rnd.Next(0, 9)];
		}
	}
}
