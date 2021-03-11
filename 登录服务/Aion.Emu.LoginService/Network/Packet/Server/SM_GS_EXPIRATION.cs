namespace Aion.Emu.LoginService
{
	public class SM_GS_EXPIRATION : GsServerPacket
	{
		private int _accountId;

		private long _time;

		public SM_GS_EXPIRATION(int accId, long time)
		{
			_accountId = accId;
			_time = time;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_accountId);
			writeQ(_time);
		}
	}
}
