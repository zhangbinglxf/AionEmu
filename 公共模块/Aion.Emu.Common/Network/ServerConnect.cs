using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public class ServerConnect
	{
		private int ConnectTimeout;

		[CompilerGenerated]
		private EventHandler<ServerConnectEventArgs> ConnectSuccessEvent;

		[CompilerGenerated]
		private EventHandler ConnectFailedEvent;

		private IPEndPoint _endPoint;

		private Socket _socket;

		public event EventHandler<ServerConnectEventArgs> ConnectSuccess
		{
			[CompilerGenerated]
			add
			{
				EventHandler<ServerConnectEventArgs> eventHandler = ConnectSuccessEvent;
				EventHandler<ServerConnectEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<ServerConnectEventArgs> value2 = (EventHandler<ServerConnectEventArgs>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref ConnectSuccessEvent, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<ServerConnectEventArgs> eventHandler = ConnectSuccessEvent;
				EventHandler<ServerConnectEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<ServerConnectEventArgs> value2 = (EventHandler<ServerConnectEventArgs>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref ConnectSuccessEvent, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public event EventHandler ConnectFailed
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = ConnectFailedEvent;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref ConnectFailedEvent, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = ConnectFailedEvent;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref ConnectFailedEvent, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public ServerConnect(IPEndPoint endpoint)
		{
			ConnectTimeout = 10;
			_endPoint = endpoint;
		}

		public void Connect()
		{
			_socket = null;
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			checked
			{
				while (true)
				{
					try
					{
						_socket.Connect(_endPoint);
						OnConnnectSuccess(_socket);
						return;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						if (ConnectTimeout <= 1)
						{
							OnConnectFailed();
							ConnectTimeout = 10;
							Thread.Sleep(1000);
							ProjectData.ClearProjectError();
						}
						else
						{
							ConnectTimeout--;
							Thread.Sleep(1000);
							ProjectData.ClearProjectError();
						}
					}
				}
			}
		}

		private void OnConnectFailed()
		{
			ConnectFailedEvent?.Invoke(this, EventArgs.Empty);
		}

		private void OnConnnectSuccess(Socket socket)
		{
			ConnectSuccessEvent?.Invoke(this, new ServerConnectEventArgs(socket));
		}
	}
}
