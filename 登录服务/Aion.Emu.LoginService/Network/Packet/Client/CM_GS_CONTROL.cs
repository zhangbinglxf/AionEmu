namespace Aion.Emu.LoginService
{
	public class CM_GS_CONTROL : GsClientPacket
	{
		private byte type;

		private string adminName;

		private string accountName;

		private string playerName;

		private long param;

		private bool result;

		public CM_GS_CONTROL(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			type = readC();
			adminName = readS();
			accountName = readS();
			playerName = readS();
			param = readQ();
		}

		protected override void runImpl()
		{
			Account account = AccountController.LoadAccount(accountName);
			switch (type)
			{
			case 1:
				account.AccessLevel = checked((byte)param);
				break;
			case 2:
				account.MemberShipExp = param;
				break;
			}
			result = DAOManager.AccountDAO.UpdataAccount(account);
			SendPacket(new SM_GS_CONTROL_RESPONSE(type, result, playerName, account.Id, param, adminName));
		}
	}
}
