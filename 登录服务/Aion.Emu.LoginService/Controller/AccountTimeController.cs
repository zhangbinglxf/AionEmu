namespace Aion.Emu.LoginService
{
	public class AccountTimeController
	{
		internal static void UpDataLoginTime(Account account)
		{
			DAOManager.AccountTimeDAO.UpdataLoginTime(account);
		}
	}
}
