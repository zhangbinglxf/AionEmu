using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class SM_AUTH_GG : LsServerPacket
	{
		protected override void writeImpl(AionConnection con)
		{
			writeD(checked((int)con.ClientID));
			if (Operators.CompareString(LoginConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				writeB(35);
			}
			else
			{
				writeB(16);
			}
		}
	}
}
