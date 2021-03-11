using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class CM_GS_ACCOUNT_TOLL_INFO : GsClientPacket
	{
		private long toll;

		private string accountName;

		public CM_GS_ACCOUNT_TOLL_INFO(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			toll = readQ();
			accountName = readS();
		}

		protected override void runImpl()
		{
			Account account = AccountController.LoadAccount(accountName);
			if (!Information.IsNothing(account))
			{
				DAOManager.AccountDAO.UpdataToll(account.Id, toll, 0L);
			}
		}
	}
}
