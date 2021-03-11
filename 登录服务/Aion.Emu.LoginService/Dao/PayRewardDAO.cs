using System;
using System.Collections.Generic;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.LoginService
{
	public class PayRewardDAO : DAO
	{
		public void InsertPayReward(PayReward pay)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "INSERT INTO pay_rewards(account_name, reward_type, pay_num, reward_id, read_num, start_time, end_time) VALUES (?account_name, ?reward_type, ?pay_num, ?reward_id, ?read_num, ?start_time, ?end_time) ON DUPLICATE KEY UPDATE pay_num=pay_num+?pay_num";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_name", pay.AccountName);
				mySqlCommand.Parameters.AddWithValue("?reward_type", pay.RewardType.ToString());
				mySqlCommand.Parameters.AddWithValue("?pay_num", pay.PayNum);
				mySqlCommand.Parameters.AddWithValue("?reward_id", pay.RewardId);
				mySqlCommand.Parameters.AddWithValue("?read_num", pay.ReadNum);
				mySqlCommand.Parameters.AddWithValue("?start_time", pay.StartTime);
				mySqlCommand.Parameters.AddWithValue("?end_time", pay.EndTime);
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

		public List<PayReward> LoadPayReward(string account)
		{
			List<PayReward> list = new List<PayReward>();
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM pay_rewards WHERE account_name = ?account_name";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_name", account);
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
				while (mySqlDataReader.Read())
				{
					PayRewardType rewardType = (PayRewardType)Conversions.ToInteger(Enum.Parse(typeof(PayRewardType), mySqlDataReader.GetString("reward_type")));
					int @int = mySqlDataReader.GetInt32("pay_num");
					int int2 = mySqlDataReader.GetInt32("reward_id");
					int int3 = mySqlDataReader.GetInt32("read_num");
					DateTime dateTime = mySqlDataReader.GetDateTime("start_time");
					DateTime dateTime2 = mySqlDataReader.GetDateTime("end_time");
					PayReward payReward = new PayReward(account, rewardType, int2, int3, dateTime, dateTime2);
					payReward.PayNum = @int;
					list.Add(payReward);
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

		public int GetReadPrice(PayReward reward)
		{
			MySqlConnection connection = DAO.GetConnection();
			int result = 0;
			try
			{
				connection.Open();
				string cmdText = "SELECT read_num FROM pay_rewards WHERE account_name = ?account_name And reward_id=?reward_id And start_time=?start_time And end_time=?end_time";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_name", reward.AccountName);
				mySqlCommand.Parameters.AddWithValue("?reward_id", reward.RewardId);
				mySqlCommand.Parameters.AddWithValue("?start_time", reward.StartTime);
				mySqlCommand.Parameters.AddWithValue("?end_time", reward.EndTime);
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
				if (mySqlDataReader.Read())
				{
					result = mySqlDataReader.GetInt32("read_num");
					return result;
				}
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2.ToString());
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				connection.Close();
			}
		}

		public void UpdataPayReward(PayReward reward, int price)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "UPDATE pay_rewards SET read_num=?read_num WHERE account_name = ?account_name And reward_id=?reward_id And start_time=?start_time And end_time=?end_time";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?read_num", price);
				mySqlCommand.Parameters.AddWithValue("?account_name", reward.AccountName);
				mySqlCommand.Parameters.AddWithValue("?reward_id", reward.RewardId);
				mySqlCommand.Parameters.AddWithValue("?start_time", reward.StartTime);
				mySqlCommand.Parameters.AddWithValue("?end_time", reward.EndTime);
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
