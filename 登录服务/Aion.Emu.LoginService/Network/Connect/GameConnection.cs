using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class GameConnection : AConnection
	{
		private ILog log;

		private int _id;

		private bool IsRead;

		private bool IsWrite;

		private State _state;

		private Queue<GsClientPacket> readPackets;

		private Queue<GsServerPacket> sendPackets;

		private LoginService _login;

		public State State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}

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

		public LoginService Login
		{
			get
			{
				return _login;
			}
			set
			{
				_login = value;
			}
		}

		public GameConnection(Socket socket)
			: base(socket, new MessageProtocol(GamePacketFactory.GetInstance()))
		{
			log = LogManager.GetLogger(GetType());
			IsRead = false;
			IsWrite = false;
			_state = State.CONNECTED;
			readPackets = new Queue<GsClientPacket>();
			sendPackets = new Queue<GsServerPacket>();
		}

		protected override void Disconnected()
		{
			log.WarnFormat("#{0} 号游戏服务器连接中断!", (object)_id);
			GameService.CloseGameServer(_id);
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
	}
}
