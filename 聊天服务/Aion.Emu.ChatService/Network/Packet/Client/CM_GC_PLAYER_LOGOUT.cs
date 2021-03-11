namespace Aion.Emu.ChatService
{
	public class CM_GC_PLAYER_LOGOUT : GsClientPacket
	{
		private int playerId;

		public CM_GC_PLAYER_LOGOUT(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			playerId = readD();
		}

		protected override void runImpl()
		{
		}
	}
}
