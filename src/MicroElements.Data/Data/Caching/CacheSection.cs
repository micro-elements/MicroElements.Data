// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroElements.Functional;
using Microsoft.Extensions.Caching.Memory;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Обертка над <see cref="IMemoryCache"/>.
    /// Особенности:
    /// - ключи представляют собой строки
    /// - используется блокировка по ключу при получении данных через фабрику
    /// </summary>
    public class CacheSection<TValue> : ICacheSection<TValue>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly bool _isExternalCache;
        private readonly ICacheSettings<TValue> _settings;
        private readonly Action<ICacheContext> _configureCacheEntry;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _keys = new ConcurrentDictionary<string, SemaphoreSlim>();

        public string Name { get; }

        public IReadOnlyCollection<string> Keys => _keys.Keys.ToList();

        public CacheSection(string name,
            IMemoryCache memoryCache = null,
            Action<ICacheContext> configureCacheEntry = null,
            ICacheSettings<TValue> settings = null)
        {
            Name = name;
            Info = new CacheSettings<>();
            _memoryCache = memoryCache;// ?? new MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
            _isExternalCache = memoryCache != null;
            _configureCacheEntry = configureCacheEntry;
            _settings = settings;
        }

        /// <inheritdoc />
        public ICacheSectionInfo Info { get; }

        public ICacheSettings SettingsUntyped => _settings;

        public ICacheSettings<TValue> Settings => _settings;

        public override string ToString() => $"Name: {Name}, Keys: {_keys.Count}";

        public async Task<CacheResult<TValue>> GetOrCreateAsync(string key, Func<ICacheContext, Task<TValue>> factory)
        {
            using var keyLock = await _keys
                .GetOrAdd(key, k => new SemaphoreSlim(1))
                .WaitAsyncAndGetLockReleaser();

            CacheItem<TValue> cacheItem;
            CacheHitMiss cacheHitMiss = CacheHitMiss.Miss;
            bool isCached = false;

            if (!_memoryCache.TryGetValue(key, out object result))
            {
                // add new to cache
                var cacheEntry = _memoryCache.CreateEntry(key);

                var cacheContext = new CacheContext(cacheEntry);
                Stopwatch sw = Stopwatch.StartNew();

                TValue value = default;
                Message message;
                bool isSuccess;
                bool shouldCache;

                try
                {
                    try
                    {
                        value = await factory(cacheContext);
                        message = _settings.Validate?.Invoke(value);
                        isSuccess = message == null || message.Severity != MessageSeverity.Error;
                    }
                    catch (Exception e)
                    {
                        message = _settings.HandleCreateError?.Invoke(e) ?? new Message($"Error in get cache value: {e.Message}", MessageSeverity.Error);
                        isSuccess = false;
                        shouldCache = _settings.CacheErrorValue;

                        if (!shouldCache)
                        {
                            throw;
                        }
                    }

                    if (isSuccess)
                    {
                        // Valid value
                        cacheItem = new CacheItem<TValue>(value, null, cacheContext);
                        shouldCache = true;
                    }
                    else
                    {
                        // Error value
                        cacheItem = new CacheItem<TValue>(value, message, cacheContext);
                        shouldCache = _settings.CacheErrorValue;
                    }

                    if (shouldCache)
                    {
                        cacheEntry.SetValue(cacheItem);
                        _configureCacheEntry?.Invoke(cacheContext);

                        // need to manually call dispose instead of having a using
                        // in case the factory passed in throws, in which case we
                        // do not want to add the entry to the cache
                        cacheEntry.Dispose();

                        // mark item as cached
                        isCached = true;
                    }
                }
                finally
                {
                    cacheContext.Metadata.SetValue(CacheManager.Elapsed, sw.Elapsed);
                }
            }
            else
            {
                // from cache
                cacheItem = (CacheItem<TValue>)result;
                cacheHitMiss = CacheHitMiss.Hit;
                isCached = true;
            }

            CacheResult<TValue> cacheResult = new CacheResult<TValue>(Settings, key, cacheItem.Value, cacheItem.Error, cacheItem.Metadata, cacheHitMiss, isCached);

            return cacheResult;
        }

        public void SetValue(string key, TValue value)
        {
            using var keyLock = _keys
                .GetOrAdd(key, k => new SemaphoreSlim(1))
                .WaitAndGetLockReleaser();

            var cacheEntry = _memoryCache.CreateEntry(key);
            var cacheContext = new CacheContext(cacheEntry);

            var cacheItem = new CacheItem<TValue>(value, null, cacheContext);
            cacheEntry.SetValue(cacheItem);
            _configureCacheEntry?.Invoke(cacheContext);

            cacheEntry.Dispose();
        }

        public void RemoveValue(string key)
        {
            _memoryCache.Remove(key);
            _keys.TryRemove(key, out _);
        }

        public bool TryGetValue(string key, out TValue result)
        {
            bool hasValue = _memoryCache.TryGetValue(key, out object cached);
            CacheItem<TValue> cacheItem = hasValue ? (CacheItem<TValue>)cached : default;
            result = hasValue ? cacheItem.Value : default;

            if (!hasValue)
            {
                _keys.TryRemove(key, out _);
            }

            return hasValue;
        }

        public Option<TValue> Get(string key)
        {
            if (TryGetValue(key, out TValue value))
                return value;
            return Option<TValue>.None;
        }

        public void Clear()
        {
            foreach (string key in Keys)
            {
                RemoveValue(key);
            }
        }
    }
}
