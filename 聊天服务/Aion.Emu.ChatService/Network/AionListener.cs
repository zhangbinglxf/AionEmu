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

namespace Aion.Emu.ChatService
{
	public class AionListener : ServerListener
	{
		private ILog log;

		private static AionListener instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private Dictionary<long, AionConnection> clients;

		public static AionListener GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new AionListener(IPAddress.Any, ChatConfig.CHAT_PORT);
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

		private AionListener(IPAddress address, ushort port)
			: base(address, port)
		{
			log = LogManager.GetLogger(GetType());
			clients = new Dictionary<long, AionConnection>();
		}

		public void Start()
		{
			StartListener();
			log.InfoFormat("开放端口 {0} 等待客户端连接!", (object)ChatConfig.CHAT_PORT);
		}

		private void AddNewClient(AionConnection client)
		{
			client.OnDisconnected += OnDisconnected;
			clients.Add(client.ClientID, client);
			client.StartReceivePacket();
			log.InfoFormat("收到来自 {0} 的客户端连接!", (object)client.IP);
		}

		private void OnDisconnected(object sender, EventArgs e)
		{
			AionConnection aionConnection = (AionConnection)sender;
			clients.Remove(aionConnection.ClientID);
			log.InfoFormat("来自 {0} 的客户端连接中断!", (object)aionConnection.IP);
		}

		protected override void ReviceAcceptSocket(Socket socket)
		{
			AddNewClient(new AionConnection(socket));
		}
	}
}
