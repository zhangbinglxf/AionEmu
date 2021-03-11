namespace Aion.Emu.LoginService
{
	public class CM_SERVER_LIST : LsClientPacket
	{
		private int _accountId;

		private int _loginOk;

		public CM_SERVER_LIST(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			_accountId = readD();
			_loginOk = readD();
			readB(15);
		}

		protected override void runImpl()
		{
			if (base.Client.SessionKey.CheckLogin(_accountId, _loginOk))
			{
				if (GameService.ServerCount == 0)
				{
					base.Client.Close(new SM_LOGIN_FAIL(AionAuthResponse.NO_GS_REGISTERED));
				}
				else
				{
					GameService.loadGSCharactersCount(_accountId);
				}
			}
			else
			{
				base.Client.Close(new SM_LOGIN_FAIL(AionAuthResponse.SYSTEM_ERROR));
			}
		}
	}
}
