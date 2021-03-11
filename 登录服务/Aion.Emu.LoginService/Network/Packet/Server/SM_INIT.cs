using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class SM_INIT : LsServerPacket
	{
		private byte[] _rsaKey;

		private byte[] _blowfishKey;

		public SM_INIT(byte[] rsa, byte[] key)
		{
			_rsaKey = rsa;
			_blowfishKey = key;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeD(checked((int)con.ClientID));
			writeD(50721);
			writeB(_rsaKey);
			writeB(16);
			writeB(_blowfishKey);
			writeD(197635);
			if (Operators.CompareString(LoginConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				writeB(19);
			}
			else
			{
				writeD(2097152);
			}
		}
	}
}
