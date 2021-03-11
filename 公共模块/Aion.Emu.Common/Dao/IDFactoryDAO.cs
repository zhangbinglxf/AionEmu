using System.Collections.Generic;

namespace Aion.Emu.Common
{
	public abstract class IDFactoryDAO : DAO
	{
		public abstract List<int> GetUsedIds();
	}
}
