namespace Aion.Emu.ChatService
{
	public class SM_GS_AUTH_RESPONSE : GsServerPacket
	{
		private GsAuthResponse _resp;

		public SM_GS_AUTH_RESPONSE(GsAuthResponse resp)
		{
			_resp = resp;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeC((int)_resp);
			writeB(SlbService.Address);
			writeH(ChatConfig.CHAT_PORT);
		}
	}
}
