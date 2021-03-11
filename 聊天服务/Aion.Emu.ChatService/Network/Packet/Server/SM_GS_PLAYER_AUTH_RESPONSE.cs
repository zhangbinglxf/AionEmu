namespace Aion.Emu.ChatService
{
	public class SM_GS_PLAYER_AUTH_RESPONSE : GsServerPacket
	{
		private ChatClient _client;

		public SM_GS_PLAYER_AUTH_RESPONSE(ChatClient client)
		{
			_client = client;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_client.PlayerId);
			writeC(_client.ToKen.Length);
			writeB(_client.ToKen);
		}
	}
}
