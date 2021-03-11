using System;
using System.Runtime.CompilerServices;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.LoginService
{
	public class BannedIpDAO : DAO
	{
		public void LoadBanedIP(ref object banips)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM banned_ip";
				MySqlDataReader mySqlDataReader = new MySqlCommand(cmdText, connection).ExecuteReader();
				while (mySqlDataReader.Read())
				{
					string @string = mySqlDataReader.GetString("mask");
					DateTime dateTime = mySqlDataReader.GetDateTime("time_end");
					object[] array;
					bool[] array2;
					NewLateBinding.LateCall(banips, null, "Add", array = new object[2]
					{
						@string,
						new BannedIP(@string, dateTime)
					}, null, null, array2 = new bool[2]
					{
						true,
						false
					}, IgnoreReturn: true);
					if (array2[0])
					{
						@string = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(string));
					}
				}
				mySqlDataReader.Close();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
			}
			finally
			{
				connection.Close();
			}
		}

		public BannedIP InsertBanIp(string ip, DateTime time)
		{
			MySqlConnection connection = DAO.GetConnection();
			BannedIP result = null;
			try
			{
				connection.Open();
				string cmdText = "REPLACE INTO banned_ip(mask, time_end) VALUES (?mask, ?time_end)";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?mask", ip);
				mySqlCommand.Parameters.AddWithValue("?time_end", time);
				mySqlCommand.ExecuteNonQuery();
				result = new BannedIP(ip, time);
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				connection.Close();
			}
		}

		public void DeleteBannedIp(string ip)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "DELETE FROM banned_ip WHERE mask = ?mask";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?mask", ip);
				mySqlCommand.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
			}
			finally
			{
				connection.Close();
			}
		}
	}
}
