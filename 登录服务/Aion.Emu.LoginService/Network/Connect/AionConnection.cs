using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class AionConnection : AConnection
	{
		private ILog log;


		private bool IsRead;

		private bool IsWrite;

		private State _state;

		private Queue<LsClientPacket> readPackets;

		private Queue<LsServerPacket> sendPackets;

		private RsaCrypt _rsa;

		private Random Rnd;

		private SessionKey _sessionKey;

		private bool _joinGS;

		private bool _checksum;

		private Account _account;

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

		public Account Account
		{
			get
			{
				return _account;
			}
			set
			{
				_account = value;
			}
		}

		public SessionKey SessionKey
		{
			get
			{
				return _sessionKey;
			}
			set
			{
				_sessionKey = value;
			}
		}

		public bool JoinGS
		{
			get
			{
				return _joinGS;
			}
			set
			{
				_joinGS = value;
			}
		}

		public AionConnection(Socket socket)
			: base(socket, new LoginProtocol(AionPacketFactory.GetInstance()))
		{
			log = LogManager.GetLogger(GetType());
			IsRead = false;
			IsWrite = false;
			Rnd = new Random();
			readPackets = new Queue<LsClientPacket>();
			sendPackets = new Queue<LsServerPacket>();
			_state = State.CONNECTED;
		}

		protected override void Disconnected()
		{
			if (!Information.IsNothing(_account) && !_joinGS)
			{
				AccountController.RemoveAccountOnLS(_account);
				if (!Information.IsNothing(_account.ActiveSlb))
				{
					_account.ActiveSlb.Decrement();
				}
				log.InfoFormat("来自 {0} 的客户端连接中断!", (object)base.IP);
			}
		}

		protected override void Initialized()
		{
			_rsa = KeyGen.NextRsaCrypt();
			SendPacket(new SM_INIT(_rsa.Modulus, base.MessageProtocol.Keys));
		}

		public byte[] RsaDecrypt(byte[] data)
		{
			try
			{
				return (byte[])_rsa.Decrypt(data);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
			}
			return new byte[4];
		}

		protected override void PacketHandle(BasePacket pak)
		{
			readPackets.Enqueue((LsClientPacket)pak);
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
				LsClientPacket lsClientPacket = readPackets.Dequeue();
				using MemoryStream input = new MemoryStream(lsClientPacket.PacketData);
				lsClientPacket.Rbuf = new BinaryReader(input);
				lsClientPacket.Client = this;
				if (lsClientPacket.Read())
				{
					lsClientPacket.Run();
				}
			}
			IsRead = false;
		}

		internal void Encrypt(ref byte[] packetData, int v, int length)
		{
			base.MessageProtocol.EnCrypt(ref packetData, v, length);
		}

		internal void Close(LsServerPacket packet)
		{
			sendPackets.Clear();
			sendPackets.Enqueue(packet);
			Disconnect();
		}

		public override void SendPacket(BasePacket pak)
		{
			sendPackets.Enqueue((LsServerPacket)pak);
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
				LsServerPacket lsServerPacket = sendPackets.Dequeue();
				try
				{
					lsServerPacket.write(this);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					log.Error((object)ex2);
					ProjectData.ClearProjectError();
				}
				base.SendPacket(lsServerPacket);
			}
			IsWrite = false;
		}

		internal int CheckSum(int length)
		{
			checked
			{
				length += 4;
				if (!_checksum)
				{
					length += 4;
					length += 8 - unchecked(length % 8);
					_checksum = true;
				}
				else
				{
					length += 8 - unchecked(length % 8);
				}
				return length;
			}
		}
	}
}
