using Compositional.Composer;

namespace Appson.Common.Configuration
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