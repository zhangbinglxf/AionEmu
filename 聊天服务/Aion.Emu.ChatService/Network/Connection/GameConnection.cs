using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class GameConnection : AConnection
	{
		private ILog log;

		private int _id;

		private bool IsRead;

		private bool IsWrite;

		private Queue<GsClientPacket> readPackets;

		private Queue<GsServerPacket> sendPackets;

		private ChatService _chat;

		public int GameServerId
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public ChatService Chat
		{
			get
			{
				return _chat;
			}
			set
			{
				_chat = new ChatService();
			}
		}

		public GameConnection(Socket socket)
			: base(socket, new MessageProtocol(GamePacketFactory.GetInstance()))
		{
			log = LogManager.GetLogger(GetType());
			IsRead = false;
			IsWrite = false;
			readPackets = new Queue<GsClientPacket>();
			sendPackets = new Queue<GsServerPacket>();
		}

		protected override void Initialized()
		{
		}

		protected override void PacketHandle(BasePacket pak)
		{
			readPackets.Enqueue((GsClientPacket)pak);
			if (!IsRead)
			{
				IsRead = true;
				ReceivePacketThread();
			}
		}

		private void ReceivePacketThread()
		{
			while (readPackets.Count != 0)
			{
				GsClientPacket gsClientPacket = readPackets.Dequeue();
				using MemoryStream input = new MemoryStream(gsClientPacket.PacketData);
				gsClientPacket.Rbuf = new BinaryReader(input);
				gsClientPacket.Client = this;
				if (gsClientPacket.Read())
				{
					gsClientPacket.Run();
				}
			}
			IsRead = false;
		}

		public override void SendPacket(BasePacket pak)
		{
			sendPackets.Enqueue((GsServerPacket)pak);
			if (!IsWrite)
			{
				IsWrite = true;
				SendPacketThread();
			}
		}

		private void SendPacketThread()
		{
			while (sendPackets.Count != 0)
			{
				GsServerPacket gsServerPacket = sendPackets.Dequeue();
				try
				{
					gsServerPacket.write(this);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					log.Error((object)ex2);
					ProjectData.ClearProjectError();
				}
				base.SendPacket(gsServerPacket);
			}
			IsWrite = false;
		}

		internal void RegisterPlayer(int playerId, string accountName, string playerName)
		{
			ChatClient chatClient = _chat.RegisterPlayer(playerId, accountName, playerName);
			if (!Information.IsNothing(chatClient))
			{
				SendPacket(new SM_GS_PLAYER_AUTH_RESPONSE(chatClient));
			}
		}

		protected override void Disconnected()
		{
			log.WarnFormat("#{0} 号游戏服务器连接中断!", (object)_id);
			GameService.CloseGameServer(_id);
		}
	}
}
