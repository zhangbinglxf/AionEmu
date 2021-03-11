using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class CM_GS_ACCOUNT_LIST : GsClientPacket
	{
		private List<string> accountNames;

		public CM_GS_ACCOUNT_LIST(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accountNames = new List<string>();
			checked
			{
				int num = readD() - 1;
				for (int i = 0; i <= num; i++)
				{
					accountNames.Add(readS());
				}
			}
		}

		protected override void runImpl()
		{
			foreach (string accountName in accountNames)
			{
				Account account = AccountController.LoadAccount(accountName);
				if (!Information.IsNothing(account) & base.Client.Login.HasAccount(account.Id))
				{
					SendPacket(new SM_GS_REQUEST_KICK_ACCOUNT(account.Id));
				}
				else
				{
					base.Client.Login.AddAccount(account);
				}
			}
		}
	}
}
