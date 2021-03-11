using System;
using log4net;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.Common
{
	public class DAO
	{
		protected ILog log;

		private static string _conString = $"Server={DatabaseConfig.DATABASE_ADDRESS};port={DatabaseConfig.DATABASE_PORT};database={DatabaseConfig.DATABASE_NAME};user={DatabaseConfig.DATABASE_USER};password={DatabaseConfig.DATABASE_PASSWORD};charset={DatabaseConfig.DATABASE_CHARSET};";

		public DAO()
		{
			log = LogManager.GetLogger(GetType());
		}

		public static bool Connect()
		{
			try
			{
				MySqlConnection mySqlConnection = new MySqlConnection(_conString);
				mySqlConnection.Open();
				mySqlConnection.Close();
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
		}

		protected static MySqlConnection GetConnection()
		{
			return new MySqlConnection(_conString);
		}
	}
}
