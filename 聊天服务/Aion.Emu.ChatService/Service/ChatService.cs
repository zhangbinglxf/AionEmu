using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using log4net;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class ChatService
	{
		

		private ILog log;

		private Dictionary<int, ChatClient> _players;

		private Random Rnd;

		private Dictionary<int, AionConnection> _connects;

		public ChatService()
		{
			log = LogManager.GetLogger(GetType());
			Rnd = new Random();
			_players = new Dictionary<int, ChatClient>();
			_connects = new Dictionary<int, AionConnection>();
		}

		public ChatClient RegisterPlayer(int playerId, string accountName, string playerName)
		{
			if (_players.ContainsKey(playerId))
			{
				log.DebugFormat("重复登录,角色名:{0} ID:{1}", (object)playerName, (object)playerId);
				_players.Remove(playerId);
			}
			if (!_players.ContainsKey(playerId))
			{
				SHA256Managed sHA256Managed = new SHA256Managed();
				byte[] bytes = Encoding.UTF8.GetBytes(accountName);
				byte[] accountToken = sHA256Managed.ComputeHash(bytes);
				byte[] token = GenerateToken(accountToken);
				ChatClient chatClient = new ChatClient(playerId, token, playerName, accountName);
				_players.Add(playerId, chatClient);
				return chatClient;
			}
			return null;
		}

		private byte[] GenerateToken(byte[] accountToken)
		{
			byte[] array = new byte[16];
			Rnd.NextBytes(array);
			return array.Concat(accountToken).ToArray();
		}

		internal void RegissterPlayerConnection(int serverId, int playerId, byte[] toKen, byte[] identifier, AionConnection client, string playerName, string accountName)
		{
			ChatClient value = null;
			if (!_players.TryGetValue(playerId, out value))
			{
				return;
			}
			_players.Remove(playerId);
			byte[] toKen2 = value.ToKen;
			if (!value.SamePlayer(playerName))
			{
				return;
			}
			bool flag = true;
			Array.ForEach(toKen, delegate(byte reg)
			{
				if (!toKen2.Contains(reg))
				{
					flag = false;
				}
			});
			if (flag)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(value.PlayerName + "@");
				value.Identifier = bytes.Concat(identifier).ToArray();
				client.PlayerId = playerId;
				client.ServerId = serverId;
				client.ChatClient = value;
				AddPlayer(playerId, client);
				client.SendPacket(new SM_PLAYER_AUTH_RESPONSE());
				log.InfoFormat("#{0} 玩家[{1}]进入聊天频道!", (object)serverId, (object)playerName);
			}
		}

		public void AddPlayer(int playerId, AionConnection con)
		{
			lock (_connects)
			{
				if (_connects.ContainsKey(playerId))
				{
					_connects[playerId].Disconnect();
					_connects.Remove(playerId);
				}
				_connects.Add(playerId, con);
			}
		}

		internal void RegisterPlayerChannel(AionConnection con, short channelIndex, byte[] channelIdentifier)
		{
			int hashCode = Encoding.Unicode.GetString(channelIdentifier).GetHashCode();
			if (!con.IsInChannel(hashCode))
			{
				con.AddChannel(hashCode);
			}
			if (!_connects.ContainsKey(con.PlayerId))
			{
				log.Warn((object)("进入频道异常 ID:" + Conversions.ToString(con.PlayerId)));
				_connects.Add(con.PlayerId, con);
			}
			con.SendPacket(new SM_CHANNEL_RESPONSE(channelIndex, hashCode));
		}

		internal void SendMessage(int playerId, int channelId, byte[] message)
		{
			lock (_connects)
			{
				ChatClient chatClient = _connects[playerId].ChatClient;
				foreach (AionConnection value in _connects.Values)
				{
					if (value.IsInChannel(channelId))
					{
						value.SendPacket(new SM_CHANNEL_MESSAGE(playerId, value.PlayerId, channelId, chatClient.Identifier, message));
					}
				}
			}
		}
	}
}
