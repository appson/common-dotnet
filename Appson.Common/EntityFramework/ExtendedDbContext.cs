using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Appson.Common.General.Collections;
using Appson.Common.General.Model;

namespace Appson.Common.EntityFramework
{
	public abstract class ExtendedDbContext : DbContext
	{
		public override int SaveChanges()
		{
			ProcessEntries();
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync()
		{
			ProcessEntries();
			return base.SaveChangesAsync();
		}

		private void ProcessEntries()
		{
			ChangeTracker.Entries().ForEach(ProcessEntry);

			ChangeTracker.Entries<IIndexedEntity>()
				.Where(e => e.State == EntityState.Modified || e.State == EntityState.Added)
				.ForEach(e => DateTimex(e));
		}

		private static DateTime? DateTimex(DbEntityEntry<IIndexedEntity> e)
		{
			return e.Property(ie => ie.IndexedTime).CurrentValue = null;
		}

		private void ProcessEntry(DbEntityEntry entry)
		{
			var state = entry.State;

			if (state != EntityState.Added && state != EntityState.Modified)
				return;

			var entity = entry.Entity;
			var creationTime = entity as ICreationTime;
			var lastModificationTime = entity as ILastModificationTime;
			var indexedEntity = entity as IIndexedEntity;

			if (indexedEntity != null)
				indexedEntity.IndexedTime = null;

			if (creationTime != null && state == EntityState.Added)
				creationTime.CreationTime = DateTime.Now;

			if (lastModificationTime != null)
				lastModificationTime.LastModificationTime = DateTime.Now;
		}

	}
}