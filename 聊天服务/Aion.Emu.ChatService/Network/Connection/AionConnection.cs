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
	public class AionConnection : AConnection
	{
		private ILog log;

		private bool IsRead;

		private bool IsWrite;

		private Queue<CsClientPacket> readPackets;

		private Queue<CsServerPacket> sendPackets;

		private int _playerId;

		private ChatClient _chatClient;

		private int _serverId;

		public int PlayerId
		{
			get
			{
				return _playerId;
			}
			set
			{
				_playerId = value;
			}
		}

		public ChatClient ChatClient
		{
			get
			{
				return _chatClient;
			}
			set
			{
				_chatClient = value;
			}
		}

		public int ServerId
		{
			get
			{
				return _serverId;
			}
			set
			{
				_serverId = value;
			}
		}

		public AionConnection(Socket socket)
			: base(socket, new MessageProtocol(ChatPacketFactory.GetInstance()))
		{
			log = LogManager.GetLogger(GetType());
			readPackets = new Queue<CsClientPacket>();
			sendPackets = new Queue<CsServerPacket>();
		}

		protected override void Initialized()
		{
		}

		protected override void PacketHandle(BasePacket pak)
		{
			readPackets.Enqueue((CsClientPacket)pak);
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
				CsClientPacket csClientPacket = readPackets.Dequeue();
				using MemoryStream input = new MemoryStream(csClientPacket.PacketData);
				csClientPacket.Rbuf = new BinaryReader(input);
				csClientPacket.Client = this;
				if (csClientPacket.Read())
				{
					csClientPacket.Run();
				}
			}
			IsRead = false;
		}

		public override void SendPacket(BasePacket pak)
		{
			sendPackets.Enqueue((CsServerPacket)pak);
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
				CsServerPacket csServerPacket = sendPackets.Dequeue();
				try
				{
					csServerPacket.write(this);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					log.Error((object)ex2);
					ProjectData.ClearProjectError();
				}
				base.SendPacket(csServerPacket);
			}
			IsWrite = false;
		}

		internal bool IsInChannel(int channelId)
		{
			if (Information.IsNothing(_chatClient))
			{
				return false;
			}
			return _chatClient.IsInChannel(channelId);
		}

		internal void AddChannel(int channelId)
		{
			_chatClient.AddChannel(channelId);
		}

		protected override void Disconnected()
		{
			if (!Information.IsNothing(_chatClient))
			{
				log.InfoFormat("#{0} 玩家[{1}]退出聊天频道!", (object)ServerId, (object)_chatClient.PlayerName);
				_chatClient = null;
			}
		}
	}
}
