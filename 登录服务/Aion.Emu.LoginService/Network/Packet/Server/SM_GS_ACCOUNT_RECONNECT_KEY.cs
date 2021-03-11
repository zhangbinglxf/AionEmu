namespace Aion.Emu.LoginService
{
	public class SM_GS_ACCOUNT_RECONNECT_KEY : GsServerPacket
	{
		private int _accountId;

		private int _rekey;

		public SM_GS_ACCOUNT_RECONNECT_KEY(int accountId, int rekey)
		{
			_accountId = accountId;
			_rekey = rekey;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_accountId);
			writeD(_rekey);
		}
	}
}
