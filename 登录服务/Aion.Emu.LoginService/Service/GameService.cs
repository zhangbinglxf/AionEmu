using System.Collections.Generic;
using log4net;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class GameService
	{
		private static ILog log = LogManager.GetLogger(typeof(GameService));

		private static Dictionary<int, GameInfo> gameservers;

		private static Dictionary<int, Dictionary<int, int>> accountCharacters = new Dictionary<int, Dictionary<int, int>>();

		internal static IEnumerable<GameInfo> AllGameServers => gameservers.Values;

		public static int ServerCount => gameservers.Count;

		public static void LoadGameservers()
		{
			gameservers = DAOManager.GameDAO.LoadGameservers();
			log.InfoFormat("注册{0}个允许接入的游戏服务器!", (object)gameservers.Count);
		}

		internal static void CloseGameServer(int id)
		{
			if (gameservers.ContainsKey(id))
			{
				gameservers[id].GameConnection = null;
			}
		}

		internal static GsAuthResponse RegisterGameServer(GameConnection client, byte gameId, byte[] ipArray, short port, int maxPlayers, string password)
		{
			if (!gameservers.ContainsKey(gameId))
			{
				log.WarnFormat("来自 {0} 的游戏服务器未注册,认证被拒绝!", (object)client.IP);
				return GsAuthResponse.NOT_AUTHED;
			}
			GameInfo gameInfo = gameservers[gameId];
			string text = client.IP.Split(':')[0];
			if (!gameInfo.ServerIp.Equals(text))
			{
				log.WarnFormat("#{0} 号游戏服务器注册IP与连接IP:{1} 不匹配!", (object)gameId, (object)text);
				return GsAuthResponse.NOT_AUTHED;
			}
			if (!Information.IsNothing(gameInfo.GameConnection))
			{
				log.WarnFormat("来自 {0} 的游戏服务器试图重复认证被拒绝!", (object)client.IP);
				return GsAuthResponse.ALREADY_REGISTERED;
			}
			if (!gameInfo.Password.Equals(password))
			{
				log.WarnFormat("来自 {0} 的游戏服务器通信密码错误认证被拒绝!", (object)client.IP);
				return GsAuthResponse.NOT_AUTHED;
			}

			string s = System.Text.Encoding.UTF8.GetString(ipArray);

			gameInfo.GameAddress = ipArray;
			gameInfo.Port = port;
			gameInfo.MaxPlayers = maxPlayers;
			client.State = State.AUTHED;
			client.GameServerId = gameId;
			client.Login = new LoginService();
			gameInfo.GameConnection = client;
			log.InfoFormat("#{0}号游戏服务器认证成功!", (object)gameId);
			return GsAuthResponse.AUTHED;
		}

		internal static void SendExpiration(int accountId, long second)
		{
			foreach (GameInfo value in gameservers.Values)
			{
				if (value.IsOnGameServer(accountId))
				{
					value.GameConnection.SendPacket(new SM_GS_EXPIRATION(accountId, second));
				}
			}
		}

		internal static void KickAccountFromGameServer(int id)
		{
			foreach (GameInfo value in gameservers.Values)
			{
				if (value.IsOnGameServer(id))
				{
					value.GameConnection.SendPacket(new SM_GS_REQUEST_KICK_ACCOUNT(id));
					break;
				}
			}
		}

		internal static bool HahServerOnline()
		{
			foreach (GameInfo value in gameservers.Values)
			{
				if (value.IsOnline())
				{
					return true;
				}
			}
			return false;
		}

		internal static bool ContainsAccount(int id)
		{
			foreach (GameInfo value in gameservers.Values)
			{
				if (value.IsOnGameServer(id))
				{
					return true;
				}
			}
			return false;
		}

		internal static void loadGSCharactersCount(int accountId)
		{
			Dictionary<int, int> dictionary = null;
			if (accountCharacters.ContainsKey(accountId))
			{
				accountCharacters.Remove(accountId);
			}
			accountCharacters.Add(accountId, new Dictionary<int, int>());
			dictionary = accountCharacters[accountId];
			foreach (GameInfo value in gameservers.Values)
			{
				if (!Information.IsNothing(value.GameConnection))
				{
					value.GameConnection.SendPacket(new SM_GS_CHARACTER_RESPONSE(accountId));
				}
				else
				{
					dictionary.Add(value.ServerId, 0);
				}
			}
		}

		internal static void AddGSCharacterCountFor(int accountId, int gameServerId, int characterCount)
		{
			lock (accountCharacters)
			{
				if (accountCharacters.ContainsKey(accountId))
				{
					if (accountCharacters[accountId].ContainsKey(gameServerId))
					{
						accountCharacters[accountId].Remove(gameServerId);
					}
					accountCharacters[accountId].Add(gameServerId, characterCount);
					if (accountCharacters[accountId].Count == gameservers.Count)
					{
						AccountController.SendServerListFor(accountId);
					}
				}
			}
		}

		internal static Dictionary<int, int> CharacterCountsFor(int accountId)
		{
			if (!accountCharacters.ContainsKey(accountId))
			{
				return new Dictionary<int, int>();
			}
			return accountCharacters[accountId];
		}

		internal static GameInfo GameServerIndoForId(int id)
		{
			if (gameservers.ContainsKey(id))
			{
				return gameservers[id];
			}
			return null;
		}

		internal static void LoginGame(AionConnection client, byte serverId)
		{
			GameInfo value = null;
			if (gameservers.TryGetValue(serverId, out value))
			{
				if (!value.IsOnline())
				{
					client.SendPacket(new SM_PLAY_FAIL(AionAuthResponse.SERVER_DOWN));
					return;
				}
				if (value.IsFull())
				{
					client.SendPacket(new SM_PLAY_FAIL(AionAuthResponse.SERVER_FULL));
					return;
				}
				client.JoinGS = true;
				client.SendPacket(new SM_PLAY_OK(client.SessionKey, serverId));
				log.InfoFormat("登陆账号: {0} 进入 #{1}号游戏服务器!", (object)client.Account.Name, (object)serverId);
			}
			else
			{
				log.WarnFormat("来自: {0} 的用户尝试进入不存在的游戏服务器({1})!", (object)client.Account.Name, (object)serverId);
			}
		}

		public static void SendPayRewardInfo(List<PayRewardTemplate> rewards)
		{
			foreach (GameInfo allGameServer in AllGameServers)
			{
				if (allGameServer.IsOnline())
				{
					allGameServer.GameConnection.SendPacket(new SM_GS_PAY_REWARD_INFO(rewards));
				}
			}
		}
	}
}
