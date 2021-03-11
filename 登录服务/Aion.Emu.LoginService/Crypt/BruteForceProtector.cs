using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class BruteForceProtector
	{
		private static Dictionary<string, FailedLoginInfo> banlist = new Dictionary<string, FailedLoginInfo>();

		public static void AddFailedLogin(string acc, string ip)
		{
			FailedLoginInfo value = null;
			checked
			{
				if (banlist.TryGetValue(ip, out value))
				{
					if (DateAndTime.Now.Subtract(value.Time).TotalMilliseconds > (double)(LoginConfig.PASSWORD_CHECK_TIME * 60 * 1000))
					{
						banlist.Remove(ip);
						banlist.Add(ip, new FailedLoginInfo(acc, ip));
					}
					else if (value.Count + 1 >= LoginConfig.PASSWORD_CHECK_COUNT)
					{
						banlist.Remove(ip);
						BannedIpController.BanIP(ip, DateAndTime.Now.AddMinutes(LoginConfig.PASSWORD_CHECK_BANTIME));
					}
					else
					{
						value.IncreseCount();
					}
				}
				else
				{
					banlist.Add(ip, new FailedLoginInfo(acc, ip));
				}
			}
		}
	}
}
