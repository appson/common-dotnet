using Compositional.Composer;

namespace Appson.Common.ScheduledTasks
{
	[Contract]
	public interface IScheduledTaskStore
	{
		void EnsureTaskDefined(string key);

		RecurringTaskStartupMode GetStartupType(string key);
		void SetStartupType(string key, RecurringTaskStartupMode startupMode);

		string GetProgress(string key);
		void SetProgress(string key, ScheduledTaskIterationResult iterationResult);
	}
}