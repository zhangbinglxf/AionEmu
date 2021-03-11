namespace Aion.Emu.LoginService
{
	public class ReConnectionAccount
	{
		private Account _account;

		private int _rekey;

		public Account Account => _account;

		public int ReKey => _rekey;

		public ReConnectionAccount(Account acc, int key)
		{
			_account = acc;
			_rekey = key;
		}
	}
}
