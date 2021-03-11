namespace Aion.Emu.LoginService
{
	public class CM_GS_ACCOUNT_DISCONNECTED : GsClientPacket
	{
		private int accountId;

		public CM_GS_ACCOUNT_DISCONNECTED(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accountId = readD();
		}

		protected override void runImpl()
		{
			base.Client.Login.RemoveGetAccount(accountId);
		}
	}
}
