using log4net.Core;
using log4net.Filter;

namespace Aion.Emu.Common
{
	public class ConsoleFilter : FilterSkeleton
	{
		public ConsoleFilter()
			: base()
		{
		}

		public override FilterDecision Decide(LoggingEvent loggingEvent)
		{
			if (loggingEvent.LoggerName.StartsWith("["))
			{
				return (FilterDecision)(-1);
			}
			return (FilterDecision)1;
		}
	}
}
