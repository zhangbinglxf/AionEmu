using System;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace Aion.Emu.LoginService
{
	public class LoginRewardDAO : DAO
	{
		public LoginReward LoadLoginReward(int accountId)
		{
			MySqlConnection connection = DAO.GetConnection();
			LoginReward result = null;
			try
			{
				connection.Open();
				string cmdText = "SELECT * FROM login_reward WHERE account_id = ?account_id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_id", accountId);
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
				if (mySqlDataReader.Read())
				{
					int @int = mySqlDataReader.GetInt32("reward_id");
					int int2 = mySqlDataReader.GetInt32("reward_index");
					DateTime dateTime = mySqlDataReader.GetDateTime("lastday_reward");
					DateTime dateTime2 = mySqlDataReader.GetDateTime("day_reward");
					DateTime dateTime3 = mySqlDataReader.GetDateTime("birthday_reward");
					result = new LoginReward(accountId, @int, int2, dateTime, dateTime2, dateTime3);
					return result;
				}
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

		public void InsertLoginReward(LoginReward reward)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "INSERT INTO login_reward(account_id, reward_id, reward_index, lastday_reward, day_reward, birthday_reward) VALUES (?account_id, ?reward_id, ?reward_index, ?lastday_reward, ?day_reward, ?birthday_reward)";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?account_id", reward.AccountId);
				mySqlCommand.Parameters.AddWithValue("?reward_id", reward.RewardID);
				mySqlCommand.Parameters.AddWithValue("?reward_index", reward.RewardIndex);
				mySqlCommand.Parameters.AddWithValue("?lastday_reward", reward.LastRewardTime);
				mySqlCommand.Parameters.AddWithValue("?day_reward", reward.NextRewardTime);
				mySqlCommand.Parameters.AddWithValue("?birthday_reward", reward.NextBirthDayTime);
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

		public void UpdataLoginReward(LoginReward reward)
		{
			MySqlConnection connection = DAO.GetConnection();
			try
			{
				connection.Open();
				string cmdText = "UPDATE login_reward SET reward_id=?reward_id, reward_index = ?reward_index, lastday_reward = ?lastday_reward, day_reward = ?day_reward, birthday_reward = ?birthday_reward WHERE account_id = ?account_id";
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, connection);
				mySqlCommand.Parameters.AddWithValue("?reward_id", reward.RewardID);
				mySqlCommand.Parameters.AddWithValue("?reward_index", reward.RewardIndex);
				mySqlCommand.Parameters.AddWithValue("?lastday_reward", reward.LastRewardTime);
				mySqlCommand.Parameters.AddWithValue("?day_reward", reward.NextRewardTime);
				mySqlCommand.Parameters.AddWithValue("?birthday_reward", reward.NextBirthDayTime);
				mySqlCommand.Parameters.AddWithValue("?account_id", reward.AccountId);
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
