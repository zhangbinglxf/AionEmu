using System.Collections.Generic;

namespace Aion.Emu.LoginService
{
	public class CM_GS_LOAD_PAYREWARD : GsClientPacket
	{
		private Dictionary<string, string> accounts;

		public CM_GS_LOAD_PAYREWARD(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			accounts = new Dictionary<string, string>();
			checked
			{
				int num = readH() - 1;
				for (int i = 0; i <= num; i++)
				{
					accounts.Add(readS(), readS());
				}
			}
		}

		protected override void runImpl()
		{
			PayRewardService.GetInstance().LoadReward(base.Client, accounts);
		}
	}
}
