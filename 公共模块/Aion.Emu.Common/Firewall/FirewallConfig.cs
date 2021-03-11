namespace Aion.Emu.Common
{
	public class FirewallConfig
	{
		[MyProperty(Key = "firewall.enable", DefaultValue = "False")]
		public static bool FIREWALL_ENABLE;

		[MyProperty(Key = "firewall.sameip.connect.maxcount", DefaultValue = "15")]
		public static int SAMEIP_MAXCOUNT;

		[MyProperty(Key = "firewall.samepacket.maxlength", DefaultValue = "8192")]
		public static int SAMEIP_PACKET_LENGTH;

		[MyProperty(Key = "firewall.inpacket.checktime", DefaultValue = "5")]
		public static int INPACKET_CHECKTIME;

		[MyProperty(Key = "firewall.cycle.inpacketlength", DefaultValue = "16384")]
		public static int CYCLE_INPACKET_LENGTH;

		[MyProperty(Key = "firewall.connect.checktime", DefaultValue = "60")]
		public static int CONNECT_CHECKTIME;

		[MyProperty(Key = "firewall.cycle.connect.maxcount", DefaultValue = "5")]
		public static int CYCLE_CONNECT_MAXCOUNT;
	}
}
