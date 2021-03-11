using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class PayRewardService
	{
		private ILog log;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static PayRewardService instance = null;

		private static PayRewardTemplates _payRewards;

		private static List<PayRewardTemplate> _activeRewards = new List<PayRewardTemplate>();

		private PayRewardService()
		{
			log = LogManager.GetLogger(typeof(PayRewardService));
			XmlDataHandle<PayRewardTemplates>.LoadFile(ref _payRewards, ".\\configs\\payrewards\\pay_rewards.xml");
			log.InfoFormat("载入 {0} 个奖励配置!", (object)_payRewards.Count);
			Initialize();
			CronService.GetInstance().scheduler(ResetRewards, "0 0 0 ? * *");
		}

		internal void TaotalConsumHandle(GameConnection gs, int id, string accountName, string playerName, int cost)
		{
			PayRewardTemplate payRewardTemplate = _payRewards[id];
			if (!Information.IsNothing(payRewardTemplate) && payRewardTemplate.IsActive() && payRewardTemplate.type == PayRewardType.TOTAL_CONSUM)
			{
				PayReward payReward = new PayReward(accountName, payRewardTemplate.type, id, 0, DateTime.Parse(payRewardTemplate.start_time), DateTime.Parse(payRewardTemplate.end_time));
				payReward.PayNum = cost;
				DAOManager.PayRewardDAO.InsertPayReward(payReward);
			}
		}

		private void ResetRewards()
		{
			foreach (PayRewardTemplate reward in _payRewards.rewards)
			{
				if (reward.IsActive() && !_activeRewards.Contains(reward))
				{
					_activeRewards.Add(reward);
				}
			}
			foreach (PayRewardTemplate item in ActiveRewards())
			{
				if (!item.IsActive())
				{
					_activeRewards.Remove(item);
				}
			}
			GameService.SendPayRewardInfo(ActiveRewards());
		}

		private void Initialize()
		{
			foreach (PayRewardTemplate reward in _payRewards.rewards)
			{
				if (reward.IsActive())
				{
					_activeRewards.Add(reward);
				}
			}
			log.InfoFormat("初始化 {0} 个有效充值奖励模板!", (object)_activeRewards.Count);
			CronService.GetInstance().scheduler(InitializeReward, "0 0/1 * ? * *");
		}

		private void InitializeReward()
		{
			List<PayInfo> list = DAOManager.PayInfoDAO.LoadPayInfo();
			foreach (PayInfo item in list)
			{
				foreach (PayRewardTemplate item2 in ActiveRewards())
				{
					if (item2.type != PayRewardType.TOTAL_CONSUM)
					{
						PayReward payReward = new PayReward(item.AccountName, item2.type, item2.id, 0, DateTime.Parse(item2.start_time), DateTime.Parse(item2.end_time));
						payReward.PayNum = item.PayNum;
						DAOManager.PayRewardDAO.InsertPayReward(payReward);
						break;
					}
				}
				DAOManager.PayInfoDAO.DeletePay(item.AccountName);
			}
		}

		public void LoadReward(GameConnection gs, Dictionary<string, string> accounts)
		{
			foreach (KeyValuePair<string, string> account in accounts)
			{
				List<PayReward> list = DAOManager.PayRewardDAO.LoadPayReward(account.Key);
				if (list.Count != 0)
				{
					InitializeSendReward(gs, account.Key, account.Value, list);
				}
			}
		}

		private void InitializeSendReward(GameConnection gs, string account, string playerName, List<PayReward> rewards)
		{
			foreach (PayReward reward in rewards)
			{
				PayRewardTemplate payRewardTemplate = _payRewards[reward.RewardId];
				foreach (PayRewardGroup item in payRewardTemplate.rewardGroup)
				{
					if (item.price <= reward.PayNum && item.price > reward.ReadNum)
					{
						PayRewardController.GetInstance().SendRequest(gs, account, playerName, reward, item, payRewardTemplate.pay_name);
					}
				}
			}
		}

		public List<PayRewardTemplate> ActiveRewards()
		{
			return _activeRewards;
		}

		public static PayRewardService GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new PayRewardService();
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
	}
}
