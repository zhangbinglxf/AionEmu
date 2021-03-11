using System;

namespace Aion.Emu.LoginService
{
	public class PayReward
	{
		private string _account;

		private PayRewardType _rewardType;

		private int _payNum;

		private int _rewardId;

		private int _read;


		private DateTime _startTime;

		private DateTime _endTime;

		public string AccountName => _account;

		public PayRewardType RewardType => _rewardType;

		public int PayNum
		{
			get
			{
				return _payNum;
			}
			set
			{
				_payNum = value;
			}
		}

		public int RewardId => _rewardId;

		public int ReadNum
		{
			get
			{
				return _read;
			}
			set
			{
				_read = value;
			}
		}

		public DateTime StartTime => _startTime;

		public DateTime EndTime => _endTime;

		public PayReward(string account, PayRewardType rewardType, int rewardId, int readNum, DateTime startTime, DateTime endTime)
		{
			_account = account;
			_rewardType = rewardType;
			_rewardId = rewardId;
			_read = readNum;
			_startTime = startTime;
			_endTime = endTime;
		}
	}
}
