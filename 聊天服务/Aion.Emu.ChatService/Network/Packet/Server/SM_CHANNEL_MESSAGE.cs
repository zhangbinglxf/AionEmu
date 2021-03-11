using System;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class SM_CHANNEL_MESSAGE : CsServerPacket
	{
		private int _sender;

		private int _reader;

		private int _channel;

		private byte[] _identifier;

		private byte[] _message;

		public SM_CHANNEL_MESSAGE(int sender, int reader, int channel, byte[] identifier, byte[] message)
		{
			_sender = sender;
			_reader = reader;
			_channel = channel;
			_identifier = identifier;
			_message = message;
		}

		protected override void writeImpl(AionConnection con)
		{
			if (Operators.CompareString(ChatConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				writeC(0);
				writeD(_reader);
				writeD(0);
				writeD(_channel);
				writeD(_sender);
				writeD(0);
				writeC(0);
			}
			else
			{
				writeC(0);
				writeD(_channel);
				writeD(_sender);
				writeD(0);
			}
			checked
			{
				writeH((int)Math.Round((double)_identifier.Count() / 2.0));
				writeB(_identifier);
				writeH((int)Math.Round((double)_message.Count() / 2.0));
				writeB(_message);
			}
		}
	}
}
