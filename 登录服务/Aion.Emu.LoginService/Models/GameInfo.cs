using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Aion.Emu.Common;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
	public class GameInfo
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<Slb, string> _0024I37_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			internal string _Lambda_0024__37_002D0(Slb s)
			{
				return s.SlbIP;
			}
		}

		private int _id;

		private string _ip;

		private string _password;

		private short _port;

		private int _maxPlayers;

		private GameConnection _gameConnect;

		private byte[] _address;


		private List<Slb> slbs;

		public int ServerId => _id;

		public string ServerIp => _ip;

		public string Password => _password;

		public short Port
		{
			get
			{
				return _port;
			}
			set
			{
				_port = value;
			}
		}

		public int MaxPlayers
		{
			get
			{
				return _maxPlayers;
			}
			set
			{
				_maxPlayers = value;
			}
		}

		public GameConnection GameConnection
		{
			get
			{
				return _gameConnect;
			}
			set
			{
				_gameConnect = value;
			}
		}

		public byte[] GameAddress
		{
			get
			{
				return _address;
			}
			set
			{
				_address = value;
			}
		}

		public GameInfo(int id, string ip, string password)
		{
			slbs = new List<Slb>();
			_id = id;
			_ip = ip;
			_password = password;
			ProcessItem();
			CronService.GetInstance().scheduler(ProcessItem, "0 0/2 * ? * *");
		}

		public byte[] SlbAddress(Account acc)
		{
			slbs.Sort();
			foreach (Slb slb in slbs)
			{
				if (slb.IsActive)
				{
					slb.Increment();
					acc.ActiveSlb = slb;
					return slb.Address;
				}
			}
			return _address;
		}

		public bool IsOnGameServer(int id)
		{
			return _gameConnect.Login.HasAccount(id);
		}

		public void AddAccount(Account acc)
		{
			_gameConnect.Login.AddAccount(acc);
		}

		public Account RemoveAccount(int id)
		{
			return _gameConnect.Login.RemoveGetAccount(id);
		}

		public int CurrentPlayers()
		{
			return _gameConnect.Login.CurrentPlayers;
		}

		public bool IsOnline()
		{
			if (!Information.IsNothing(_gameConnect))
			{
				return _gameConnect.State == State.AUTHED;
			}
			return false;
		}

		public bool IsFull()
		{
			return CurrentPlayers() >= MaxPlayers;
		}

		public void ClearGameAccount()
		{
			_gameConnect.Login.Clear();
		}

		public Account GetAccount(int id)
		{
			return _gameConnect.Login.GetAccount(id);
		}

		private void ProcessItem()
		{
			try
			{
				Dictionary<string, Slb> dictionary = slbs.ToDictionary((_Closure_0024__._0024I37_002D0 != null) ? _Closure_0024__._0024I37_002D0 : (_Closure_0024__._0024I37_002D0 = (Slb s) => s.SlbIP));
				slbs.Clear();
				if (LoginConfig.ALIYUN_SLB_ENDPOINT.Equals("UNKNOWN"))
				{
					return;
				}
				WebRequest webRequest = WebRequest.Create(LoginConfig.ALIYUN_SLB_ENDPOINT);
				webRequest.GetResponse();
				Stream responseStream = webRequest.GetResponse().GetResponseStream();
				string[] array = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd().Split(';');
				foreach (string text in array)
				{
					Slb value = null;
					if (dictionary.TryGetValue(text, out value))
					{
						slbs.Add(value);
					}
					else
					{
						slbs.Add(new Slb(text.Trim()));
					}
				}
				dictionary.Clear();
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
