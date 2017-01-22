using Compositional.Composer;

namespace Appson.Common.ScheduledTasks
{
	[Contract]
	public interface IScheduledTaskManager
	{
		void IterateTask(string key, bool isManuallyTriggered = false);
		void ChangeTaskStartup(string key, RecurringTaskStartupMode startupMode);
	}
}