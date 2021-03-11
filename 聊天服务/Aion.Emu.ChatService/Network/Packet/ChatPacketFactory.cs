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
	public class ChatPacketFactory : PacketHandleFactory
	{
		private ILog log;

		private Dictionary<int, CsClientPacket> Rev;

		private Dictionary<Type, int> Sed;

		private static ChatPacketFactory instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		public static ChatPacketFactory GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new ChatPacketFactory();
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

		private ChatPacketFactory()
		{
			log = LogManager.GetLogger(GetType());
			Rev = new Dictionary<int, CsClientPacket>();
			Sed = new Dictionary<Type, int>();
			AddClient(new CM_VERSION_CHECK(48));
			AddClient(new CM_PLAYER_AUTH(5));
			AddClient(new CM_CHANNEL_REQUEST(16));
			AddClient(new CM_QUIT_CHANNEL(18));
			AddClient(new CM_CHANNEL_MESSAGE(24));
			AddClient(new CM_UNK_CHECK(44));
			AddClient(new CM_UNK_PING(255));
			AddServer(typeof(SM_VERSION_CHECK), 49);
			AddServer(typeof(SM_PLAYER_AUTH_RESPONSE), 2);
			AddServer(typeof(SM_CHANNEL_RESPONSE), 17);
			AddServer(typeof(SM_CHANNEL_MESSAGE), 26);
		}

		protected override void AddClient(BasePacket packet)
		{
			if (!Rev.ContainsKey(packet.OpCode))
			{
				Rev.Add(packet.OpCode, (CsClientPacket)packet);
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
					CsClientPacket obj = (CsClientPacket)Rev[op].ClonePacket();
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
				log.DebugFormat("收到来自客户端的未知数据包, 操作码:0x{0}\r\n{1}", (object)Conversion.Hex(op).PadLeft(2, '0'), (object)Util.ToHex(data, 0));
			}
			return null;
		}
	}
}
