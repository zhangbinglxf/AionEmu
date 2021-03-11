namespace Aion.Emu.LoginService
{
	public class CM_GS_LUNA_UPDATA_KEY : GsClientPacket
	{
		private int responseId;

		private int accountId;

		private int keyCount;

		private int rewardId;

		public CM_GS_LUNA_UPDATA_KEY(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			responseId = readD();
			accountId = readD();
			keyCount = readD();
			rewardId = readD();
		}

		protected override void runImpl()
		{
			LunaInfo lunaInfo = DAOManager.LunaDAO.LoadLuna(accountId);
			checked
			{
				lunaInfo.Keys += keyCount;
				if (rewardId > lunaInfo.TodayRewardId)
				{
					lunaInfo.TodayRewardId = rewardId;
				}
				if (DAOManager.LunaDAO.Store(lunaInfo))
				{
					SendPacket(new SM_GS_LUNA_UPDATA_KEY(responseId, lunaInfo.Keys, lunaInfo.TodayRewardId));
				}
			}
		}
	}
}
