namespace Aion.Emu.ChatService
{
	public class SM_VERSION_CHECK : CsServerPacket
	{
		private int _unk;

		public SM_VERSION_CHECK(int unk)
		{
			_unk = unk;
		}

		protected override void writeImpl(AionConnection con)
		{
			writeC(64);
			writeD(_unk);
			writeH(0);
		}
	}
}
