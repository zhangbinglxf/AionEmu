namespace Aion.Emu.LoginService
{
	public class PayRequest
	{
		private int _requestId;

		private string _account;

		private string _playerName;

		private PayReward _reward;

		private PayRewardGroup _rewardGroup;

		public int RequestId => _requestId;

		public string AccountName => _account;

		public string PlayerName => _playerName;

		public PayReward PayReward => _reward;

		public PayRewardGroup RewardGroup => _rewardGroup;

		public PayRequest(int requestId, string account, string playerName, PayReward reward, PayRewardGroup rewardGroup)
		{
			_requestId = requestId;
			_account = account;
			_playerName = playerName;
			_reward = reward;
			_rewardGroup = rewardGroup;
		}
	}
}
