namespace Aion.Emu.ChatService
{
	public class CM_GS_AUTH : GsClientPacket
	{
		private byte gameId;

		private byte[] ip;

		private string password;

		public CM_GS_AUTH(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			gameId = readC();
			byte length = readC();
			ip = readB(length);
			password = readS();
		}

		protected override void runImpl()
		{
			GsAuthResponse resp = GameService.RegisterServer(gameId, ip, password, base.Client);
			base.Client.SendPacket(new SM_GS_AUTH_RESPONSE(resp));
		}
	}
}
