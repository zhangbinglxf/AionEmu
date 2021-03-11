using System.Runtime.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class Account
	{
		private int _id;

		private string _accountName;

		private string _password;

		private byte _accessLevel;

		private long _memberExp;

		private byte _activated;

		private byte _lastServer;

		private string _lastIp;

		private AccountTime _accountTime;

		private LoginReward _loginReward;

		private LunaInfo _lunaInfo;

		private long _toll;


		public Slb ActiveSlb
		{
			get;
			set;
		}

		public int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public string Name
		{
			get
			{
				return _accountName;
			}
			set
			{
				_accountName = value;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		public byte AccessLevel
		{
			get
			{
				return _accessLevel;
			}
			set
			{
				_accessLevel = value;
			}
		}

		public long MemberShipExp
		{
			get
			{
				return _memberExp;
			}
			set
			{
				_memberExp = value;
			}
		}

		public byte Activated
		{
			get
			{
				return _activated;
			}
			set
			{
				_activated = value;
			}
		}

		public byte LastServer
		{
			get
			{
				return _lastServer;
			}
			set
			{
				_lastServer = value;
			}
		}

		public string LastIp
		{
			get
			{
				return _lastIp;
			}
			set
			{
				_lastIp = value;
			}
		}

		public int ServerId
		{
			get
			{
				return _lastServer;
			}
			set
			{
				_lastServer = checked((byte)value);
			}
		}

		public AccountTime AccountTime
		{
			get
			{
				return _accountTime;
			}
			set
			{
				_accountTime = value;
			}
		}

		public LoginReward LoginReward
		{
			get
			{
				return _loginReward;
			}
			set
			{
				_loginReward = value;
			}
		}

		public long Toll
		{
			get
			{
				return _toll;
			}
			set
			{
				_toll = value;
			}
		}

		public LunaInfo LunaInfo
		{
			get
			{
				return _lunaInfo;
			}
			set
			{
				_lunaInfo = value;
			}
		}

		public Account()
		{
			_lastIp = "127.0.0.1";
		}

		public Account(int id, string accName)
		{
			_lastIp = "127.0.0.1";
			_id = id;
			_accountName = accName;
		}
	}
}
