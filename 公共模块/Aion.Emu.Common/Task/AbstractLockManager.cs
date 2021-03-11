using System.Threading;

namespace Aion.Emu.Common
{
	public abstract class AbstractLockManager
	{
		private static ReaderWriterLock _lock = new ReaderWriterLock();

		public void WriteLock()
		{
			_lock.AcquireWriterLock(-1);
		}

		public void UnWriteLock()
		{
			_lock.ReleaseWriterLock();
		}

		public void ReadLock()
		{
			_lock.AcquireReaderLock(-1);
		}

		public void UnReadLock()
		{
			_lock.ReleaseReaderLock();
		}
	}
}
