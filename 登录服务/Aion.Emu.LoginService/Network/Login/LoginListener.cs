using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class LoginListener : ServerListener
	{
		private ILog log;

		private static LoginListener instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private Dictionary<int, AionConnection> clients;

		public static LoginListener GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new LoginListener(IPAddress.Any, LoginConfig.LOGIN_PORT);
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

		private LoginListener(IPAddress address, ushort port)
			: base(address, port)
		{
			log = LogManager.GetLogger(GetType());
			clients = new Dictionary<int, AionConnection>();
		}

		public void Start()
		{
			StartListener();
			log.InfoFormat("开放端口 {0} 等待客户端连接!", (object)LoginConfig.LOGIN_PORT);
		}

		protected override void ReviceAcceptSocket(Socket socket)
		{
			AddNewClient(new AionConnection(socket));
		}

		private void AddNewClient(AionConnection aion)
		{
			aion.OnDisconnected += OnDisconnected;
			aion.StartReceivePacket();
			clients.Add(checked((int)aion.ClientID), aion);
			log.InfoFormat("收到来自 {0} 的客户端连接!", (object)aion.IP);
		}

		private void OnDisconnected(object sender, EventArgs e)
		{
			AionConnection aionConnection = (AionConnection)sender;
			clients.Remove(checked((int)aionConnection.ClientID));
		}
	}
}
