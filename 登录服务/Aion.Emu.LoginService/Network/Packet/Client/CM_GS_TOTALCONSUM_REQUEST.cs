namespace Aion.Emu.LoginService
{
	public class CM_GS_TOTALCONSUM_REQUEST : GsClientPacket
	{
		private int id;

		private string accountName;

		private string playerName;

		private int cost;

		public CM_GS_TOTALCONSUM_REQUEST(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			id = readD();
			accountName = readS();
			playerName = readS();
			cost = readD();
		}

		protected override void runImpl()
		{
			PayRewardService.GetInstance().TaotalConsumHandle(base.Client, id, accountName, playerName, cost);
		}
	}
}
