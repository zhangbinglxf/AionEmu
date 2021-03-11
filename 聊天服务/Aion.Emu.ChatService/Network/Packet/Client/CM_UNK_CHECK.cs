namespace Aion.Emu.ChatService
{
	public class CM_UNK_CHECK : CsClientPacket
	{
		public CM_UNK_CHECK(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			readB(147);
		}

		protected override void runImpl()
		{
		}
	}
}
