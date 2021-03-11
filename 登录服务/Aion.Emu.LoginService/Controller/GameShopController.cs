using System.Runtime.CompilerServices;
using System.Threading;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class GameShopController
	{
		private ILog log;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static GameShopController instance;

		private static byte RESULT_FAIL = 1;

		private static byte RESULT_LOW_POINTS = 2;

		private static byte RESULT_OK = 3;

		private static byte RESULT_ADD = 4;

		public GameShopController()
		{
			log = LogManager.GetLogger("[GAMESHOP]");
		}

		public static GameShopController GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new GameShopController();
				}
				return instance;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(@lock);
				}
			}
		}

		public void RequestBuy(int accountId, int requestId, long cost, byte serverId)
		{
			long tollPoint = DAOManager.AccountDAO.GetTollPoint(accountId);
			GameInfo gameInfo = GameService.GameServerIndoForId(serverId);
			checked
			{
				if (Information.IsNothing(gameInfo) || !gameInfo.IsOnline() || !gameInfo.IsOnGameServer(accountId))
				{
					log.Error((object)("#" + Conversions.ToString(serverId) + "服务器不存在,或帐号ID " + Conversions.ToString(accountId) + " 未登录当前服务器!"));
				}
				else if (cost < 0)
				{
					long points = tollPoint + cost * -1;
					DAOManager.AccountDAO.UpdataToll(accountId, points, 0L);
					gameInfo.GameConnection.SendPacket(new SM_GS_PREMIUM_RESPONSE(requestId, RESULT_ADD, points));
				}
				else if (tollPoint < cost)
				{
					gameInfo.GameConnection.SendPacket(new SM_GS_PREMIUM_RESPONSE(requestId, RESULT_LOW_POINTS, tollPoint));
				}
				else if (DAOManager.AccountDAO.UpdataToll(accountId, tollPoint, cost))
				{
					tollPoint -= cost;
					gameInfo.GameConnection.SendPacket(new SM_GS_PREMIUM_RESPONSE(requestId, RESULT_OK, tollPoint));
					log.InfoFormat("#{0}服务器帐号ID为 {1} 的用户购买物品成功! 收取费用: {2}", (object)serverId, (object)accountId, (object)cost);
				}
				else
				{
					gameInfo.GameConnection.SendPacket(new SM_GS_PREMIUM_RESPONSE(requestId, RESULT_FAIL, tollPoint));
					log.WarnFormat("#{0}服务器帐号ID为 {1} 的用户购买物品失败! 原因: 无法正常收取费用 {2}", (object)serverId, (object)accountId, (object)cost);
				}
			}
		}
	}
}
