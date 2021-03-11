using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class GamePacketFactory : PacketHandleFactory
	{
		private ILog log;

		private Dictionary<int, GsClientPacket> Rev;

		private Dictionary<Type, int> Sed;

		private static GamePacketFactory instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		public static GamePacketFactory GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new GamePacketFactory();
				}
				return instance;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(@lock);
				}
			}
		}

		private GamePacketFactory()
		{
			log = LogManager.GetLogger(GetType());
			Rev = new Dictionary<int, GsClientPacket>();
			Sed = new Dictionary<Type, int>();
			AddClient(new CM_GS_AUTH(0));
			AddClient(new CM_GS_PLAYER_AUTH(1));
			AddClient(new CM_GC_PLAYER_LOGOUT(2));
			AddServer(typeof(SM_GS_AUTH_RESPONSE), 0);
			AddServer(typeof(SM_GS_PLAYER_AUTH_RESPONSE), 1);
		}

		protected override void AddClient(BasePacket packet)
		{
			if (!Rev.ContainsKey(packet.OpCode))
			{
				Rev.Add(packet.OpCode, (GsClientPacket)packet);
				return;
			}
			throw new ArgumentException("重复的操作码:" + Conversion.Hex(packet.OpCode).PadLeft(4, '0'));
		}

		protected override void AddServer(Type type, int op)
		{
			if (!Sed.ContainsValue(op))
			{
				Sed.Add(type, op);
				return;
			}
			throw new ArgumentException("重复的数据包 -OP:" + Conversion.Hex(op).PadLeft(4, '0') + " -Packet:" + type.Name);
		}

		public override int GetOpCode(Type type)
		{
			if (Sed.ContainsKey(type))
			{
				return Sed[type];
			}
			throw new ArgumentNullException("操作码不存在 " + type.Name);
		}

		public override BasePacket GetPacket(int op, byte[] data)
		{
			if (Rev.ContainsKey(op))
			{
				try
				{
					GsClientPacket obj = (GsClientPacket)Rev[op].ClonePacket();
					obj.PacketData = data;
					return obj;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					log.Error((object)ex2);
					BasePacket result = null;
					ProjectData.ClearProjectError();
					return result;
				}
			}
			if (ChatConfig.UNKNOWN_PACKET)
			{
				log.DebugFormat("收到来自游戏服务器的未知数据包, 操作码:0x{0}\r\n{1}", (object)Conversion.Hex(op).PadLeft(2, '0'), (object)Util.ToHex(data, 0));
			}
			return null;
		}
	}
}
