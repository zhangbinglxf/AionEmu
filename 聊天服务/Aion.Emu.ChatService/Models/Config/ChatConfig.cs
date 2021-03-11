using Aion.Emu.Common;

namespace Aion.Emu.ChatService
{
	public class ChatConfig
	{
		[MyProperty(Key = "chatserver.client.version", DefaultValue = "5.x")]
		public static string CLIENT_VERSION;

		[MyProperty(Key = "gameserver.channel.port", DefaultValue = "9021")]
		public static short GAME_CHANNEL_PORT;

		[MyProperty(Key = "chatserver.network.ip", DefaultValue = "127.0.0.1")]
		public static string CHAT_IP;

		[MyProperty(Key = "chatserver.network.port", DefaultValue = "10241")]
		public static ushort CHAT_PORT;

		[MyProperty(Key = "chatserver.crossserver.enable", DefaultValue = "False")]
		public static bool CROSS_SERVER_ENABLE;

		[MyProperty(Key = "chatserver.workingmemoryset.enable", DefaultValue = "True")]
		public static bool WORKING_MEMORY_SET_ENABLE;

		[MyProperty(Key = "chatserver.workingmemoryset.cron", DefaultValue = "0 0 0 ? * *")]
		public static string WORKING_MEMORY_SET_CRON;

		[MyProperty(Key = "chatserver.slbip.url", DefaultValue = "UNKNOWN")]
		public static string ALIYUN_SLB_ENDPOINT;

		[MyProperty(Key = "chatserver.display.unknownpacket", DefaultValue = "False")]
		public static bool UNKNOWN_PACKET;
	}
}
