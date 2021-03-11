using System;

namespace Aion.Emu.LoginService
{
	public class LunaInfo
	{
		private int _accountId;

		private long _luna;

		private int _todayUse;

		private int _keys;

		private int _rewardId;

		private DateTime _nexttime;

		public int AccountId => _accountId;

		public long Luna
		{
			get
			{
				return _luna;
			}
			set
			{
				_luna = value;
			}
		}

		public int TodayUse
		{
			get
			{
				return _todayUse;
			}
			set
			{
				_todayUse = value;
			}
		}

		public int Keys
		{
			get
			{
				return _keys;
			}
			set
			{
				_keys = value;
			}
		}

		public int TodayRewardId
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

		public DateTime NextTime
		{
			get
			{
				return _nexttime;
			}
			set
			{
				_nexttime = value;
			}
		}

		public LunaInfo(int accountId, long luna, int todayUse, int keys, int rewardId, DateTime time)
		{
			_accountId = accountId;
			_luna = luna;
			_todayUse = todayUse;
			_keys = keys;
			_rewardId = rewardId;
			_nexttime = time;
		}
	}
}
