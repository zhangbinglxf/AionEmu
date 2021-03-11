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
			AddClient(new CM_GS_AUTH(0, State.CONNECTED));
			AddClient(new CM_GS_ACCOUNT_AUTH(1, State.AUTHED));
			AddClient(new CM_GS_ACCOUNT_RECONNECT_KEY(2, State.AUTHED));
			AddClient(new CM_GS_ACCOUNT_DISCONNECTED(3, State.AUTHED));
			AddClient(new CM_GS_ACCOUNT_LIST(4, State.AUTHED));
			AddClient(new CM_GS_CONTROL(5, State.AUTHED));
			AddClient(new CM_GS_BAN(6, State.AUTHED));
			AddClient(new CM_GS_CHARACTER(8, State.AUTHED));
			AddClient(new CM_GS_ACCOUNT_TOLL_INFO(9, State.AUTHED));
			AddClient(new CM_GS_PREMIUM_CONTROL(11, State.AUTHED));
			AddClient(new CM_GS_LUNA_CONTROL(15, State.AUTHED));
			AddClient(new CM_GS_LOGINREWARD_CONTROL(16, State.AUTHED));
			AddClient(new CM_GS_LUNA_UPDATA_KEY(17, State.AUTHED));
			AddClient(new CM_GS_LOAD_PAYREWARD(18, State.AUTHED));
			AddClient(new CM_GS_PAY_SEND_REQUEST(19, State.AUTHED));
			AddClient(new CM_GS_TOTALCONSUM_REQUEST(20, State.AUTHED));
			AddServer(typeof(SM_GS_AUTH_RESPONSE), 0);
			AddServer(typeof(SM_GS_ACCOUNT_AUTH_RESPONSE), 1);
			AddServer(typeof(SM_GS_REQUEST_KICK_ACCOUNT), 2);
			AddServer(typeof(SM_GS_ACCOUNT_RECONNECT_KEY), 3);
			AddServer(typeof(SM_GS_CONTROL_RESPONSE), 4);
			AddServer(typeof(SM_GS_BAN_RESPONE), 5);
			AddServer(typeof(SM_GS_CHARACTER_RESPONSE), 8);
			AddServer(typeof(SM_GS_PREMIUM_RESPONSE), 10);
			AddServer(typeof(SM_GS_EXPIRATION), 15);
			AddServer(typeof(SM_GS_LUNA_RESPONSE), 16);
			AddServer(typeof(SM_GS_LUNA_UPDATA_KEY), 17);
			AddServer(typeof(SM_GS_PAY_REWARD_INFO), 18);
			AddServer(typeof(SM_GS_SEND_PAYREWARD), 19);
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
			if (LoginConfig.UNKNOWN_PACKET)
			{
				log.DebugFormat("收到来自游戏服务器的未知数据包, 操作码:0x{0}\r\n{1}", (object)Conversion.Hex(op).PadLeft(2, '0'), (object)Util.ToHex(data, 0));
			}
			return null;
		}
	}
}
