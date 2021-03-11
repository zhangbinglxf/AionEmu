using System;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class AccountTime
	{
		private int _accountId;

		private DateTime _createTime;

		private DateTime _lastLoginTime;

		private uint _expirationTime;

		private DateTime _expirationPeriod;

		private ExpirationType _expiration_type;

		public int AccountId => _accountId;

		public DateTime CreateTime
		{
			get
			{
				return _createTime;
			}
			set
			{
				_createTime = value;
			}
		}

		public DateTime LastLoginTime
		{
			get
			{
				return _lastLoginTime;
			}
			set
			{
				_lastLoginTime = value;
			}
		}

		public uint ExpirationTime
		{
			get
			{
				return _expirationTime;
			}
			set
			{
				_expirationTime = value;
			}
		}

		public DateTime ExpirationPeriod
		{
			get
			{
				return _expirationPeriod;
			}
			set
			{
				_expirationPeriod = value;
			}
		}

		public ExpirationType ExpirationType
		{
			get
			{
				return _expiration_type;
			}
			set
			{
				_expiration_type = value;
			}
		}

		public AccountTime(int id)
		{
			_accountId = id;
		}

		public void RunExpiration()
		{
			long num = 0L;
			checked
			{
				switch (ExpirationType)
				{
				case ExpirationType.TIMING:
					num = unchecked((long)ExpirationTime) - (long)Math.Round(DateAndTime.Now.Subtract(LastLoginTime).TotalMilliseconds / 1000.0);
					break;
				case ExpirationType.PERIOD:
					num = (long)Math.Round(ExpirationPeriod.Subtract(DateAndTime.Now).TotalMilliseconds / 1000.0);
					break;
				}
				if (num <= 0)
				{
					ExpirationService.GetInstance().Remove(_accountId);
					GameService.KickAccountFromGameServer(_accountId);
					return;
				}
				switch (num)
				{
				case 1L:
				case 2L:
				case 3L:
				case 4L:
				case 5L:
				case 15L:
				case 30L:
				case 60L:
				case 300L:
				case 600L:
				case 900L:
				case 1800L:
					GameService.SendExpiration(_accountId, num);
					break;
				}
			}
		}
	}
}
