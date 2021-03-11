using System;
using System.Net.Sockets;

namespace Aion.Emu.Common
{
	public class ServerListenerEventArgs : EventArgs
	{
		private Socket _socket;

		public Socket ClientSocket => _socket;

		public ServerListenerEventArgs(Socket socket)
		{
			_socket = socket;
		}
	}
}
