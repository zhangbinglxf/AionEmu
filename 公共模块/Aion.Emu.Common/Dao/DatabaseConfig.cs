namespace Aion.Emu.Common
{
	public class DatabaseConfig
	{
		[MyProperty(Key = "database.address", DefaultValue = "127.0.0.1")]
		public static string DATABASE_ADDRESS;

		[MyProperty(Key = "database.port", DefaultValue = "3306")]
		public static int DATABASE_PORT;

		[MyProperty(Key = "database.name", DefaultValue = "al_server_gs")]
		public static string DATABASE_NAME;

		[MyProperty(Key = "database.user", DefaultValue = "root")]
		public static string DATABASE_USER;

		[MyProperty(Key = "database.password", DefaultValue = "root")]
		public static string DATABASE_PASSWORD;

		[MyProperty(Key = "database.charset", DefaultValue = "utf8")]
		public static string DATABASE_CHARSET;
	}
}
