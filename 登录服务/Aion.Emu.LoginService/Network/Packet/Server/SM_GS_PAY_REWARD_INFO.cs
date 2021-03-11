using System.Collections.Generic;

namespace Aion.Emu.LoginService
{
	public class SM_GS_PAY_REWARD_INFO : GsServerPacket
	{
		private List<PayRewardTemplate> _rewards;

		public SM_GS_PAY_REWARD_INFO(List<PayRewardTemplate> rewards)
		{
			_rewards = rewards;
		}

		protected override void writeImpl(GameConnection con)
		{
			writeH(_rewards.Count);
			foreach (PayRewardTemplate reward in _rewards)
			{
				writeH(reward.id);
				writeC((int)reward.type);
				writeS(reward.announce);
			}
		}
	}
}
