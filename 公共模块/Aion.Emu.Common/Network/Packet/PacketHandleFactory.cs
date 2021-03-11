using System;

namespace Aion.Emu.Common
{
	public abstract class PacketHandleFactory
	{
		protected abstract void AddClient(BasePacket packet);

		protected abstract void AddServer(Type type, int op);

		public abstract int GetOpCode(Type type);

		public abstract BasePacket GetPacket(int op, byte[] data);
	}
}
