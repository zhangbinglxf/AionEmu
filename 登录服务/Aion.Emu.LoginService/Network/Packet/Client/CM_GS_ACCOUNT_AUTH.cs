namespace Aion.Emu.LoginService
{
	public class CM_GS_ACCOUNT_AUTH : GsClientPacket
	{
		private SessionKey key;

		public CM_GS_ACCOUNT_AUTH(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			int accountId = readD();
			int loginOk = readD();
			int playerOk = readD();
			int playerOk2 = readD();
			key = new SessionKey(accountId, loginOk, playerOk, playerOk2);
		}

		protected override void runImpl()
		{
			AccountController.CheckAuth(key, base.Client);
		}
	}
}
