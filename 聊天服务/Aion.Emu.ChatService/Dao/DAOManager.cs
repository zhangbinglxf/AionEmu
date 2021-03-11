using System;
using Aion.Emu.Common;
using log4net;

namespace Aion.Emu.ChatService
{
	public class DAOManager
	{
		private static ILog log = LogManager.GetLogger(typeof(DAOManager));

		public static GameServerDAO GameDAO;

		public static void Initialize()
		{
			if (!DAO.Connect())
			{
				throw new ArgumentException("无法连接到数据库，请检查连接设置。");
			}
			log.Info((object)"数据库连接成功!");
			GameDAO = new GameServerDAO();
		}
	}
}
