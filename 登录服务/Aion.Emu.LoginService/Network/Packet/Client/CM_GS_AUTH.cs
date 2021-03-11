using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Aion.Emu.Common;

namespace Aion.Emu.LoginService
{
	public class CM_GS_AUTH : GsClientPacket
	{
		private byte gameId;

		private string password;

		private int maxPlayers;

		private short port;

		private byte[] ipArray;


		public CM_GS_AUTH(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			gameId = readC();
			byte length = readC();
			ipArray = readB(length);
			readD();
			port = readH();
			maxPlayers = readD();
			password = readS();
		}

		protected override void runImpl()
		{
			GsAuthResponse gsAuthResponse = GameService.RegisterGameServer(base.Client, gameId, ipArray, port, maxPlayers, password);
			SendPacket(new SM_GS_AUTH_RESPONSE(gsAuthResponse));
			if (gsAuthResponse == GsAuthResponse.AUTHED)
			{
				TimeThread.GetInstance().Schedule(delegate
				{
					_Lambda_0024__8_002D0();
				}, 500L);
			}
		}

		[CompilerGenerated]
		private void _Lambda_0024__R8_002D1(object a0)
		{
			_Lambda_0024__8_002D0();
		}

		[CompilerGenerated]
		private void _Lambda_0024__8_002D0()
		{
			SendPacket(new SM_GS_PAY_REWARD_INFO(PayRewardService.GetInstance().ActiveRewards()));
		}
	}
}
