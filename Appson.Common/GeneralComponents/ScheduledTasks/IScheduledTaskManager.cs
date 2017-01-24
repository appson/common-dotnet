
using Appson.Composer;

namespace Appson.Common.GeneralComponents.ScheduledTasks
{
    [Contract]
	public interface IScheduledTaskManager
	{
		void IterateTask(string key, bool isManuallyTriggered = false);
		void ChangeTaskStartup(string key, RecurringTaskStartupMode startupMode);
	}
}