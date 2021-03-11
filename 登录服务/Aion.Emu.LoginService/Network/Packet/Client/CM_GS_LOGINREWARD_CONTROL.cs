using System;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class CM_GS_LOGINREWARD_CONTROL : GsClientPacket
	{
		private int accountId;

		private bool updataDay;

		private bool updataBirth;

		private int rewardId;

		private int rewardIndex;

		public CM_GS_LOGINREWARD_CONTROL(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accountId = readD();
			rewardId = readC();
			rewardIndex = readC();
			updataDay = readBC();
			updataBirth = readBC();
		}

		protected override void runImpl()
		{
			LoginReward loginReward = DAOManager.LoginRewardDAO.LoadLoginReward(accountId);
			AccountTime accountTime = DAOManager.AccountTimeDAO.LoadAccountTime(accountId);
			loginReward.RewardID = rewardId;
			loginReward.RewardIndex = rewardIndex;
			if (updataDay)
			{
				loginReward.LastRewardTime = DateAndTime.Now;
				loginReward.NextRewardTime = DateAndTime.DateAdd(DateInterval.Day, 1.0, new DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, DateAndTime.Now.Day, 8, 0, 0));
				log.InfoFormat("帐号ID为 {0} 的用户领取了切克隆每日奖励!", (object)accountId);
			}
			if (updataBirth)
			{
				loginReward.NextBirthDayTime = DateAndTime.DateAdd(DateInterval.Year, 1.0, new DateTime(DateAndTime.Now.Year, accountTime.CreateTime.Month, accountTime.CreateTime.Day));
				log.InfoFormat("帐号ID为 {0} 的用户领取了切克隆生日奖励!", (object)accountId);
			}
			DAOManager.LoginRewardDAO.UpdataLoginReward(loginReward);
		}
	}
}
