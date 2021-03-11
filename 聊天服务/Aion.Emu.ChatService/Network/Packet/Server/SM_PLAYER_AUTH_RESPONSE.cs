using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class SM_PLAYER_AUTH_RESPONSE : CsServerPacket
	{
		protected override void writeImpl(AionConnection con)
		{
			writeC(64);
			writeH(1);
			if (Operators.CompareString(ChatConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				writeD(0);
				writeH(7442);
			}
			else
			{
				writeD(199032832);
			}
		}
	}
}
