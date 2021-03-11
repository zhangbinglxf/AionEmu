using System;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class CM_PLAYER_AUTH : CsClientPacket
	{
		private int serverId;

		private int playerId;

		private byte[] identifier;

		private string accountName;

		private byte[] toKen;

		public CM_PLAYER_AUTH(int opcode)
			: base(opcode)
		{
		}

		protected override void readImpl()
		{
			if (Operators.CompareString(ChatConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
			{
				readB(21);
				serverId = readD();
				readD();
				playerId = readD();
				readD();
				readD();
				readD();
			}
			else
			{
				readB(19);
				serverId = readD();
				playerId = readD();
				readD();
				readD();
			}
			checked
			{
				short length = (short)(readH() * 2);
				identifier = readB(length);
				length = (short)(readH() * 2);
				accountName = readS(length);
				length = readH();
				toKen = readB(length);
			}
		}

		protected override void runImpl()
		{
			try
			{
				string[] array = Encoding.Unicode.GetString(identifier).Split('@');
				string[] array2 = array[0].Split(' ');
				string playerName = array2[0];
				if (array2.Length > 1)
				{
					playerName = array2[1];
				}
				identifier = Encoding.Unicode.GetBytes(array[1]);
				GameService.RegissterPlayerToServer(serverId, playerId, toKen, identifier, base.Client, playerName, accountName);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
			}
		}
	}
}
