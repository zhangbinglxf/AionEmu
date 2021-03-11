using System;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class CM_GS_BAN : GsClientPacket
	{
		private byte type;

		private int accountId;

		private string ip;

		private int time;

		private int adminObj;

		public CM_GS_BAN(int opcode, State state)
			: base(opcode, state)
		{
		}

		protected override void readImpl()
		{
			type = readC();
			accountId = readD();
			ip = readS();
			time = readD();
			adminObj = readD();
		}

		protected override void runImpl()
		{
			if (type == 2)
			{
				if (time > 0)
				{
					DateTime dateTime = DateAndTime.Now.AddMinutes(time);
					BannedIpController.BanIP(ip, dateTime);
				}
				else
				{
					BannedIpController.UnBan(ip);
				}
				SendPacket(new SM_GS_BAN_RESPONE(type, accountId, ip, time, adminObj, result: true));
			}
			else
			{
				log.Warn((object)"未实现的封禁方法!");
			}
		}
	}
}
