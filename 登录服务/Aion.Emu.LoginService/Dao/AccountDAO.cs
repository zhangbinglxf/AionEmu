using System;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.LoginService
{
	public class AccountDAO : DAO
	{
		public Account LoadAccount(string name)
		{
			Account account = null;
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM account_data WHERE name = ?name";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?name", name);
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
				if (mySqlDataReader.Read())
				{
					int @int = mySqlDataReader.GetInt32("id");
					account = new Account(@int, name);
					account.Password = mySqlDataReader.GetString("password");
					account.Activated = mySqlDataReader.GetByte("activated");
					account.AccessLevel = mySqlDataReader.GetByte("access_level");
					account.MemberShipExp = mySqlDataReader.GetInt64("membership_exp");
					account.LastServer = mySqlDataReader.GetByte("last_server");
					account.LastIp = mySqlDataReader.GetString("last_ip");
					account.Toll = mySqlDataReader.GetInt64("toll");
				}
				mySqlDataReader.Close();
				return account;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				Account result = null;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				connection.Close();
			}
		}

		internal bool UpdataAccount(Account account)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "UPDATE account_data SET name = ?name, password = ?password, access_level = ?access_level, membership_exp = ?membership_exp, last_server = ?last_server, last_ip = ?last_ip WHERE id = ?id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?name", account.Name);
				mySqlCommand.Parameters.AddWithValue("?password", account.Password);
				mySqlCommand.Parameters.AddWithValue("?access_level", account.AccessLevel);
				mySqlCommand.Parameters.AddWithValue("?membership_exp", account.MemberShipExp);
				mySqlCommand.Parameters.AddWithValue("?last_server", account.LastServer);
				mySqlCommand.Parameters.AddWithValue("?last_ip", account.LastIp);
				mySqlCommand.Parameters.AddWithValue("?id", account.Id);
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

		internal bool InsertAccount(Account acc)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "INSERT INTO account_data(name, password) VALUES (?name, ?password)";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?name", acc.Name);
				mySqlCommand.Parameters.AddWithValue("?password", acc.Password);
				mySqlCommand.ExecuteNonQuery();
				acc.Id = checked((int)mySqlCommand.LastInsertedId);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)"账号创建失败!", ex2);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				connection.Close();
			}
		}

		internal void UpdateLastIpAndServer(Account acc)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "UPDATE account_data SET last_server=?last_server, last_ip = ?last_ip WHERE id = ?id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?last_server", acc.LastServer);
				mySqlCommand.Parameters.AddWithValue("?last_ip", acc.LastIp);
				mySqlCommand.Parameters.AddWithValue("?id", acc.Id);
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

		internal bool UpdataToll(int accId, long points, long required)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "UPDATE account_data SET toll=?toll WHERE id = ?id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?toll", checked(points - required));
				mySqlCommand.Parameters.AddWithValue("?id", accId);
				mySqlCommand.ExecuteNonQuery();
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
			return true;
		}

		internal long GetTollPoint(int accountId)
		{
			long result = 0L;
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "SELECT toll FROM account_data WHERE id = ?id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?id", accountId);
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
				if (mySqlDataReader.Read())
				{
					result = mySqlDataReader.GetInt64("toll");
				}
				mySqlDataReader.Close();
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
	}
}
