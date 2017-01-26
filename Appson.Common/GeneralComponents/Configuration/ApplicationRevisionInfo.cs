
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Configuration
{
    [Contract]
	[Component]
	[IgnoredOnAssemblyRegistration]
	public class ApplicationRevisionInfo
	{
		[ConfigurationPoint]
		public string VersionString { get; set; }

		[ConfigurationPoint(false)]
		public string ApiVersionString { get; set; }
	}
}