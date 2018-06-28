using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Appson.Composer;

namespace Appson.Common.GeneralComponents.Cache.Components
{
    public abstract class AutoLoadItemCacheBase<TKey, TValue>
	{
		protected long _lastMaintenance;
        
		#region Plugs and Configuration points

		[ComponentPlug]
		public ICacheValueCopier<TValue> Copier { get; set; } 

		private int? _minimumSize;
		// private int? _maximumSize; TODO
		private int? _maintenanceFrequencySeconds;
		private int? _minimumLifetimeSeconds;
		private int? _maximumLifetimeSeconds;
		private int? _idleSecondsToRemove;

        protected virtual int DefaultMinimumSize => 50;
	    // protected virtual int DefaultMaximumSize { get { return 0; } } TODO
		protected virtual int DefaultMaintenanceFrequencySeconds => 600;
	    protected virtual int DefaultMinimumLifetimeSeconds => 600;
	    protected virtual int DefaultMaximumLifetimeSeconds => 3600;
	    protected virtual int DefaultIdleSecondsToRemove => 30000;

	    [ConfigurationPoint(false)]
		public int MinimumSize
		{
			get { return _minimumSize ?? DefaultMinimumSize; }
			set { _minimumSize = value; }
		}

//		[ConfigurationPoint(false)]
//		public int MaximumSize TODO
//		{
//			get { return _maximumSize ?? DefaultMaximumSize; }
//			set { _maximumSize = value; }
//		}

		[ConfigurationPoint(false)]
		public int MaintenanceFrequencySeconds
		{
			get { return _maintenanceFrequencySeconds ?? DefaultMaintenanceFrequencySeconds; }
			set { _maintenanceFrequencySeconds = value; }
		}


		[ConfigurationPoint(false)]
		public int MinimumLifetimeSeconds
		{
			get { return _minimumLifetimeSeconds ?? DefaultMinimumLifetimeSeconds; }
			set { _minimumLifetimeSeconds = value; }
		}

		[ConfigurationPoint(false)]
		public int MaximumLifetimeSeconds
		{
			get { return _maximumLifetimeSeconds ?? DefaultMaximumLifetimeSeconds; }
			set { _maximumLifetimeSeconds = value; }
		}


		[ConfigurationPoint(false)]
		public int IdleSecondsToRemove
		{
			get { return _idleSecondsToRemove ?? DefaultIdleSecondsToRemove; }
			set { _idleSecondsToRemove = value; }
		}

		#endregion

		#region Initialization

	    protected AutoLoadItemCacheBase()
		{
			_lastMaintenance = DateTime.UtcNow.Ticks;
		}

		#endregion

		protected class CacheItem<T>
		{
			private readonly T _value;
		    private long _lastAccessTime;

			public CacheItem(T value)
			{
				_value = value;
				CreationTime = _lastAccessTime = DateTime.UtcNow.Ticks;
			}

			public T Value
			{
				get 
				{
					_lastAccessTime = DateTime.UtcNow.Ticks; 
					return _value; 
				}
			}

			public long CreationTime { get; }

		    public long LastAccessTime => _lastAccessTime;
		}
	}
}