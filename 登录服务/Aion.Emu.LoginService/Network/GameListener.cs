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
	public class GameListener : ServerListener
	{
		private ILog log;

		private static GameListener instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private Dictionary<long, GameConnection> clients;

		public static GameListener GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new GameListener(IPAddress.Any, LoginConfig.GAME_PORT);
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

		private GameListener(IPAddress address, ushort port)
			: base(address, port)
		{
			log = LogManager.GetLogger(GetType());
			clients = new Dictionary<long, GameConnection>();
		}

		public void Start()
		{
			StartListener();
			log.InfoFormat("开放端口 {0} 等待游戏服务器连接!", (object)LoginConfig.GAME_PORT);
		}

		protected override void ReviceAcceptSocket(Socket socket)
		{
			AddNewGameServer(new GameConnection(socket));
		}

		private void AddNewGameServer(GameConnection game)
		{
			game.OnDisconnected += OnDisconnected;
			game.StartReceivePacket();
			clients.Add(game.ClientID, game);
			log.InfoFormat("收到来自 {0} 的游戏服务器连接!", (object)game.IP);
		}

		private void OnDisconnected(object sender, EventArgs e)
		{
			GameConnection gameConnection = (GameConnection)sender;
			clients.Remove(gameConnection.ClientID);
			log.WarnFormat("来自 {0} 的游戏服务器连接中断!", (object)gameConnection.IP);
		}
	}
}
