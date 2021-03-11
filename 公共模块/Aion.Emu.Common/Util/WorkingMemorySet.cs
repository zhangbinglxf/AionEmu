using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.Common
{
	public class WorkingMemorySet
	{
		private ILog log;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static WorkingMemorySet instance;

		private string CRON_STRING;

		public static WorkingMemorySet GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new WorkingMemorySet();
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

		private WorkingMemorySet()
		{
			log = LogManager.GetLogger(typeof(WorkingMemorySet));
			CRON_STRING = "0 0 0/8 ? * *";
		}

		public void Initialize(string cron)
		{
			CRON_STRING = cron;
			CronService.GetInstance().scheduler(delegate
			{
				ClearMemory();
			}, CRON_STRING);
			ClearMemory();
		}

		private void NextInfo()
		{
			long runTime = CronService.GetInstance().GetRunTime(CRON_STRING, 1440);
			log.InfoFormat("下次内存整理任务将在 {0} 执行!", (object)DateAndTime.Now.AddMilliseconds(runTime).ToString("yyyy/MM/dd HH:mm:ss"));
		}

		public void ClearMemory()
		{
			try
			{
				Process currentProcess = Process.GetCurrentProcess();
				GC.Collect();
				long privateMemorySize = currentProcess.PrivateMemorySize64;
				log.Info((object)("内存整理完成! 占用内存: " + Conversions.ToString(Math.Round((double)privateMemorySize / 10240.0 / 1024.0, 2)) + "Mb"));
				NextInfo();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				log.Warn((object)"内存整理失败!");
				ProjectData.ClearProjectError();
			}
		}

		[CompilerGenerated]
		private void _Lambda_0024__8_002D0()
		{
			ClearMemory();
		}
	}
}
