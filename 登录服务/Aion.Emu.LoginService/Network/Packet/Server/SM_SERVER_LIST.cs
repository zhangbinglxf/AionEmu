using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class SM_SERVER_LIST : LsServerPacket
	{
		protected override void writeImpl(AionConnection con)
		{
			int id = con.Account.Id;
			int num = 0;
			Dictionary<int, int> dictionary = GameService.CharacterCountsFor(id);
			writeC(dictionary.Count);
			writeC(con.Account.LastServer);
			foreach (GameInfo allGameServer in GameService.AllGameServers)
			{
				if (allGameServer.ServerId > num)
				{
					num = allGameServer.ServerId;
				}
				writeC(allGameServer.ServerId);
				if (allGameServer.IsOnline())
				{
					writeB(allGameServer.SlbAddress(con.Account));
				}
				else
				{
					writeB(4);
				}
				writeD(allGameServer.Port);
				writeC(0);
				writeC(1);
				writeH(allGameServer.CurrentPlayers());
				writeH(allGameServer.MaxPlayers);
				writeBC(allGameServer.IsOnline());
				writeD(1);
				writeC(0);
			}
			checked
			{
				writeH(num + 1);
				writeC(1);
				int num2 = num;
				for (int i = 1; i <= num2; i++)
				{
					if (dictionary.ContainsKey(i))
					{
						writeC(dictionary[i]);
					}
					else
					{
						writeC(0);
					}
				}
				if (Operators.CompareString(LoginConfig.CLIENT_VERSION, "5.x", TextCompare: false) == 0)
				{
					writeC(0);
					writeH(0);
					writeD(1097983527);
					writeD(0);
				}
			}
		}
	}
}
