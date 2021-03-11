namespace Aion.Emu.ChatService
{
	public class CM_UNK_PING : CsClientPacket
	{
		public CM_UNK_PING(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			readB(19);
		}

		protected override void runImpl()
		{
		}
	}
}
