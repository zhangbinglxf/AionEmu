using System;

namespace Aion.Emu.Common
{
	public abstract class BaseServerPacket : BasePacket
	{
		protected BaseServerPacket()
			: base(PacketType.SERVER)
		{
		}

		protected BaseServerPacket(int opcode)
			: base(PacketType.SERVER, opcode)
		{
		}

		protected void writeD(int value)
		{
			base.Wbuf.Write(value);
		}

		protected void writeH(int value)
		{
			base.Wbuf.Write(checked((short)value));
		}

		protected void writeC(int value)
		{
			base.Wbuf.Write(checked((byte)value));
		}

		protected void writeBC(bool value)
		{
			base.Wbuf.Write(value);
		}

		protected void writeF(float value)
		{
			base.Wbuf.Write(value);
		}

		protected void writeQ(long value)
		{
			base.Wbuf.Write(value);
		}

		protected void writeS(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				writeH(0);
				return;
			}
			foreach (char value2 in value)
			{
				base.Wbuf.Write(BitConverter.GetBytes(value2));
			}
			writeH(0);
		}

		protected void writeS(string value, int length)
		{
			if (string.IsNullOrEmpty(value))
			{
				writeB(length);
				return;
			}
			checked
			{
				int num = value.Length * 2 + 2;
				if (num < length)
				{
					writeS(value);
					writeB(length - num);
				}
				else
				{
					value = value.Substring(0, (int)Math.Round((double)length / 2.0 - 1.0));
					writeS(value);
				}
			}
		}

		protected void writeB(byte[] value)
		{
			base.Wbuf.Write(value);
		}

		protected void writeB(int value)
		{
			byte[] buffer = new byte[checked(value - 1 + 1)];
			base.Wbuf.Write(buffer);
		}
	}
}
