using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class SM_CHANNEL_RESPONSE : CsServerPacket
	{
		private short _index;

		private int _channelId;

		public SM_CHANNEL_RESPONSE(short index, int channelId)
		{
			_index = index;
			_channelId = channelId;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeC(64);
			writeH(_index);
			if (Operators.CompareString(ChatConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				writeH(0);
			}
			writeH(0);
			writeD(_channelId);
		}
	}
}
