namespace Appson.Common.EntityFramework
{
	public class EntityChange
	{
		public string PropertyPath { get; set; }
		public object OriginalValue { get; set; }
		public object ChangedValue { get; set; }
	}
}