using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public class MessageProtocol
	{
		protected ILog log;

		protected MemoryStream read;

		private object _lock;

		private PacketHandleFactory _handle;

		protected PacketHandleFactory Handle => _handle;

		public virtual byte[] Keys
		{
			get;
		}

		public virtual int Key
		{
			get;
		}

		public MessageProtocol(PacketHandleFactory handle)
		{
			log = LogManager.GetLogger(GetType());
			read = new MemoryStream();
			_lock = RuntimeHelpers.GetObjectValue(new object());
			_handle = handle;
		}

		public void ReSet()
		{
			read = new MemoryStream();
		}

		public List<BasePacket> CreatePacket(byte[] buffer)
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				read.Position = read.Length;
				read.Write(buffer, 0, buffer.Length);
				List<BasePacket> packets = new List<BasePacket>();
				while (ReadPacket(ref packets))
				{
				}
				return packets;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(@lock);
				}
			}
		}

		protected virtual bool ReadPacket(ref List<BasePacket> packets)
		{
			read.Position = 0L;
			if (read.Length < 2)
			{
				return false;
			}
			checked
			{
				try
				{
					byte[] array = new byte[2];
					read.Read(array, 0, 2);
					short num = BitConverter.ToInt16(array, 0);
					if (read.Length < num)
					{
						return false;
					}
					short op = (short)read.ReadByte();
					array = new byte[num - 4 + 1];
					read.Read(array, 0, array.Length);
					BasePacket packet = Handle.GetPacket(op, array);
					if (!Information.IsNothing(packet))
					{
						packets.Add(packet);
					}
					if (read.Length - read.Position > 2)
					{
						array = new byte[(int)(read.Length - read.Position - 1) + 1];
						read.Read(array, 0, array.Length);
						read.Dispose();
						ReSet();
						read.Write(array, 0, array.Length);
						return true;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					log.Error((object)ex2);
					ProjectData.ClearProjectError();
				}
				ReSet();
				return false;
			}
		}

		public virtual void EnCrypt(ref byte[] packetData, int v, int length)
		{
		}
	}
}
