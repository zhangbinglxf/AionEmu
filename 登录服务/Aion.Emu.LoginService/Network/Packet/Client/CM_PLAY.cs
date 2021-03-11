namespace Aion.Emu.LoginService
{
	public class CM_PLAY : LsClientPacket
	{
		private int accountId;

		private int loginOk;

		private byte serverId;

		public CM_PLAY(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accountId = readD();
			loginOk = readD();
			serverId = readC();
			readB(14);
		}

		protected override void runImpl()
		{
			if (base.Client.SessionKey.CheckLogin(accountId, loginOk))
			{
				GameService.LoginGame(base.Client, serverId);
			}
			else
			{
				base.Client.Close(new SM_LOGIN_FAIL(AionAuthResponse.SYSTEM_ERROR));
			}
		}
	}
}
