using Aion.Emu.Common;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class SM_GS_ACCOUNT_AUTH_RESPONSE : GsServerPacket
	{
		private bool _ok;

		private int _accountId;

		private Account _acc;

		private AccountTime _time;

		private LoginReward _reward;

		private LunaInfo _luna;

		public SM_GS_ACCOUNT_AUTH_RESPONSE(int accountId, bool ok, Account acc)
		{
			_accountId = accountId;
			_ok = ok;
			_acc = acc;
			if (!Information.IsNothing(acc))
			{
				_time = acc.AccountTime;
				_reward = acc.LoginReward;
				_luna = acc.LunaInfo;
			}
		}

		protected override void writeImpl(GameConnection con)
		{
			writeD(_accountId);
			writeBC(_ok);
			if (_ok)
			{
				writeS(_acc.Name);
				writeQ(0L);
				writeQ(0L);
				writeC(_acc.AccessLevel);
				if (Operators.CompareString(LoginConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
				{
					writeQ(_acc.MemberShipExp);
					writeQ(0L);
					writeQ(Util.GetTime(_time.CreateTime));
					writeC(_reward.RewardID);
					writeC(_reward.RewardIndex);
					writeQ(Util.GetTime(_reward.LastRewardTime));
					writeQ(Util.GetTime(_reward.NextRewardTime));
					writeQ(Util.GetTime(_reward.NextBirthDayTime));
					writeQ(_luna.Luna);
					writeD(_luna.TodayUse);
					writeD(_luna.Keys);
					writeD(_luna.TodayRewardId);
				}
				else
				{
					writeC(0);
					writeQ(_acc.Toll);
				}
			}
		}
	}
}
