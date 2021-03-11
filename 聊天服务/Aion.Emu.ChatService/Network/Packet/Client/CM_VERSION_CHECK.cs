namespace Aion.Emu.ChatService
{
	public class CM_VERSION_CHECK : CsClientPacket
	{
		private int unk;

		public CM_VERSION_CHECK(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			readC();
			readH();
			unk = readD();
			readD();
			readD();
		}

		protected override void runImpl()
		{
			SendPacket(new SM_VERSION_CHECK(unk));
		}
	}
}
