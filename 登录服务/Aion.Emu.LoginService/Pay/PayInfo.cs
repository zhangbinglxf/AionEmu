namespace Aion.Emu.LoginService
{
	public class PayInfo
	{
		private string _account;

		private int _pay_num;

		public string AccountName => _account;

		public int PayNum => _pay_num;

		public PayInfo(string account, int num)
		{
			_account = account;
			_pay_num = num;
		}
	}
}
