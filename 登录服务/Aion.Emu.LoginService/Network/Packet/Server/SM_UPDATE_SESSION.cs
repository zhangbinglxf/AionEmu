namespace Aion.Emu.LoginService
{
	public class SM_UPDATE_SESSION : LsServerPacket
	{
		private SessionKey _key;

		public SM_UPDATE_SESSION(SessionKey key)
		{
			_key = key;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeD(_key.AccountId);
			writeD(_key.LoginOk);
			writeC(0);
		}
	}
}
