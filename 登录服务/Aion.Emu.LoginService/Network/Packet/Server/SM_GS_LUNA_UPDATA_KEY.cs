namespace Aion.Emu.LoginService
{
	public class SM_GS_LUNA_UPDATA_KEY : GsServerPacket
	{
		private int _responseId;

		private int _keyCount;

		private int _rewardId;

		public SM_GS_LUNA_UPDATA_KEY(int responId, int keyCount, int rewardId)
		{
			_responseId = responId;
			_keyCount = keyCount;
			_rewardId = rewardId;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_responseId);
			writeD(_keyCount);
			writeD(_rewardId);
		}
	}
}
