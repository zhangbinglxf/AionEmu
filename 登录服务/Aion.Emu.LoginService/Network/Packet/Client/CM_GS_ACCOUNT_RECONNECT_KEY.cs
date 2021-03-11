using System;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class CM_GS_ACCOUNT_RECONNECT_KEY : GsClientPacket
	{
		private int accountId;

		public CM_GS_ACCOUNT_RECONNECT_KEY(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accountId = readD();
		}

		protected override void runImpl()
		{
			int num = new Random().Next();
			Account account = base.Client.Login.RemoveGetAccount(accountId);
			if (!Information.IsNothing(account))
			{
				AccountController.AddReconnectingAccount(new ReConnectionAccount(account, num));
			}
			SendPacket(new SM_GS_ACCOUNT_RECONNECT_KEY(accountId, num));
		}
	}
}
