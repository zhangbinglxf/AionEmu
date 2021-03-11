using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public abstract class ServerListener
	{
		private IPAddress _address;

		private ushort _port;

		private TcpListener _listener;

		private bool IsRuning;

		private Thread _thread;

		public ServerListener(IPAddress address, ushort port)
		{
			_address = address;
			_port = port;
		}

		private void DoListener()
		{
			while (IsRuning)
			{
				try
				{
					Socket socket = _listener.AcceptSocket();
					if (socket.Connected)
					{
						ReviceAcceptSocket(socket);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					StopSocket();
					Thread.Sleep(1000);
					if (!IsRuning)
					{
						ProjectData.ClearProjectError();
						return;
					}
					StartSocket();
					ProjectData.ClearProjectError();
				}
			}
		}

		public void StartListener()
		{
			StartSocket();
			IsRuning = true;
			_thread = new Thread(DoListener);
			_thread.Start();
		}

		private void StartSocket()
		{
			try
			{
				_listener = new TcpListener(_address, _port);
				_listener.Start();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
		}

		private void StopSocket()
		{
			try
			{
				_listener.Stop();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
		}

		public void StopListener()
		{
			IsRuning = false;
			StopSocket();
			_thread.Abort();
		}

		protected abstract void ReviceAcceptSocket(Socket socket);
	}
}
