namespace Aion.Emu.ChatService
{
	public class CM_GS_PLAYER_AUTH : GsClientPacket
	{
		private int playerId;

		private string accountName;

		private string playerName;

		public CM_GS_PLAYER_AUTH(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			playerId = readD();
			accountName = readS();
			playerName = readS();
		}

		protected override void runImpl()
		{
			base.Client.RegisterPlayer(playerId, accountName, playerName);
		}
	}
}
