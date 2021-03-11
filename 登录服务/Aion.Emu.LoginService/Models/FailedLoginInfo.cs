using System;

namespace Aion.Emu.LoginService
{
	internal class FailedLoginInfo
	{
		private int _count;

		private string _account;

		private string _ip;

		private DateTime _time;

		public int Count => _count;

		public string AccountName => _account;

		public string Ip => _ip;

		public DateTime Time => _time;

		public FailedLoginInfo(string acc, string ip)
		{
			_account = acc;
			_ip = ip;
			_count = 1;
			_time = DateTime.UtcNow;
		}

		public void IncreseCount()
		{
			checked
			{
				_count++;
			}
		}
	}
}
