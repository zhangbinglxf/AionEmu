namespace Aion.Emu.LoginService
{
	public class SM_GS_CHARACTER_RESPONSE : GsServerPacket
	{
		private int _accountId;

		private byte _code;

		public SM_GS_CHARACTER_RESPONSE(int accountId)
		{
			_code = 5;
			_accountId = accountId;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_accountId);
			writeC(_code);
		}
	}
}
