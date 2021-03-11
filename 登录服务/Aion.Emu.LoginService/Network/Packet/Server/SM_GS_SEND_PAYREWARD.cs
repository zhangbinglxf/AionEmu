namespace Aion.Emu.LoginService
{
	public class SM_GS_SEND_PAYREWARD : GsServerPacket
	{
		private string _payName;

		private PayRequest _request;

		public SM_GS_SEND_PAYREWARD(string payname, PayRequest request)
		{
			_payName = payname;
			_request = request;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_request.RequestId);
			writeS(_payName);
			writeD(_request.RewardGroup.price);
			writeS(_request.PlayerName);
			writeH(_request.RewardGroup.parts.Count);
			foreach (RewardItemPart part in _request.RewardGroup.parts)
			{
				writeD(part.itemId);
				writeD(part.count);
			}
		}
	}
}
