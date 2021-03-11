using Aion.Emu.Common;

namespace Aion.Emu.LoginService
{
	public class ExpirationConfig
	{
		[MyProperty(Key = "expiration.enable", DefaultValue = "False")]
		public static bool EXPIRATION_ENABLE;

		[MyProperty(Key = "expiration.default.type", DefaultValue = "TIMING")]
		public static string EXPIRATION_TYPE;

		[MyProperty(Key = "expiration.default.value", DefaultValue = "60")]
		public static int EXPIRATION_VALUE;
	}
}
