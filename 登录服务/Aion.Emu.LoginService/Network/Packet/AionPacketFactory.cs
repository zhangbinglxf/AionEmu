using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class AionPacketFactory : PacketHandleFactory
	{
		private ILog log;

		private Dictionary<int, LsClientPacket> Rev;

		private Dictionary<Type, int> Sed;

		private static AionPacketFactory instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		public static AionPacketFactory GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new AionPacketFactory();
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

		private AionPacketFactory()
		{
			log = LogManager.GetLogger(GetType());
			Rev = new Dictionary<int, LsClientPacket>();
			Sed = new Dictionary<Type, int>();
			AddClient(new CM_PLAY(2, State.AUTHED_LOGIN));
			AddClient(new CM_SERVER_LIST(5, State.AUTHED_LOGIN));
			AddClient(new CM_AUTH_GG(7, State.CONNECTED));
			AddClient(new CM_UPDATE_SESSION(8, State.CONNECTED));
			AddClient(new CM_LOGIN(11, State.AUTHED));
			AddServer(typeof(SM_INIT), 0);
			AddServer(typeof(SM_LOGIN_FAIL), 1);
			AddServer(typeof(SM_LOGIN_BAN), 2);
			AddServer(typeof(SM_LOGIN_OK), 3);
			AddServer(typeof(SM_SERVER_LIST), 4);
			AddServer(typeof(SM_PLAY_FAIL), 6);
			AddServer(typeof(SM_PLAY_OK), 7);
			AddServer(typeof(SM_AUTH_GG), 11);
			AddServer(typeof(SM_UPDATE_SESSION), 12);
		}

		protected override void AddClient(BasePacket packet)
		{
			if (!Rev.ContainsKey(packet.OpCode))
			{
				Rev.Add(packet.OpCode, (LsClientPacket)packet);
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
					LsClientPacket obj = (LsClientPacket)Rev[op].ClonePacket();
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
			if (LoginConfig.UNKNOWN_PACKET)
			{
				log.DebugFormat("收到来自客户端的未知数据包, 操作码:0x{0}\r\n{1}", (object)Conversion.Hex(op).PadLeft(2, '0'), (object)Util.ToHex(data, 0));
			}
			return null;
		}
	}
}
