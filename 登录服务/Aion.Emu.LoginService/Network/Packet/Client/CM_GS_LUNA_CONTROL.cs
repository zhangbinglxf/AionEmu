namespace Aion.Emu.LoginService
{
	public class CM_GS_LUNA_CONTROL : GsClientPacket
	{
		private int responseId;

		private int activeId;

		private int accountId;

		private long point;

		public CM_GS_LUNA_CONTROL(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			responseId = readD();
			activeId = readC();
			accountId = readD();
			point = readQ();
		}

		protected override void runImpl()
		{
			LunaController.GetInstance().Request(base.Client, responseId, activeId, accountId, point);
		}
	}
}
