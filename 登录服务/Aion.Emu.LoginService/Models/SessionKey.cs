using System;

namespace Aion.Emu.LoginService
{
	public class SessionKey
	{
		private Random Rnd;

		private int _accountId;

		private int _loginOk;

		private int _playerOk1;

		private int _playerOk2;

		public int AccountId => _accountId;

		public int LoginOk => _loginOk;

		public int PlayerOk1 => _playerOk1;

		public int PlayerOk2 => _playerOk2;

		public SessionKey(Account acc)
		{
			Rnd = new Random();
			_accountId = acc.Id;
			_loginOk = Rnd.Next();
			_playerOk1 = Rnd.Next();
			_playerOk2 = Rnd.Next();
		}

		public SessionKey(int accountId, int loginOk, int playerOk1, int playerOk2)
		{
			Rnd = new Random();
			_accountId = accountId;
			_loginOk = loginOk;
			_playerOk1 = playerOk1;
			_playerOk2 = playerOk2;
		}

		public bool CheckLogin(int accountId, int loginOk)
		{
			if (_accountId == accountId)
			{
				return _loginOk == loginOk;
			}
			return false;
		}

		public object CheckSessionKey(SessionKey key)
		{
			return _playerOk1 == key.PlayerOk1 && _playerOk2 == key.PlayerOk2 && _loginOk == key.LoginOk && _accountId == key.AccountId;
		}
	}
}
