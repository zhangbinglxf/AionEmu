using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class AccountController
	{
		private static Regex NamePattern = new Regex(LoginConfig.ACCOUNT_CREATION_REGEX);

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static Dictionary<int, AionConnection> accountOnLS = new Dictionary<int, AionConnection>();

		private static Dictionary<int, ReConnectionAccount> reconnectingAccounts = new Dictionary<int, ReConnectionAccount>();

		internal static Account LoadAccount(string name)
		{
			Account account = null;
			account = DAOManager.AccountDAO.LoadAccount(name);
			if (!Information.IsNothing(account))
			{
				account.AccountTime = DAOManager.AccountTimeDAO.LoadAccountTime(account.Id);
				account.LoginReward = DAOManager.LoginRewardDAO.LoadLoginReward(account.Id);
				account.LunaInfo = DAOManager.LunaDAO.LoadLuna(account.Id);
				if (Information.IsNothing(account.LoginReward))
				{
					DateTime lastTime = DateAndTime.Now.AddDays(-1.0);
					DateTime dayTime = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day, 8, 0, 0);
					DateTime birthDayTime = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day);
					account.LoginReward = new LoginReward(account.Id, 0, 0, lastTime, dayTime, birthDayTime);
					DAOManager.LoginRewardDAO.InsertLoginReward(account.LoginReward);
				}
				if (Information.IsNothing(account.LunaInfo))
				{
					DateTime time = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day, 9, 0, 0);
					account.LunaInfo = new LunaInfo(account.Id, 0L, 0, 0, 0, time);
					DAOManager.LunaDAO.Store(account.LunaInfo);
				}
			}
			return account;
		}

		internal static void RemoveAccountOnLS(Account _account)
		{
			if (accountOnLS.ContainsKey(_account.Id))
			{
				accountOnLS.Remove(_account.Id);
			}
		}

		internal static AionAuthResponse Login(string userName, string password, AionConnection client)
		{
			Account account = LoadAccount(userName);
			if (Information.IsNothing(account) & LoginConfig.ACCOUNT_AUTO_CREATION)
			{
				account = CreateAccount(userName, password);
			}
			if (Information.IsNothing(account))
			{
				return AionAuthResponse.INVALID_PASSWORD;
			}
			if (!account.Password.Equals(RuntimeHelpers.GetObjectValue(EncodePassword(password))))
			{
				if (LoginConfig.PASSWORD_CHECK_ENABLE)
				{
					BruteForceProtector.AddFailedLogin(userName, client.IP);
				}
				return AionAuthResponse.INVALID_PASSWORD;
			}
			if (account.Activated != 1)
			{
				return AionAuthResponse.BAN_ACCOUNT;
			}
			string text = client.IP.Split(':')[0];
			if (BannedIpController.IsBannedIp(text))
			{
				return AionAuthResponse.BAN_IP;
			}
			if (!GameService.HahServerOnline())
			{
				return AionAuthResponse.NOT_SERVER_ONLINE;
			}
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (GameService.ContainsAccount(account.Id))
				{
					GameService.KickAccountFromGameServer(account.Id);
					return AionAuthResponse.ALREADY_LOGGED_IN;
				}
				if (accountOnLS.ContainsKey(account.Id))
				{
					accountOnLS[account.Id].Disconnect();
					accountOnLS.Remove(account.Id);
					return AionAuthResponse.ALREADY_LOGGED_IN;
				}
				client.Account = account;
				accountOnLS.Add(account.Id, client);
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(@lock);
				}
			}
			account.LastIp = text;
			account.AccountTime.LastLoginTime = DateAndTime.Now;
			AccountTimeController.UpDataLoginTime(account);
			return AionAuthResponse.AUTHED;
		}

		private static object EncodePassword(string password)
		{
			if (!LoginConfig.ENCODE_PASSWORD)
			{
				return password;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			return Convert.ToBase64String(SHA1.Create().ComputeHash(bytes));
		}

		private static Account CreateAccount(string userName, string password)
		{
			if (!NamePattern.Match(userName).Success)
			{
				return null;
			}
			string password2 = Conversions.ToString(EncodePassword(password));
			Account account = new Account();
			account.Name = userName;
			account.Password = password2;
			account.Activated = 1;
			if (DAOManager.AccountDAO.InsertAccount(account))
			{
				AccountTime accountTime = new AccountTime(account.Id);
				accountTime.CreateTime = DateAndTime.Now;
				accountTime.ExpirationPeriod = DateAndTime.Now;
				DateTime lastTime = DateAndTime.Now.AddDays(-1.0);
				DateTime dayTime = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day, 8, 0, 0);
				DateTime birthDayTime = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day);
				if (ExpirationConfig.EXPIRATION_ENABLE)
				{
					accountTime.ExpirationType = (ExpirationType)Conversions.ToInteger(Enum.Parse(typeof(ExpirationType), ExpirationConfig.EXPIRATION_TYPE, ignoreCase: true));
					if (accountTime.ExpirationType == ExpirationType.TIMING)
					{
						accountTime.ExpirationPeriod = DateAndTime.Now.AddHours(ExpirationConfig.EXPIRATION_VALUE);
					}
					else if (accountTime.ExpirationType == ExpirationType.PERIOD)
					{
						accountTime.ExpirationTime = checked((uint)(ExpirationConfig.EXPIRATION_VALUE * 60 * 60));
					}
				}
				else
				{
					accountTime.ExpirationType = ExpirationType.FREE;
				}
				DAOManager.AccountTimeDAO.InsertAccountTime(account.Id, accountTime);
				account.AccountTime = accountTime;
				account.LoginReward = new LoginReward(account.Id, 0, 0, lastTime, dayTime, birthDayTime);
				DAOManager.LoginRewardDAO.InsertLoginReward(account.LoginReward);
				account.LunaInfo = new LunaInfo(account.Id, 0L, 0, 0, 0, dayTime.AddHours(1.0));
				DAOManager.LunaDAO.Store(account.LunaInfo);
				return account;
			}
			return null;
		}

		internal static void SendServerListFor(int accountId)
		{
			if (accountOnLS.ContainsKey(accountId))
			{
				accountOnLS[accountId].SendPacket(new SM_SERVER_LIST());
			}
		}

		internal static void CheckAuth(SessionKey key, GameConnection client)
		{
			AionConnection value = null;
			if (Conversions.ToBoolean(accountOnLS.TryGetValue(key.AccountId, out value) && Conversions.ToBoolean(value.SessionKey.CheckSessionKey(key))))
			{
				accountOnLS.Remove(key.AccountId);
				GameInfo gameInfo = GameService.GameServerIndoForId(client.GameServerId);
				Account account = value.Account;
				gameInfo.AddAccount(account);
				account.LastServer = checked((byte)gameInfo.ServerId);
				LunaInfo lunaInfo = DAOManager.LunaDAO.LoadLuna(key.AccountId);
				if (Information.IsNothing(lunaInfo))
				{
					DateTime time = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day, 9, 0, 0);
					lunaInfo = new LunaInfo(key.AccountId, 0L, 0, 0, 0, time);
					DAOManager.LunaDAO.Store(lunaInfo);
				}
				account.LunaInfo = lunaInfo;
				client.SendPacket(new SM_GS_ACCOUNT_AUTH_RESPONSE(key.AccountId, ok: true, account));
				DAOManager.AccountDAO.UpdateLastIpAndServer(account);
				if (ExpirationConfig.EXPIRATION_ENABLE)
				{
					ExpirationService.GetInstance().Add(account);
				}
			}
			else
			{
				client.SendPacket(new SM_GS_ACCOUNT_AUTH_RESPONSE(key.AccountId, ok: false, null));
			}
		}

		internal static void AddReconnectingAccount(ReConnectionAccount reConnectionAccount)
		{
			lock (reconnectingAccounts)
			{
				if (!reconnectingAccounts.ContainsKey(reConnectionAccount.Account.Id))
				{
					reconnectingAccounts.Add(reConnectionAccount.Account.Id, reConnectionAccount);
				}
			}
		}

		internal static void AuthReconnectingAccount(int accountId, int loginOk, int rekey, AionConnection client)
		{
			ReConnectionAccount value = null;
			if (reconnectingAccounts.TryGetValue(accountId, out value) && value.ReKey == rekey)
			{
				reconnectingAccounts.Remove(accountId);
				client.Account = value.Account;
				accountOnLS.Add(value.Account.Id, client);
				client.State = State.AUTHED_LOGIN;
				client.SessionKey = new SessionKey(client.Account);
				client.SendPacket(new SM_UPDATE_SESSION(client.SessionKey));
			}
			else
			{
				client.Disconnect();
			}
		}
	}
}
