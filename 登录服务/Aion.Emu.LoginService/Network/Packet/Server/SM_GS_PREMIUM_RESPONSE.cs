namespace Aion.Emu.LoginService
{
	public class SM_GS_PREMIUM_RESPONSE : GsServerPacket
	{
		private int _reqId;

		private int _result;

		private long _points;

		public SM_GS_PREMIUM_RESPONSE(int requstId, int result, long points)
		{
			_reqId = requstId;
			_result = result;
			_points = points;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_reqId);
			writeD(_result);
			writeQ(_points);
		}
	}
}
