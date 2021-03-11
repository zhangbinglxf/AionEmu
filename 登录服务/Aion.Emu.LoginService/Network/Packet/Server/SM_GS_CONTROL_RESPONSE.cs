namespace Aion.Emu.LoginService
{
	public class SM_GS_CONTROL_RESPONSE : GsServerPacket
	{
		private byte _type;

		private bool _result;

		private string _playerName;

		private long _param;

		private string _adminName;

		private int _accountId;

		public SM_GS_CONTROL_RESPONSE(byte type, bool result, string playerName, int accountId, long param, string adminName)
		{
			_type = type;
			_result = result;
			_playerName = playerName;
			_accountId = accountId;
			_param = param;
			_adminName = adminName;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeC(_type);
			writeBC(_result);
			writeS(_adminName);
			writeS(_playerName);
			writeQ(_param);
			writeD(_accountId);
		}
	}
}
