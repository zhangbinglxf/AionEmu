namespace Aion.Emu.LoginService
{
	public class CM_GS_PAY_SEND_REQUEST : GsClientPacket
	{
		private int requestId;

		private bool sendSuccess;

		public CM_GS_PAY_SEND_REQUEST(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			requestId = readD();
			sendSuccess = readBC();
		}

		protected override void runImpl()
		{
			PayRewardController.GetInstance().FinishRequest(requestId, sendSuccess);
		}
	}
}
