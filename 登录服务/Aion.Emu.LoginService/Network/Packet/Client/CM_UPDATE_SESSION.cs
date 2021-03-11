using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class CM_UPDATE_SESSION : LsClientPacket
	{
		private int accountId;

		private int loginOk;

		private int rekey;

		public CM_UPDATE_SESSION(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accountId = readD();
			loginOk = readD();
			rekey = readD();
			if (Operators.CompareString(LoginConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				readB(11);
			}
		}

		protected override void runImpl()
		{
			AccountController.AuthReconnectingAccount(accountId, loginOk, rekey, base.Client);
		}
	}
}
