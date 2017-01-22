using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using JahanJooy.Common.Util.Collections;
using JahanJooy.Common.Util.DomainModel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JahanJooy.Common.Util.EF
{
    public abstract class ExtendedIdentityDbContext<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> 
        : IdentityDbContext<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
    {
        private readonly DbChangeHistoryGenerator _changeHistoryGenerator = new DbChangeHistoryGenerator();

        protected ExtendedIdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public override int SaveChanges()
        {
            try
            {
                if (!_changeHistoryGenerator.IsClean)
                    throw new InvalidOperationException("Re-entry to SaveChanges method detected - ChangeHistoryGenerator is not clean. This method should not be called recursively.");

                ProcessEntries();
                var result = base.SaveChanges();

                if (!_changeHistoryGenerator.IsClean)
                {
                    _changeHistoryGenerator.PostProcessAfterSaveChanges();
                }

                if (!_changeHistoryGenerator.IsClean)
                {
                    OnChangeHistoryGenerated(_changeHistoryGenerator.Entries);
                }

                return result;
            }
            finally
            {
                _changeHistoryGenerator.CleanUp();
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                if (!_changeHistoryGenerator.IsClean)
                    throw new InvalidOperationException("Re-entry to SaveChanges method detected - ChangeHistoryGenerator is not clean. This method should not be called recursively.");

                ProcessEntries();
                var result = await base.SaveChangesAsync();

                if (!_changeHistoryGenerator.IsClean)
                {
                    _changeHistoryGenerator.PostProcessAfterSaveChanges();
                }

                if (!_changeHistoryGenerator.IsClean)
                {
                    await OnChangeHistoryGeneratedAsync(_changeHistoryGenerator.Entries);
                }

                return result;
            }
            finally
            {
                _changeHistoryGenerator.CleanUp();
            }
        }

        public int SaveChangesWithoutProcessing()
        {
            return base.SaveChanges();
        }

        public Task<int> SaveChangesWithoutProcessingAsync()
        {
            return base.SaveChangesAsync();
        }

        private void ProcessEntries()
        {
            ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ForEach(ProcessEntry);
        }

        protected virtual void ProcessEntry(DbEntityEntry entry)
        {
            var state = entry.State;
            var entity = entry.Entity;

            if (state != EntityState.Deleted)
            {
                var creationTime = entity as ICreationTime;
                var lastModificationTime = entity as ILastModificationTime;
                var stateTime = entity as IStateTime;
                var indexedEntity = entity as IIndexedEntity;

                if (creationTime != null && state == EntityState.Added)
                    creationTime.CreationTime = DateTime.Now;

                if (lastModificationTime != null)
                    lastModificationTime.LastModificationTime = DateTime.Now;

                if (stateTime != null)
                {
                    var stateProperty = entry.Property("State");
                    if (stateProperty != null && (stateProperty.IsModified || state == EntityState.Added))
                    {
                        stateTime.StateTime = DateTime.Now;
                    }
                }

                if (indexedEntity != null)
                    indexedEntity.IndexedTime = null;
            }

            _changeHistoryGenerator.ProcessEntry(entry);
        }

        protected virtual void OnChangeHistoryGenerated(IEnumerable<DbChangeHistoryEntry> entries)
        {
            // Do nothing.
            // Inheritors can provide implementation, if desired
        }

        protected virtual Task OnChangeHistoryGeneratedAsync(IEnumerable<DbChangeHistoryEntry> entries)
        {
            // Do nothing.
            // Inheritors can provide implementation, if desired

            return Task.FromResult(0);
        }
    }
}