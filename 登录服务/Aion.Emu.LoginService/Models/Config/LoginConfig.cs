using System;
using Aion.Emu.Common;

namespace Aion.Emu.LoginService
{
	public class LoginConfig
	{
		[MyProperty(Key = "loginserver.client.version", DefaultValue = "5.x")]
		public static string CLIENT_VERSION;

		[MyProperty(Key = "loginserver.network.client.port", DefaultValue = "2106")]
		public static ushort LOGIN_PORT;

		[MyProperty(Key = "loginserver.network.game.port", DefaultValue = "9014")]
		public static ushort GAME_PORT;

		[MyProperty(Key = "loginserver.accounts.autocreate", DefaultValue = "True")]
		public static bool ACCOUNT_AUTO_CREATION;

		[MyProperty(Key = "loginserver.accounts.regex", DefaultValue = "[a-zA-Z0-9]{4,16}")]
		public static string ACCOUNT_CREATION_REGEX;

		[MyProperty(Key = "loginserver.encode.password", DefaultValue = "True")]
		public static bool ENCODE_PASSWORD;

		[MyProperty(Key = "loginserver.password.error.check", DefaultValue = "True")]
		public static bool PASSWORD_CHECK_ENABLE;

		[MyProperty(Key = "loginserver.password.error.checktime", DefaultValue = "10")]
		public static int PASSWORD_CHECK_TIME;

		[MyProperty(Key = "loginserver.password.error.count", DefaultValue = "5")]
		public static int PASSWORD_CHECK_COUNT;

		[MyProperty(Key = "loginserver.password.error.bantime", DefaultValue = "15")]
		public static int PASSWORD_CHECK_BANTIME;

		[MyProperty(Key = "workingmemoryset.enable", DefaultValue = "True")]
		public static bool WORKING_MEMORY_SET_ENABLE;

		[MyProperty(Key = "workingmemoryset.cron", DefaultValue = "0 0 0 ? * *")]
		public static string WORKING_MEMORY_SET_CRON;

		[MyProperty(Key = "loginserver.display.unknownpacket", DefaultValue = "False")]
		public static bool UNKNOWN_PACKET;

		[MyProperty(Key = "loginserver.slbip.url", DefaultValue = "UNKNOWN")]
		public static string ALIYUN_SLB_ENDPOINT;
	}
}
