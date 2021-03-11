using System.IO;
using Aion.Emu.Common;

namespace Aion.Emu.ChatService
{
	public abstract class GsServerPacket : BaseServerPacket
	{
		public GsServerPacket()
		{
			base.OpCode = GamePacketFactory.GetInstance().GetOpCode(GetType());
		}

		public void write(GameConnection con)
		{
			checked
			{
				using MemoryStream memoryStream = new MemoryStream();
				base.Wbuf = new BinaryWriter(memoryStream);
				base.Wbuf.Write((short)0);
				base.Wbuf.Write((byte)base.OpCode);
				writeImpl(con);
				memoryStream.Position = 0L;
				base.Wbuf.Write((short)memoryStream.Length);
				base.PacketData = memoryStream.ToArray();
			}
		}

		protected abstract void writeImpl(GameConnection con);

		public override BasePacket ClonePacket()
		{
			return null;
		}
	}
}
