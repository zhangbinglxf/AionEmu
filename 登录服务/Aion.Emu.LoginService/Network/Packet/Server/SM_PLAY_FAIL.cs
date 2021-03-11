namespace Aion.Emu.LoginService
{
	public class SM_PLAY_FAIL : LsServerPacket
	{
		private AionAuthResponse _resp;

		public SM_PLAY_FAIL(AionAuthResponse resp)
		{
			_resp = resp;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeD((int)_resp);
		}
	}
}
