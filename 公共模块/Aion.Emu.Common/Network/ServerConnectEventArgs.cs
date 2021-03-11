using System;
using System.Net.Sockets;

namespace Aion.Emu.Common
{
	public class ServerConnectEventArgs : EventArgs
	{
		private Socket _socket;

		public Socket Socket => _socket;

		public ServerConnectEventArgs(Socket socket)
		{
			_socket = socket;
		}
	}
}
