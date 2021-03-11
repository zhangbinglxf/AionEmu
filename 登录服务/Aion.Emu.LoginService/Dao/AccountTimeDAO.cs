using System;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.LoginService
{
	public class AccountTimeDAO : DAO
	{
		public void InsertAccountTime(int id, AccountTime accTime)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "INSERT INTO account_time(account_id, create_time, last_active, expiration_type, expiration_time, expiration_period) VALUES (?account_id, ?create_time, ?last_active, ?expiration_type, ?expiration_time, ?expiration_period)";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_id", id);
				mySqlCommand.Parameters.AddWithValue("?create_time", accTime.CreateTime);
				mySqlCommand.Parameters.AddWithValue("?last_active", accTime.LastLoginTime);
				mySqlCommand.Parameters.AddWithValue("?expiration_type", accTime.ExpirationType.ToString());
				mySqlCommand.Parameters.AddWithValue("?expiration_time", accTime.ExpirationTime);
				mySqlCommand.Parameters.AddWithValue("?expiration_period", accTime.ExpirationPeriod);
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

		internal void UpdataAccountTime(Account acc)
		{
		}

		internal void UpdataLoginTime(Account account)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "UPDATE account_time SET last_active=?last_active WHERE account_id = ?account_id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?last_active", account.AccountTime.LastLoginTime);
				mySqlCommand.Parameters.AddWithValue("?account_id", account.Id);
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

		internal AccountTime LoadAccountTime(int id)
		{
			AccountTime accountTime = new AccountTime(id);
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM account_time WHERE account_id = ?account_id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_id", id);
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
				if (mySqlDataReader.Read())
				{
					accountTime.CreateTime = mySqlDataReader.GetDateTime("create_time");
					accountTime.LastLoginTime = mySqlDataReader.GetDateTime("last_active");
					accountTime.ExpirationType = (ExpirationType)Conversions.ToInteger(Enum.Parse(typeof(ExpirationType), mySqlDataReader.GetString("expiration_type"), ignoreCase: true));
					accountTime.ExpirationTime = mySqlDataReader.GetUInt32("expiration_time");
					accountTime.ExpirationPeriod = mySqlDataReader.GetDateTime("expiration_period");
				}
				mySqlDataReader.Close();
				return accountTime;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
				return accountTime;
			}
			finally
			{
				connection.Close();
			}
		}
	}
}
