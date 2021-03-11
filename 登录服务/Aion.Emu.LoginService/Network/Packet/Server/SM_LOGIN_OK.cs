using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class SM_LOGIN_OK : LsServerPacket
	{
		private SessionKey _key;

		public SM_LOGIN_OK(SessionKey key)
		{
			_key = key;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeD(_key.AccountId);
			writeD(_key.LoginOk);
			writeD(0);
			writeD(0);
			if (Operators.CompareString(LoginConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				writeD(2000);
				writeD(82570696);
				writeB(47);
			}
			else
			{
				writeD(1002);
				writeB(28);
			}
		}
	}
}
