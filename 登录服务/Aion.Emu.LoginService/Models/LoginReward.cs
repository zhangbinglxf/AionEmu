using System;

namespace Aion.Emu.LoginService
{
	public class LoginReward
	{
		private int _accountId;

		private int _rewardId;

		private int _rewardIndex;

		private DateTime _lastRewardTime;

		private DateTime _dayRewardTime;

		private DateTime _birthDayRewardTime;

		public int AccountId => _accountId;

		public int RewardID
		{
			get
			{
				return _rewardId;
			}
			set
			{
				_rewardId = value;
			}
		}

		public int RewardIndex
		{
			get
			{
				return _rewardIndex;
			}
			set
			{
				_rewardIndex = value;
			}
		}

		public DateTime LastRewardTime
		{
			get
			{
				return _lastRewardTime;
			}
			set
			{
				_lastRewardTime = value;
			}
		}

		public DateTime NextRewardTime
		{
			get
			{
				return _dayRewardTime;
			}
			set
			{
				_dayRewardTime = value;
			}
		}

		public DateTime NextBirthDayTime
		{
			get
			{
				return _birthDayRewardTime;
			}
			set
			{
				_birthDayRewardTime = value;
			}
		}

		public LoginReward(int accountId, int rewardId, int rewardIndex, DateTime lastTime, DateTime dayTime, DateTime birthDayTime)
		{
			_accountId = accountId;
			_rewardId = rewardId;
			_rewardIndex = rewardIndex;
			_lastRewardTime = lastTime;
			_dayRewardTime = dayTime;
			_birthDayRewardTime = birthDayTime;
		}
	}
}
