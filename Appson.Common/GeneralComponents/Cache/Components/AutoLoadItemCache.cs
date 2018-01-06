﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Appson.Composer;
using Appson.Composer.Cache;

namespace Appson.Common.GeneralComponents.Cache.Components
{
    [Component]
	[ComponentCache(typeof(DefaultComponentCache))]
	[IgnoredOnAssemblyRegistration]
	public class AutoLoadItemCache<TKey, TValue> : IItemCache<TKey, TValue>
	{
		private readonly ConcurrentDictionary<TKey, CacheItem<TValue>> _cacheData;
		private long _lastMaintenance;

		#region Plugs and Configuration points

		[ComponentPlug]
		public ICacheItemLoader<TKey, TValue> ItemLoader { get; set; }

		[ComponentPlug]
		public ICacheValueCopier<TValue> Copier { get; set; } 

		private int? _minimumSize;
		// private int? _maximumSize; TODO
		private int? _maintenanceFrequencySeconds;
		private int? _minimumLifetimeSeconds;
		private int? _maximumLifetimeSeconds;
		private int? _idleSecondsToRemove;

	    private const long TicksPerSecond = 10000000;

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

		public AutoLoadItemCache()
		{
			_cacheData = new ConcurrentDictionary<TKey, CacheItem<TValue>>();
			_lastMaintenance = DateTime.UtcNow.Ticks;
		}

		#endregion

		#region Implementation of ICache<in TKey,out TValue>

		public void InvalidateAll()
		{
			_cacheData.Clear();
		}

		public TValue this[TKey key]
		{
			get
			{
				var creationTimeLimit = DateTime.UtcNow.Ticks - MaximumLifetimeSeconds*TicksPerSecond;
				
				var item = _cacheData.GetOrAdd(key, k => new CacheItem<TValue>(ItemLoader.Load(k)));
				if (item.LastAccessTime < creationTimeLimit)
				{
					CacheItem<TValue> removedValue;
					_cacheData.TryRemove(key, out removedValue);
					
					item = _cacheData.GetOrAdd(key, k => new CacheItem<TValue>(ItemLoader.Load(k)));
				}

				var result = item.Value;

				CheckForMaintenance();
				return Copier.Copy(result);
			}
		}

		#endregion

	    #region Implementation of IItemCache<in TKey,out TValue>

	    public void InvalidateItem(TKey key)
	    {
	        CacheItem<TValue> item;

            if (_cacheData.ContainsKey(key))
                _cacheData.TryRemove(key, out item);
	    }

	    public void InvalidateItems(IEnumerable<TKey> keys)
	    {
	        foreach (var key in keys)
	        {
	            InvalidateItem(key);
	        }
	    }

	    #endregion

	    private void CheckForMaintenance()
		{
			if ((MaintenanceFrequencySeconds <= 0) || 
				(_cacheData.Count < MinimumSize) ||
				(_lastMaintenance + MaintenanceFrequencySeconds*TicksPerSecond > DateTime.UtcNow.Ticks))
				return;

			PerformMaintenance();
		}

		private void PerformMaintenance()
		{
			_lastMaintenance = DateTime.UtcNow.Ticks;

			var creationTimeLimit = DateTime.UtcNow.Ticks - MinimumLifetimeSeconds*TicksPerSecond;
			var lastAccessLimit = DateTime.UtcNow.Ticks - IdleSecondsToRemove*TicksPerSecond;

			var keysToRemove = _cacheData.Where(item => item.Value.CreationTime < creationTimeLimit && item.Value.LastAccessTime < lastAccessLimit).Select(item => item.Key).ToList();

			CacheItem<TValue> removedValue;
			foreach(var key in keysToRemove)
				_cacheData.TryRemove(key, out removedValue);
		}

		private class CacheItem<T>
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