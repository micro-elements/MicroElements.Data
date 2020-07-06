// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    /// <typeparam name="TValue">Value type.</typeparam>
    public class CacheSection<TValue> : ICacheSection<TValue>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheSettings<TValue> _settings;
        private readonly Action<ICacheContext>? _configureCacheEntry;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _keys = new ConcurrentDictionary<string, SemaphoreSlim>();

        /// <inheritdoc />
        public string SectionName { get; }

        /// <inheritdoc />
        public Type ValueType => typeof(TValue);

        /// <inheritdoc />
        public IReadOnlyCollection<string> Keys => _keys.Keys.ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheSection{TValue}"/> class.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <param name="memoryCache">Provided memory cache.</param>
        /// <param name="configureCacheEntry">External configure function.</param>
        /// <param name="settings">Cache settings.</param>
        public CacheSection(
            [NotNull] string sectionName,
            [NotNull] IMemoryCache memoryCache,
            Action<ICacheContext>? configureCacheEntry = null,
            ICacheSettings<TValue>? settings = null)
        {
            SectionName = sectionName.AssertArgumentNotNull(nameof(sectionName));
            _memoryCache = memoryCache.AssertArgumentNotNull(nameof(memoryCache));
            _configureCacheEntry = configureCacheEntry;
            _settings = settings ?? CacheSettings<TValue>.Default;
        }

        /// <inheritdoc />
        public ICacheSettings SettingsUntyped => _settings;

        /// <inheritdoc />
        public ICacheSettings<TValue> Settings => _settings;

        /// <inheritdoc />
        public override string ToString() => $"Name: {SectionName}, Keys: {_keys.Count}";

        /// <inheritdoc />
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
                Message? message;
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

            CacheResult<TValue> cacheResult = new CacheResult<TValue>(
                SectionName,
                Settings,
                key,
                cacheItem.Value,
                cacheItem.Error,
                cacheItem.Metadata,
                cacheHitMiss,
                isCached);

            return cacheResult;
        }

        /// <inheritdoc/>
        public void Set(string key, TValue value)
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

        /// <inheritdoc/>
        public void RemoveValue(string key)
        {
            _memoryCache.Remove(key);
            _keys.TryRemove(key, out _);
        }

        /// <summary>
        /// Gets the item associated with this key if present.
        /// </summary>
        /// <param name="key">An object identifying the requested entry.</param>
        /// <param name="value">The located value or null.</param>
        /// <returns>True if the key was found.</returns>
        public bool TryGetValue(string key, out TValue value)
        {
            bool hasValue = _memoryCache.TryGetValue(key, out object cached);
            CacheItem<TValue> cacheItem = hasValue ? (CacheItem<TValue>)cached : default;
            value = hasValue ? cacheItem.Value : default;

            if (!hasValue)
            {
                _keys.TryRemove(key, out _);
            }

            return hasValue;
        }

        /// <inheritdoc />
        public Option<TValue> Get(string key)
        {
            if (TryGetValue(key, out TValue value))
                return value;
            return Option<TValue>.None;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (string key in Keys)
            {
                RemoveValue(key);
            }
        }
    }
}
