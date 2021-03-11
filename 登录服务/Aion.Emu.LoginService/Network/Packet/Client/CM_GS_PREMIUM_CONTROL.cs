namespace Aion.Emu.LoginService
{
	public class CM_GS_PREMIUM_CONTROL : GsClientPacket
	{
		private int accId;

		private int reqId;

		private long reqCost;

		private byte serverId;

		public CM_GS_PREMIUM_CONTROL(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accId = readD();
			reqId = readD();
			reqCost = readQ();
			serverId = readC();
		}

		protected override void runImpl()
		{
			GameShopController.GetInstance().RequestBuy(accId, reqId, reqCost, serverId);
		}
	}
}
