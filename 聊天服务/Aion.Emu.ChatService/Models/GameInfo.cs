using System.Net;

namespace Aion.Emu.ChatService
{
	public class GameInfo
	{
		private int _id;

		private string _ip;

		private string _password;

		private GameConnection _server;

		public int GameId => _id;

		public string GameIp => _ip;

		public string Password => _password;

		public byte[] GameIpArray => IPAddress.Parse(_ip).GetAddressBytes();

		public GameConnection GameConnection
		{
			get
			{
				return _server;
			}
			set
			{
				_server = value;
			}
		}

		public GameInfo(int id, string ip, string password)
		{
			_id = id;
			_ip = ip;
			_password = password;
		}
	}
}
