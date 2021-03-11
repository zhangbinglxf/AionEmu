using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Aion.Emu.LoginService
{
	public class LoginService
	{
		private Dictionary<int, Account> accounts;

		public int CurrentPlayers => accounts.Count;

		public LoginService()
		{
			accounts = new Dictionary<int, Account>();
		}

		internal bool HasAccount(int id)
		{
			return accounts.ContainsKey(id);
		}

		internal void AddAccount(Account acc)
		{
			lock (accounts)
			{
				if (!accounts.ContainsKey(acc.Id))
				{
					accounts.Add(acc.Id, acc);
				}
			}
		}

		internal Account RemoveGetAccount(int id)
		{
			Account value = null;
			if (accounts.TryGetValue(id, out value))
			{
				if (!Information.IsNothing(value.ActiveSlb))
				{
					value.ActiveSlb.Decrement();
				}
				accounts.Remove(id);
			}
			return value;
		}

		internal void Clear()
		{
			lock (accounts)
			{
				accounts.Clear();
			}
		}

		internal Account GetAccount(int id)
		{
			if (accounts.ContainsKey(id))
			{
				return accounts[id];
			}
			return null;
		}
	}
}
