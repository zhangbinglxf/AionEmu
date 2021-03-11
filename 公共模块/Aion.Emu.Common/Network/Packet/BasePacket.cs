using System.IO;

namespace Aion.Emu.Common
{
	public abstract class BasePacket
	{
		private int _opCode;

		private byte[] _data;

		private PacketType _packetType;

		private BinaryReader _reader;

		private BinaryWriter _write;

		public BinaryReader Rbuf
		{
			get
			{
				return _reader;
			}
			set
			{
				_reader = value;
			}
		}

		public BinaryWriter Wbuf
		{
			get
			{
				return _write;
			}
			set
			{
				_write = value;
			}
		}

		public int OpCode
		{
			get
			{
				return _opCode;
			}
			set
			{
				_opCode = value;
			}
		}

		public byte[] PacketData
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}

		public PacketType PacketType => _packetType;

		protected BasePacket(PacketType packetType)
		{
			_packetType = packetType;
		}

		protected BasePacket(PacketType packetType, int opcode)
		{
			_packetType = packetType;
			_opCode = opcode;
		}

		public BasePacket(PacketType packetType, int opcode, byte[] data)
		{
			_packetType = packetType;
			_opCode = opcode;
			_data = data;
		}

		public abstract BasePacket ClonePacket();
	}
}
