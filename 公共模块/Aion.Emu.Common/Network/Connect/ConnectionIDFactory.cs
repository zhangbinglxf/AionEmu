using System.Threading;

namespace Aion.Emu.Common
{
	public class ConnectionIDFactory
	{
		private static long _id;

		public static long NextID()
		{
			return Interlocked.Increment(ref _id);
		}
	}
}
