using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using Microsoft.VisualBasic;

namespace Aion.Emu.ChatService
{
	public class GameService
	{


		private static ILog log = LogManager.GetLogger(typeof(GameService));

		private static Dictionary<int, GameInfo> gameservers = new Dictionary<int, GameInfo>();

		public static void Load()
		{
			gameservers = DAOManager.GameDAO.LoadGameServer();
			log.InfoFormat("注册{0}个允许接入的游戏服务器!", (object)gameservers.Count);
		}

		internal static GsAuthResponse RegisterServer(byte gameId, byte[] ips, string password, GameConnection client)
		{
			string text = client.IP.Split(':')[0];
			if (!gameservers.ContainsKey(gameId))
			{
				log.WarnFormat("未注册的游戏服务器ID:{0} IP:{1}", (object)gameId, (object)text);
				return GsAuthResponse.NOT_AUTHED;
			}
			GameInfo gameInfo = gameservers[gameId];
			if (!Information.IsNothing(gameInfo.GameConnection))
			{
				log.WarnFormat("游戏服务器ID:{0} IP:{1} 试图重复注册.", (object)gameId, (object)text);
				return GsAuthResponse.ALREADY_REGISTERED;
			}
			if (!gameInfo.GameIp.Equals(text))
			{
				log.WarnFormat("游戏服务器ID:{0} 授权通信地址与 IP:{1} 不匹配.", (object)gameId, (object)text);
				return GsAuthResponse.NOT_AUTHED;
			}
			if (!password.Equals(gameInfo.Password))
			{
				log.WarnFormat("游戏服务器ID:{0} IP:{1} 通信密码错误.", (object)gameId, (object)text);
				return GsAuthResponse.NOT_AUTHED;
			}
			gameInfo.GameConnection = client;
			gameInfo.GameConnection.GameServerId = gameId;
			gameInfo.GameConnection.Chat = new ChatService();
			log.InfoFormat("#{0} 号游戏服务器认证成功.", (object)gameId);
			return GsAuthResponse.AUTHED;
		}

		internal static void RegissterPlayerToServer(int serverId, int playerId, byte[] toKen, byte[] identifier, AionConnection client, string playerName, string accountName)
		{
			if (!gameservers.ContainsKey(serverId))
			{
				log.WarnFormat("来自 {0} 的客户端尝试连接未注册的游戏服务器 ID:{1}", (object)client.IP, (object)serverId);
				client.Disconnect();
				return;
			}
			GameInfo gameInfo = gameservers[serverId];
			if (Information.IsNothing(gameInfo.GameConnection))
			{
				log.WarnFormat("#{0}号游戏服务器尚未连接,来自{0}的客户端频道连接请求被拒绝!", (object)serverId, (object)client.IP);
				client.Disconnect();
			}
			else
			{
				gameInfo.GameConnection.Chat.RegissterPlayerConnection(serverId, playerId, toKen, identifier, client, playerName, accountName);
			}
		}

		internal static void CloseGameServer(int id)
		{
			if (gameservers.ContainsKey(id))
			{
				gameservers[id].GameConnection = null;
			}
		}

		internal static void RegisterPlayerChannel(AionConnection con, short channelIndex, byte[] channelIdentifier)
		{
			gameservers[con.ServerId].GameConnection.Chat.RegisterPlayerChannel(con, channelIndex, channelIdentifier);
		}

		internal static void SendMessage(int serverId, int playerId, int channelId, byte[] message)
		{
			if (!ChatConfig.CROSS_SERVER_ENABLE)
			{
				gameservers[serverId].GameConnection.Chat.SendMessage(playerId, channelId, message);
				return;
			}
			gameservers.Values.ToList().ForEach(delegate(GameInfo game)
			{
				game.GameConnection.Chat.SendMessage(playerId, channelId, message);
			});
		}
	}
}
