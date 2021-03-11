namespace Aion.Emu.LoginService
{
	public class CM_AUTH_GG : LsClientPacket
	{
		private int sessionId;

		public CM_AUTH_GG(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			sessionId = readD();
			readD();
			readD();
			readD();
			readD();
			readB(11);
		}

		protected override void runImpl()
		{
			if (base.Client.ClientID == sessionId)
			{
				base.Client.State = State.AUTHED;
				SendPacket(new SM_AUTH_GG());
			}
			else
			{
				SendPacket(new SM_LOGIN_FAIL(AionAuthResponse.SYSTEM_ERROR));
			}
		}
	}
}
