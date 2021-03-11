using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Quartz;
using Quartz.Util;

namespace Aion.Emu.Common
{
	public class RunnableJob : IJob
	{
		public static string TASK_KEY = Guid.NewGuid().ToString();


        Task IJob.Execute(IJobExecutionContext context)
        {
			
			try
			{
				Action action = (Action)context.JobDetail.JobDataMap.Get(TASK_KEY);
				if (!Information.IsNothing(action))
				{
					action();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			return Task.CompletedTask;
		}
    }
}
