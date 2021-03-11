using System;
using System.Collections.Generic;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.LoginService
{
	public class PayInfoDAO : DAO
	{
		public List<PayInfo> LoadPayInfo()
		{
			List<PayInfo> list = new List<PayInfo>();
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM pay_infos";
				MySqlDataReader mySqlDataReader = new MySqlCommand(cmdText, connection).ExecuteReader();
				while (mySqlDataReader.Read())
				{
					string @string = mySqlDataReader.GetString("account_name");
					int @int = mySqlDataReader.GetInt32("day_pay");
					PayInfo item = new PayInfo(@string, @int);
					list.Add(item);
				}
				mySqlDataReader.Close();
				return list;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2.ToString());
				ProjectData.ClearProjectError();
				return list;
			}
			finally
			{
				connection.Close();
			}
		}

		public void DeletePay(string account)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "DELETE FROM pay_infos WHERE account_name=?account_name";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_name", account);
				mySqlCommand.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2.ToString());
				ProjectData.ClearProjectError();
			}
			finally
			{
				connection.Close();
			}
		}
	}
}
