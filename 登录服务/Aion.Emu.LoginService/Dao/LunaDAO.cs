using System;
using Aion.Emu.Common;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.LoginService
{
	public class LunaDAO : DAO
	{
		public LunaInfo LoadLuna(int accountId)
		{
			MySqlConnection connection = DAO.GetConnection();
			LunaInfo lunaInfo = null;
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM luna WHERE account_id = ?account_id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_id", accountId);
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
				if (mySqlDataReader.Read())
				{
					long @int = mySqlDataReader.GetInt64("luna");
					int int2 = mySqlDataReader.GetInt32("today_use");
					int int3 = mySqlDataReader.GetInt32("key_value");
					int int4 = mySqlDataReader.GetInt32("reward_id");
					DateTime dateTime = mySqlDataReader.GetDateTime("next_updata");
					if (DateTime.Compare(dateTime, DateTime.Now) < 0 && int2 > 0)
					{
						DateTime time = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, checked(DateAndTime.Now.Day + 1), 9, 0, 0);
						lunaInfo = new LunaInfo(accountId, @int, 0, int3, int4, time);
						Store(lunaInfo);
					}
					else
					{
						lunaInfo = new LunaInfo(accountId, @int, int2, int3, int4, dateTime);
					}
				}
				mySqlDataReader.Close();
				return lunaInfo;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
				return lunaInfo;
			}
			finally
			{
				connection.Close();
			}
		}

		public bool Store(LunaInfo luna)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "REPLACE INTO luna(account_id, luna, today_use, key_value, reward_id, next_updata) VALUES (?account_id, ?luna, ?today_use, ?key_value, ?reward_id, ?next_updata)";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_id", luna.AccountId);
				mySqlCommand.Parameters.AddWithValue("?luna", luna.Luna);
				mySqlCommand.Parameters.AddWithValue("?today_use", luna.TodayUse);
				mySqlCommand.Parameters.AddWithValue("?key_value", luna.Keys);
				mySqlCommand.Parameters.AddWithValue("?reward_id", luna.TodayRewardId);
				mySqlCommand.Parameters.AddWithValue("?next_updata", luna.NextTime);
				mySqlCommand.ExecuteNonQuery();
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				connection.Close();
			}
		}

		public void UpdataLuna()
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "UPDATE luna SET today_use=0, reward_id=0 WHERE today_use > 0";
				new MySqlCommand(cmdText, connection).ExecuteNonQuery();
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
