using System;

namespace Appson.Common.DomainModel
{
	public interface IIndexedEntity
	{
		DateTime? IndexedTime { get; set; }
	}
}