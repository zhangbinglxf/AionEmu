using System.Collections.Generic;

namespace Aion.Emu.ChatService
{
	public class ChatClient
	{
		private int _playerId;

		private byte[] _identifier;

		private byte[] _token;

		private string _accountName;

		private string _PlayerName;

		private List<int> _channels;

		public int PlayerId => _playerId;

		public byte[] ToKen => _token;

		public string AccountName => _accountName;

		public string PlayerName => _PlayerName;

		public byte[] Identifier
		{
			get
			{
				return _identifier;
			}
			set
			{
				_identifier = value;
			}
		}

		public ChatClient(int playerId, byte[] token, string playerName, string accountName)
		{
			_playerId = playerId;
			_token = token;
			_PlayerName = playerName;
			_accountName = accountName;
			_channels = new List<int>();
		}

		public void AddChannel(int channel)
		{
			if (!_channels.Contains(channel))
			{
				_channels.Add(channel);
			}
		}

		public void RemoveChannel(int channel)
		{
			if (_channels.Contains(channel))
			{
				_channels.Remove(channel);
			}
		}

		public bool IsInChannel(int channel)
		{
			return _channels.Contains(channel);
		}

		public bool SamePlayer(string playerName)
		{
			return _PlayerName.Equals(playerName);
		}
	}
}
