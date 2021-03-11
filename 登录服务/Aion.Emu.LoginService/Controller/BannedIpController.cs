using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class BannedIpController
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<BannedIP, bool> _0024I5_002D0;

			public static Action<BannedIP> _0024I5_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			internal bool _Lambda_0024__5_002D0(BannedIP b)
			{
				return DateTime.Compare(b.TimeEnd, DateAndTime.Now) <= 0;
			}

			internal void _Lambda_0024__5_002D1(BannedIP b)
			{
				Banips.Remove(b.BanIp);
				DAOManager.BannedIpDAO.DeleteBannedIp(b.BanIp);
			}
		}

		private static ILog log = LogManager.GetLogger(typeof(BannedIpController));

		private static Dictionary<string, BannedIP> Banips = new Dictionary<string, BannedIP>();

		public static void Initialize()
		{
			Banips.Clear();
			BannedIpDAO bannedIpDAO = DAOManager.BannedIpDAO;
			object banips = Banips;
			bannedIpDAO.LoadBanedIP(ref banips);
			Banips = (Dictionary<string, BannedIP>)banips;
			log.InfoFormat("载入 {0} 个禁止连接的IP地址", (object)Banips.Count);
			Clean();
		}

		private static void Clean()
		{
			Banips.Values.Where((_Closure_0024__._0024I5_002D0 != null) ? _Closure_0024__._0024I5_002D0 : (_Closure_0024__._0024I5_002D0 = (BannedIP b) => DateTime.Compare(b.TimeEnd, DateAndTime.Now) <= 0)).ToList().ForEach((_Closure_0024__._0024I5_002D1 != null) ? _Closure_0024__._0024I5_002D1 : (_Closure_0024__._0024I5_002D1 = delegate(BannedIP b)
			{
				Banips.Remove(b.BanIp);
				DAOManager.BannedIpDAO.DeleteBannedIp(b.BanIp);
			}));
		}

		public static bool IsBannedIp(string ip)
		{
			BannedIP value = null;
			if (Banips.TryGetValue(ip, out value))
			{
				if (DateTime.Compare(value.TimeEnd, DateAndTime.Now) > 0)
				{
					return true;
				}
				Banips.Remove(ip);
			}
			return false;
		}

		public static bool BanIP(string ip, DateTime time)
		{
			if (ip.Equals("127.0.0.1"))
			{
				return false;
			}
			BannedIP bannedIP = DAOManager.BannedIpDAO.InsertBanIp(ip, time);
			if (!Information.IsNothing(bannedIP))
			{
				Banips.Add(ip, bannedIP);
				return true;
			}
			return false;
		}

		public static bool UnBan(string ip)
		{
			if (Banips.ContainsKey(ip))
			{
				DAOManager.BannedIpDAO.DeleteBannedIp(ip);
				Banips.Remove(ip);
				return true;
			}
			return false;
		}
	}
}
