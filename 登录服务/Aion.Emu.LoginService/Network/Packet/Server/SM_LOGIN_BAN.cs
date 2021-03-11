namespace Aion.Emu.LoginService
{
	public class SM_LOGIN_BAN : LsServerPacket
	{
		private int _id;

		public SM_LOGIN_BAN(int id)
		{
			_id = id;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeD(_id);
		}
	}
}
