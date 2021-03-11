namespace Aion.Emu.LoginService
{
	public class CM_GS_CHARACTER : GsClientPacket
	{
		private int accountId;

		private int characterCount;

		public CM_GS_CHARACTER(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accountId = readD();
			characterCount = readC();
		}

		protected override void runImpl()
		{
			GameService.AddGSCharacterCountFor(accountId, base.Client.GameServerId, characterCount);
		}
	}
}
