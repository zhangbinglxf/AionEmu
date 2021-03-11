using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Aion.Emu.Common;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
	public class SlbService
	{
		//private static bool IsUseSLB = false;

		private static List<Slb> slbs = new List<Slb>();

		private static SlbService instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		public static byte[] Address
		{
			get
			{
				slbs.Sort();
				foreach (Slb slb in slbs)
				{
					if (slb.IsActive)
					{
						return slb.Address;
					}
				}
				return IPAddress.Parse(ChatConfig.CHAT_IP).GetAddressBytes();
			}
		}

		private SlbService()
		{
		}

		public static object GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new SlbService();
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

		public void Start()
		{
			ProcessItem();
			CronService.GetInstance().scheduler(ProcessItem, "0 0/2 * ? * *");
		}

		private void ProcessItem()
		{
			slbs.Clear();
			try
			{
				if (ChatConfig.ALIYUN_SLB_ENDPOINT.Equals("UNKNOWN"))
				{
					slbs.Add(new Slb(ChatConfig.CHAT_IP));
					return;
				}
				WebRequest webRequest = WebRequest.Create(ChatConfig.ALIYUN_SLB_ENDPOINT);
				webRequest.GetResponse();
				Stream responseStream = webRequest.GetResponse().GetResponseStream();
				string[] array = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd().Split(';');
				foreach (string text in array)
				{
					slbs.Add(new Slb(text.Trim()));
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}
	}
}
