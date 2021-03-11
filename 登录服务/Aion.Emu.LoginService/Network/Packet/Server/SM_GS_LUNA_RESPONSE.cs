namespace Aion.Emu.LoginService
{
	public class SM_GS_LUNA_RESPONSE : GsServerPacket
	{
		private int _responseId;

		private bool _isSuccess;

		private long _point;

		private int _usePoint;

		public SM_GS_LUNA_RESPONSE(int responseId, bool isSuccess, long point, int usePoint)
		{
			_responseId = responseId;
			_isSuccess = isSuccess;
			_point = point;
			_usePoint = usePoint;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_responseId);
			writeBC(_isSuccess);
			writeQ(_point);
			writeD(_usePoint);
		}
	}
}
