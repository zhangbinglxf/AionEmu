using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public abstract class AConnection
	{
		private ILog log;

		private long _clientId;

		private MessageProtocol _protocol;

		private string _ip;

		private Socket _socket;

		private byte[] _buffer;

		private bool _isRuning;

		[CompilerGenerated]
		private EventHandler OnDisconnectedEvent;

		public long ClientID => _clientId;

		public string IP => _ip;

		public MessageProtocol MessageProtocol => _protocol;

		public bool IsRuning => _isRuning;

		public event EventHandler OnDisconnected
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = OnDisconnectedEvent;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref OnDisconnectedEvent, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = OnDisconnectedEvent;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref OnDisconnectedEvent, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public AConnection(Socket socket, MessageProtocol protocol)
		{
			log = LogManager.GetLogger(GetType());
			_buffer = new byte[16384];
			_clientId = ConnectionIDFactory.NextID();
			_protocol = protocol;
			_ip = socket.RemoteEndPoint.ToString();
			_socket = socket;
		}

		protected abstract void Initialized();

		protected abstract void PacketHandle(BasePacket pak);

		public virtual void SendPacket(BasePacket pak)
		{
			checked
			{
				try
				{
					int i = 0;
					int num;
					for (byte[] packetData = pak.PacketData; i < packetData.Length; i += num)
					{
						num = _socket.Send(packetData, i, packetData.Length - i, SocketFlags.None);
						if (num <= 0)
						{
							Disconnect();
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Disconnect();
					ProjectData.ClearProjectError();
				}
			}
		}

		public void StartReceivePacket()
		{
			_isRuning = true;
			ReceivePacket();
			Initialized();
		}

		private void ReceivePacket()
		{
			if (IsRuning)
			{
				try
				{
					_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, EndReceicePacket, null);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Disconnect();
					ProjectData.ClearProjectError();
				}
			}
		}

		private void EndReceicePacket(IAsyncResult ar)
		{
			if (!IsRuning)
			{
				return;
			}
			try
			{
				if (Information.IsNothing(_socket) | !_socket.Connected)
				{
					Disconnect();
					return;
				}
				int num = _socket.EndReceive(ar);
				if (num <= 0)
				{
					Disconnect();
					return;
				}
				byte[] array = new byte[checked(num - 1 + 1)];
				Array.Copy(_buffer, 0, array, 0, array.Length);
				MessageProtocol.CreatePacket(array).ForEach(delegate(BasePacket p)
				{
					PacketHandle(p);
				});
				if (IsRuning)
				{
					ReceivePacket();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Disconnect();
				ProjectData.ClearProjectError();
			}
		}

		public void Disconnect()
		{
			if (Information.IsNothing(_socket))
			{
				return;
			}
			if (IsRuning)
			{
				_isRuning = false;
				OnDisconnectedEvent?.Invoke(this, EventArgs.Empty);
				Disconnected();
			}
			try
			{
				if (_socket.Connected)
				{
					_socket.Close();
				}
				_socket.Dispose();
			}
			finally
			{
				_socket = null;
				_protocol = null;
			}
		}

		protected abstract void Disconnected();

		[CompilerGenerated]
		private void _Lambda_0024__25_002D0(BasePacket p)
		{
			PacketHandle(p);
		}
	}
}
