using System;
using System.Text;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public abstract class BaseClientPacket<T> : BasePacket
	{
		protected ILog log;

		private T _client;

		public T Client
		{
			get
			{
				return _client;
			}
			set
			{
				_client = value;
			}
		}

		protected int RemainingLength
		{
			get
			{
				long position = base.Rbuf.BaseStream.Position;
				return checked((int)(base.Rbuf.BaseStream.Length - position));
			}
		}

		protected BaseClientPacket(int opcode)
			: base(PacketType.CLIENT, opcode)
		{
			log = LogManager.GetLogger(GetType());
		}

		protected int readD()
		{
			try
			{
				return base.Rbuf.ReadInt32();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 D"));
				ProjectData.ClearProjectError();
			}
			return 0;
		}

		protected byte readC()
		{
			try
			{
				return base.Rbuf.ReadByte();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 C"));
				ProjectData.ClearProjectError();
			}
			return 0;
		}

		protected short readH()
		{
			try
			{
				return base.Rbuf.ReadInt16();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 H"));
				ProjectData.ClearProjectError();
			}
			return 0;
		}

		protected float readF()
		{
			try
			{
				return base.Rbuf.ReadSingle();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 F"));
				ProjectData.ClearProjectError();
			}
			return 0f;
		}

		protected long readQ()
		{
			try
			{
				return base.Rbuf.ReadInt64();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 Q"));
				ProjectData.ClearProjectError();
			}
			return 0L;
		}

		protected string readS()
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				while (true)
				{
					int num = base.Rbuf.ReadInt16();
					if (num != 0)
					{
						stringBuilder.Append(Strings.ChrW(num));
						continue;
					}
					break;
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 String 剩余字节长度小于 2"));
				ProjectData.ClearProjectError();
			}
			return stringBuilder.ToString();
		}

		protected string readS(int size)
		{
			string result = "";
			try
			{
				byte[] bytes = base.Rbuf.ReadBytes(size);
				result = Encoding.Unicode.GetString(bytes);
				return result;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 String 剩余字节长度小于" + Conversions.ToString(size) + "无法读取定长字符串"));
				ProjectData.ClearProjectError();
				return result;
			}
		}

		protected byte[] readB(int length)
		{
			try
			{
				return base.Rbuf.ReadBytes(length);
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 Byte() 剩余字节长度小于 " + Conversions.ToString(length) + " 无法读取指定大小字节数组"));
				ProjectData.ClearProjectError();
			}
			return new byte[0];
		}

		protected bool readBC()
		{
			try
			{
				return base.Rbuf.ReadBoolean();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)(GetType().Name + ": 丢失 Boolean"));
				ProjectData.ClearProjectError();
			}
			return false;
		}
	}
}
