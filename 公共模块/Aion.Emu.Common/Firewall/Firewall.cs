using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Aion.Emu.Common
{
	public class Firewall
	{
		private ConnectInfo ci;

		private Dictionary<string, ConnectInfo> cis;

		public Firewall(bool isServer)
		{
			if (isServer)
			{
				cis = new Dictionary<string, ConnectInfo>();
			}
			else
			{
				ci = new ConnectInfo();
			}
		}

		public bool CheckConection(string ip)
		{
			if (!cis.ContainsKey(ip))
			{
				cis.Add(ip, new ConnectInfo());
				return true;
			}
			ConnectInfo connectInfo = cis[ip];
			if (DateAndTime.Now.Subtract(connectInfo.ResetTime).TotalSeconds > (double)FirewallConfig.CONNECT_CHECKTIME)
			{
				cis.Remove(ip);
				return true;
			}
			checked
			{
				if (connectInfo.ConnectCount + 1 >= FirewallConfig.CYCLE_CONNECT_MAXCOUNT)
				{
					return false;
				}
				connectInfo.ConnectCount++;
				return true;
			}
		}

		public bool CheckStream(string ip)
		{
			return false;
		}

		private void banIp(string ip)
		{
		}
	}
}
