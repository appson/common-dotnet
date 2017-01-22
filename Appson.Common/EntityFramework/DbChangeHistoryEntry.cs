using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Appson.Common.Model;

namespace Appson.Common.EntityFramework
{
    public class DbChangeHistoryEntry
    {
        private DbEntityEntry _entry;
        private object _entity;
        private EntityState _state;
        private Dictionary<string, DbChangeHistoryEntryProperty> _properties;

        private DbChangeHistoryEntry()
        {
        }

        public void PostProcessAfterSaveChanges()
        {
            if (_properties == null || _entry == null)
                throw new InvalidOperationException("The object is not properly initialized.");

            ReapplyCurrentValues();
            RemoveUnchangedProperties();
        }

        public static DbChangeHistoryEntry FromDbEntityEntry(DbEntityEntry entry)
        {
            if (entry.State != EntityState.Added &&
                entry.State != EntityState.Modified &&
                entry.State != EntityState.Deleted)
                return null;

            if (!(entry.Entity is IChangeHistoryEntity))
                return null;

            var result = new DbChangeHistoryEntry
            {
                _entry = entry,
                _entity = entry.Entity,
                _state = entry.State,
                _properties = new Dictionary<string, DbChangeHistoryEntryProperty>()
            };

            var propertyNames = entry.State == EntityState.Deleted
                ? entry.OriginalValues.PropertyNames
                : entry.CurrentValues.PropertyNames;

            foreach (string propName in propertyNames)
            {
                var entryProperty = entry.Property(propName);
                var historyProperty = new DbChangeHistoryEntryProperty {Name = propName};

                if (entry.State != EntityState.Added)
                    historyProperty.OldValue = entryProperty.OriginalValue;

                if (entry.State != EntityState.Deleted)
                    historyProperty.NewValue = entryProperty.CurrentValue;

                result._properties.Add(propName, historyProperty);
            }

            return result;
        }

        public bool HasAnyChanges
        {
            get { return _properties != null && _properties.Count > 0; }
        }

        public object Entity
        {
            get { return _entity; }
        }

        public EntityState State
        {
            get { return _state; }
        }

        public List<DbChangeHistoryEntryProperty> Properties
        {
            get { return _properties.Values.ToList(); }
        }

        private void ReapplyCurrentValues()
        {
            // Should not re-apply deleted entities, as they don't have any current values
            if (_state == EntityState.Deleted)
                return;

            foreach (var propName in _properties.Keys)
            {
                var entryProperty = _entry.Property(propName);
                _properties[propName].NewValue = entryProperty.CurrentValue;
            }
        }

        private void RemoveUnchangedProperties()
        {
            // Processing all states, either deleted, added or modified.
            //   - For deleted entities, new values are always null; so if the original values were null, they will be removed.
            //   - For added entities, old values are always null; so if the new values are also null, they will be removed.
            //   - For modified entities, the old and new values will be compared.

            foreach (var propName in _properties.Keys.ToArray())
            {
                var property = _properties[propName];
                bool remove;

                if (property.OldValue == null)
                {
                    remove = property.NewValue == null;
                }
                else
                {
                    remove = property.OldValue.Equals(property.NewValue);
                }

                if (remove)
                    _properties.Remove(propName);
            }
        }
    }
}