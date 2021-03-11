using System;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class CM_LOGIN : LsClientPacket
	{
		private byte[] data;

		public CM_LOGIN(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			readD();
			data = readB(128);
			readB(35);
		}

		protected override void runImpl()
		{
			if (!Information.IsNothing(data))
			{
				byte[] array = null;
				try
				{
					array = base.Client.RsaDecrypt(data);
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					SendPacket(new SM_LOGIN_FAIL(AionAuthResponse.SYSTEM_ERROR));
					ProjectData.ClearProjectError();
					return;
				}
				string userName = Encoding.UTF8.GetString(array, 64, 32).Replace("\0", "");
				string password = Encoding.UTF8.GetString(array, 96, 32).Replace("\0", "");
				AionAuthResponse aionAuthResponse = AccountController.Login(userName, password, base.Client);
				switch (aionAuthResponse)
				{
				case AionAuthResponse.AUTHED:
					base.Client.State = State.AUTHED_LOGIN;
					base.Client.SessionKey = new SessionKey(base.Client.Account);
					SendPacket(new SM_LOGIN_OK(base.Client.SessionKey));
					break;
				case AionAuthResponse.BAN_ACCOUNT:
					SendPacket(new SM_LOGIN_BAN(1));
					break;
				default:
					SendPacket(new SM_LOGIN_FAIL(aionAuthResponse));
					break;
				}
			}
		}
	}
}
