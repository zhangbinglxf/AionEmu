using System.IO;
using Aion.Emu.Common;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public abstract class LsServerPacket : BaseServerPacket
	{
		protected LsServerPacket()
		{
			base.OpCode = AionPacketFactory.GetInstance().GetOpCode(GetType());
		}

		public void write(AionConnection con)
		{
			checked
			{
				using MemoryStream memoryStream = new MemoryStream();
				base.Wbuf = new BinaryWriter(memoryStream);
				base.Wbuf.Write((short)0);
				base.Wbuf.Write((byte)base.OpCode);
				writeImpl(con);
				int length = (int)(memoryStream.Length - 4);
				int num = con.CheckSum(length);
				memoryStream.Position = 0L;
				base.Wbuf.Write((short)(num + 2));
				base.PacketData = memoryStream.ToArray();
				base.PacketData = (byte[])Utils.CopyArray(base.PacketData, new byte[num + 1 + 1]);
				byte[] packetData = base.PacketData;
				con.Encrypt(ref packetData, 2, num);
				base.PacketData = packetData;
			}
		}

		protected abstract void writeImpl(AionConnection con);

		public override BasePacket ClonePacket()
		{
			return null;
		}
	}
}
