namespace Aion.Emu.LoginService
{
	public class SM_GS_BAN_RESPONE : GsServerPacket
	{
		private byte _type;

		private int _accountId;

		private string _ip;

		private int _time;

		private int _adminObj;

		private bool _result;

		public SM_GS_BAN_RESPONE(byte type, int accountId, string ip, int time, int adminObj, bool result)
		{
			_type = type;
			_accountId = accountId;
			_ip = ip;
			_time = time;
			_adminObj = adminObj;
			_result = result;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeC(_type);
			writeD(_accountId);
			writeS(_ip);
			writeD(_time);
			writeD(_adminObj);
			writeBC(_result);
		}
	}
}
