namespace Aion.Emu.LoginService
{
	public class SM_GS_REQUEST_KICK_ACCOUNT : GsServerPacket
	{
		private int _accountId;

		public SM_GS_REQUEST_KICK_ACCOUNT(int account)
		{
			_accountId = account;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_accountId);
		}
	}
}
