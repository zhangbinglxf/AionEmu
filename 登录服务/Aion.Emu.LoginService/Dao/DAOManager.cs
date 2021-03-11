using System;
using Aion.Emu.Common;
using log4net;

namespace Aion.Emu.LoginService
{
	public class DAOManager
	{
		private static ILog log = LogManager.GetLogger(typeof(DAOManager));

		public static GameServerDAO GameDAO;

		public static AccountDAO AccountDAO;

		public static BannedIpDAO BannedIpDAO;

		public static AccountTimeDAO AccountTimeDAO;

		public static LoginRewardDAO LoginRewardDAO;

		public static LunaDAO LunaDAO;

		public static PayInfoDAO PayInfoDAO;

		public static PayRewardDAO PayRewardDAO;

		public static void Initialize()
		{
			if (!DAO.Connect())
			{
				throw new ArgumentException("无法连接到数据库，请检查连接设置。");
			}
			log.Info((object)"数据库连接成功!");
			GameDAO = new GameServerDAO();
			AccountDAO = new AccountDAO();
			BannedIpDAO = new BannedIpDAO();
			AccountTimeDAO = new AccountTimeDAO();
			LoginRewardDAO = new LoginRewardDAO();
			LunaDAO = new LunaDAO();
			PayInfoDAO = new PayInfoDAO();
			PayRewardDAO = new PayRewardDAO();
		}
	}
}
