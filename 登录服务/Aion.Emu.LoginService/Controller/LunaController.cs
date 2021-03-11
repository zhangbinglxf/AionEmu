using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class LunaController
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Action _0024I7_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			internal void _Lambda_0024__7_002D0()
			{
				DAOManager.LunaDAO.UpdataLuna();
			}
		}

		private ILog log;

		private static LunaController instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private const string UPDATE_TIME = "0 0 9 ? * *";

		public LunaController()
		{
			log = LogManager.GetLogger(typeof(LunaController));
		}

		public static LunaController GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new LunaController();
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

		public void Initial()
		{
			CronService.GetInstance().scheduler((_Closure_0024__._0024I7_002D0 != null) ? _Closure_0024__._0024I7_002D0 : (_Closure_0024__._0024I7_002D0 = delegate
			{
				DAOManager.LunaDAO.UpdataLuna();
			}), "0 0 9 ? * *");
			log.Info((object)"露娜系统统计服务初始化!");
		}

		public void Request(GameConnection client, int responseId, int activeId, int accountId, long cost)
		{
			LunaInfo lunaInfo = DAOManager.LunaDAO.LoadLuna(accountId);
			DateTime now = DateTime.Now;
			DateTime nextTime = new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day, 9, 0, 0);
			if (now.Hour > 9)
			{
				nextTime = nextTime.AddDays(1.0);
			}
			if (cost > 0 && lunaInfo.Luna < cost)
			{
				log.InfoFormat("#{0}服务器ID为{1}的用户使用{2}露娜失败!账户余额不足!", (object)client.GameServerId, (object)accountId, (object)cost);
				client.SendPacket(new SM_GS_LUNA_RESPONSE(responseId, isSuccess: false, lunaInfo.Luna, lunaInfo.TodayUse));
				return;
			}
			checked
			{
				long num = cost * -1;
				lunaInfo.Luna += num;
				if (num < 0)
				{
					LunaInfo lunaInfo2;
					(lunaInfo2 = lunaInfo).TodayUse = (int)(lunaInfo2.TodayUse + cost);
					lunaInfo.NextTime = nextTime;
				}
				if (DAOManager.LunaDAO.Store(lunaInfo))
				{
					if (num > 0)
					{
						log.InfoFormat("#{0}服务器ID为{1}的用户充值{2}露娜!账户余额:{3}", new object[4]
						{
							client.GameServerId,
							accountId,
							num,
							lunaInfo.Luna
						});
					}
					else
					{
						log.InfoFormat("#{0}服务器ID为{1}的用户消费{2}露娜!账户余额:{3}", new object[4]
						{
							client.GameServerId,
							accountId,
							cost,
							lunaInfo.Luna
						});
					}
					client.SendPacket(new SM_GS_LUNA_RESPONSE(responseId, isSuccess: true, lunaInfo.Luna, lunaInfo.TodayUse));
				}
			}
		}
	}
}
