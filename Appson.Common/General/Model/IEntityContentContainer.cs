namespace Appson.Common.DomainModel
{
	public interface IEntityContentContainer<TContent> where TContent : class, IEntityContent
	{
		string ContentString { get; set; }
		TContent Content { get; set; }
	}
}