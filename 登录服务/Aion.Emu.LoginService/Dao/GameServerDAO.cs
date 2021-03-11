using System;
using System.Collections.Generic;
using Aion.Emu.Common;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class GameServerDAO : DAO
	{
		public Dictionary<int, GameInfo> LoadGameservers()
		{
			Dictionary<int, GameInfo> dictionary = new Dictionary<int, GameInfo>();
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM gameservers";
				MySqlDataReader mySqlDataReader = new MySqlCommand(cmdText, connection).ExecuteReader();
				while (mySqlDataReader.Read())
				{
					int @int = mySqlDataReader.GetInt32("id");
					string @string = mySqlDataReader.GetString("mask");
					string string2 = mySqlDataReader.GetString("password");
					dictionary.Add(@int, new GameInfo(@int, @string, string2));
				}
				mySqlDataReader.Close();
				return dictionary;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
				return dictionary;
			}
			finally
			{
				connection.Close();
			}
		}
	}
}
