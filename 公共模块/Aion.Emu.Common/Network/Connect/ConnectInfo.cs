using System;
using Microsoft.VisualBasic;

namespace Aion.Emu.Common
{
	public class ConnectInfo
	{
		private DateTime _connectTime;

		private int _connectCount;

		private int _readLength;

		private DateTime _resetTime;

		public DateTime ConnectTime
		{
			get
			{
				return _connectTime;
			}
			set
			{
				_connectTime = value;
			}
		}

		public DateTime ResetTime
		{
			get
			{
				return _resetTime;
			}
			set
			{
				_resetTime = value;
			}
		}

		public int ConnectCount
		{
			get
			{
				return _connectCount;
			}
			set
			{
				_connectCount = value;
			}
		}

		public int ReadLength
		{
			get
			{
				return _readLength;
			}
			set
			{
				_readLength = value;
			}
		}

		public ConnectInfo()
		{
			_connectCount = 1;
			_connectTime = DateAndTime.Now;
			_resetTime = DateAndTime.Now;
		}
	}
}
