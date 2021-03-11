using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class BannedIP
	{
		private string _ip;

		private DateTime _timeEnd;

		public bool IsActive => !Information.IsNothing(_timeEnd) & (DateTime.Compare(_timeEnd, DateAndTime.Now) > 0);

		public string BanIp => _ip;

		public DateTime TimeEnd
		{
			get
			{
				return _timeEnd;
			}
			set
			{
				_timeEnd = value;
			}
		}

		public BannedIP(string ip, DateTime timeEnd)
		{
			_ip = ip;
			_timeEnd = timeEnd;
		}

		public override bool Equals(object obj)
		{
			if (Information.IsNothing(RuntimeHelpers.GetObjectValue(obj)))
			{
				return false;
			}
			if (obj.GetType() != typeof(BannedIP))
			{
				return false;
			}
			if (Operators.ConditionalCompareObjectEqual(this, obj, TextCompare: false))
			{
				return true;
			}
			BannedIP bannedIP = (BannedIP)obj;
			return BanIp.Equals(bannedIP.BanIp);
		}

		public override int GetHashCode()
		{
			return _ip.GetHashCode();
		}
	}
}
