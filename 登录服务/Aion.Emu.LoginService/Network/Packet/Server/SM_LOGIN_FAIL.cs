namespace Aion.Emu.LoginService
{
	public class SM_LOGIN_FAIL : LsServerPacket
	{
		private AionAuthResponse _resp;

		public SM_LOGIN_FAIL(AionAuthResponse resp)
		{
			_resp = resp;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeD((int)_resp);
		}
	}
}
