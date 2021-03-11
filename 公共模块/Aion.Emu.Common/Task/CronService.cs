using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Util;

namespace Aion.Emu.Common
{
	public class CronService
	{
		private ILog log;

		private static CronService instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private long Id;

		private IScheduler _scheduler;

		public static CronService GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new CronService();
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

		private CronService()
		{
			log = LogManager.GetLogger(GetType());
			Id = 0L;
		}

		public void Initialize()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			ISchedulerFactory val = (ISchedulerFactory)new StdSchedulerFactory();
			_scheduler = val.GetScheduler().Result;
			_scheduler.Start();
		}

		public void Shutdown()
		{
			_scheduler.Shutdown(false);
		}

		public void scheduler(Action act, string cron)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			try
			{
				JobDataMap val = new JobDataMap();
				((DirtyFlagMap<string, object>)(object)val).Add(RunnableJob.TASK_KEY, (object)act);
				string text = "job_" + Conversions.ToString(NextId());
				JobKey val2 = new JobKey(text);
				JobDetailImpl val3 = (JobDetailImpl)JobBuilder.Create(typeof(RunnableJob)).UsingJobData(val).WithIdentity(val2)
					.Build();
				CronScheduleBuilder val4 = CronScheduleBuilder.CronSchedule(cron);
				ICronTrigger val5 = (ICronTrigger)TriggerBuilder.Create().WithSchedule((IScheduleBuilder)(object)val4).Build();
				_scheduler.ScheduleJob((IJobDetail)(object)val3, (ITrigger)(object)val5);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
			}
		}

		private long NextId()
		{
			return Interlocked.Increment(ref Id);
		}

		public long GetRunTime(string cron, int runTime)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			checked
			{
				try
				{
					CronScheduleBuilder val = CronScheduleBuilder.CronSchedule(cron);
					ICronTrigger val2 = (ICronTrigger)TriggerBuilder.Create().WithSchedule((IScheduleBuilder)(object)val).Build();
					DateTimeOffset dateTimeOffset = DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(runTime)).ToLocalTime();
					IList<DateTimeOffset> list = (IList<DateTimeOffset>)TriggerUtils.ComputeFireTimesBetween((IOperableTrigger)val2, (ICalendar)null, dateTimeOffset, DateTimeOffset.Now.ToLocalTime());
					foreach (DateTimeOffset item in list)
					{
						long num = (long)Math.Round(item.AddMinutes(runTime).Subtract(DateTimeOffset.Now).TotalMilliseconds);
						if (num > 0 && num <= runTime * 60 * 1000)
						{
							return num;
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					log.Error((object)ex2);
					ProjectData.ClearProjectError();
				}
				return 0L;
			}
		}
	}
}
