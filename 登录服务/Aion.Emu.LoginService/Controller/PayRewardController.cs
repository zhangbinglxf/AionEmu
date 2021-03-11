using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class PayRewardController
	{
		private ILog log;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static PayRewardController instance = null;

		private ReaderWriterLock @lock;

		private Dictionary<int, PayRequest> activeRequest;

		private int requestId;


		private PayRewardController()
		{
			log = LogManager.GetLogger("[PAYREWARD]");
			@lock = new ReaderWriterLock();
			requestId = 0;
			activeRequest = new Dictionary<int, PayRequest>();
		}

		public static PayRewardController GetInstance()
		{
			object obj = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new PayRewardController();
				}
				return instance;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
		}

		public void FinishRequest(int backId, bool sendSuccess)
		{
			try
			{
				PayRequest value = null;
				if (activeRequest.TryGetValue(backId, out value))
				{
					activeRequest.Remove(backId);
					if (sendSuccess)
					{
						log.InfoFormat("角色名:{0} 充值或消费 {1} 商城币奖励发放成功!", (object)value.PlayerName, (object)value.RewardGroup.price);
						if (DAOManager.PayRewardDAO.GetReadPrice(value.PayReward) < value.RewardGroup.price)
						{
							DAOManager.PayRewardDAO.UpdataPayReward(value.PayReward, value.RewardGroup.price);
						}
					}
				}
				else
				{
					log.Debug((object)("未知的返回操作ID:" + Conversions.ToString(backId) + " 服务器操作状态:" + Conversions.ToString(sendSuccess)));
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2.ToString());
				ProjectData.ClearProjectError();
			}
		}

		internal void SendRequest(GameConnection gs, string account, string playerName, PayReward pay, PayRewardGroup rd, string payName)
		{
			try
			{
				Interlocked.Increment(ref requestId);
				PayRequest payRequest = new PayRequest(requestId, account, playerName, pay, rd);
				activeRequest.Add(requestId, payRequest);
				gs.SendPacket(new SM_GS_SEND_PAYREWARD(payName, payRequest));
				log.InfoFormat("发送: 角色名:{0} 充值或消费 {1} 商城币奖励 操作ID:{2}", (object)playerName, (object)rd.price, (object)payRequest.RequestId);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2.ToString());
				ProjectData.ClearProjectError();
			}
		}
	}
}
