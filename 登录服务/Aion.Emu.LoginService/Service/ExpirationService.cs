using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class ExpirationService : AbstractLockManager
	{
		private ILog log;

		private static ExpirationService instance;

		private static object _lock = RuntimeHelpers.GetObjectValue(new object());

		private static Timer _task;

		private static Dictionary<int, Account> expirables;


		public static ExpirationService GetInstance()
		{
			object @lock = _lock;
			ObjectFlowControl.CheckForSyncLockOnValueType(@lock);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(@lock, ref lockTaken);
				if (Information.IsNothing(instance))
				{
					instance = new ExpirationService();
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

		private ExpirationService()
		{
			log = LogManager.GetLogger(GetType());
		}

		public void initiate()
		{
			expirables = new Dictionary<int, Account>();
			_task = new Timer(Runing, null, 1000, 1000);
		}

		public void Add(Account acc)
		{
			WriteLock();
			try
			{
				expirables.Add(acc.Id, acc);
			}
			finally
			{
				UnWriteLock();
			}
		}

		public void Remove(int id)
		{
			WriteLock();
			try
			{
				Account account = expirables[id];
				expirables.Remove(id);
				if (Information.IsNothing(account))
				{
					DAOManager.AccountTimeDAO.UpdataAccountTime(account);
				}
			}
			finally
			{
				UnWriteLock();
			}
		}

		private void Runing(object state)
		{
			WriteLock();
			try
			{
				_ = DateAndTime.Now;
				int count = expirables.Count;
				for (int i = 0; i <= count; i = checked(i + 1))
				{
					expirables.ElementAt(i).Value.AccountTime.RunExpiration();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				log.Error((object)ex2);
				ProjectData.ClearProjectError();
			}
			finally
			{
				UnWriteLock();
			}
		}
	}
}
