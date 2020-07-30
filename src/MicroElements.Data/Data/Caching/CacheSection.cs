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
    /// Represents a wrapper over <see cref="IMemoryCache"/>.
    /// Details:
    /// - Cache keys are strings.
    /// - Synchronized call to factory method by cache key.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class CacheSection<TValue> : ICacheSection<TValue>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheSectionDescriptor<TValue> _cacheSectionDescriptor;
        private readonly Action<ICacheEntryContext>? _configureCacheEntry;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _keys = new ConcurrentDictionary<string, SemaphoreSlim>();

        /// <inheritdoc />
        public string SectionName => _cacheSectionDescriptor.SectionName;

        /// <inheritdoc />
        public ICacheSettings SettingsUntyped => _cacheSectionDescriptor.CacheSettings;

        /// <inheritdoc />
        public ICacheSettings<TValue> Settings => _cacheSectionDescriptor.CacheSettings;

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
            Action<ICacheEntryContext>? configureCacheEntry = null,
            ICacheSettings<TValue>? settings = null)
        {
            sectionName.AssertArgumentNotNull(nameof(sectionName));

            _cacheSectionDescriptor = new CacheSectionDescriptor<TValue>(sectionName, settings ?? CacheSettings<TValue>.Default);
            _memoryCache = memoryCache.AssertArgumentNotNull(nameof(memoryCache));
            _configureCacheEntry = configureCacheEntry;
        }

        /// <inheritdoc />
        public override string ToString() => $"Name: {SectionName}, Keys: {_keys.Count}";

        /// <inheritdoc />
        public async Task<CacheResult<TValue>> GetOrCreateAsync(string key, Func<ICacheEntryContext, Task<TValue>> factory)
        {
            using var keyLock = await _keys
                .GetOrAdd(key, k => new SemaphoreSlim(1))
                .WaitAsyncAndGetLockReleaser();

            CacheItem<TValue> cacheItem;
            CacheHitMiss cacheHitMiss = CacheHitMiss.Miss;
            bool isCached = false;

            if (_memoryCache.TryGetValue(key, out object result))
            {
                // from cache
                cacheItem = (CacheItem<TValue>)result;
                cacheHitMiss = CacheHitMiss.Hit;
                isCached = true;
            }
            else
            {
                // add new to cache
                var cacheEntry = _memoryCache.CreateEntry(key);

                var cacheEntryContext = new CacheEntryContext(_cacheSectionDescriptor, cacheEntry);
                var sw = Stopwatch.StartNew();

                try
                {
                    TValue value = default;
                    Message? message = null;
                    bool? shouldCache = null;

                    try
                    {
                        // Create value with factory
                        value = await factory(cacheEntryContext);

                        // Optionally can check whether value is valid and get error message
                        if (Settings.Validate != null)
                        {
                            var validationContext = new ValidationContext<TValue>(_cacheSectionDescriptor, value, cacheEntryContext.Metadata);
                            Settings.Validate(validationContext);

                            message = validationContext.Error;
                            shouldCache = validationContext.ShouldCache;
                        }
                    }
                    catch (Exception e)
                    {
                        if (Settings.HandleError != null)
                        {
                            var errorHandleContext = new ErrorHandleContext<TValue>(_cacheSectionDescriptor, e, cacheEntryContext.Metadata)
                            {
                                Error = CreateErrorMessage(),
                                Value = default,
                                ShouldCache = Settings.CacheErrorValue,
                            };
                            Settings.HandleError(errorHandleContext);

                            value = errorHandleContext.Value;
                            message = errorHandleContext.Error;
                            shouldCache = errorHandleContext.ShouldCache;

                            if (message != null)
                                message = EnrichMessage(message);
                        }
                        else if (Settings.HandleErrorUntyped != null)
                        {
                            var errorHandleContext = new ErrorHandleContext(_cacheSectionDescriptor, e, cacheEntryContext.Metadata)
                            {
                                Error = CreateErrorMessage(),
                                ShouldCache = Settings.CacheErrorValue,
                            };
                            Settings.HandleErrorUntyped(errorHandleContext);

                            value = default;
                            message = errorHandleContext.Error;
                            shouldCache = errorHandleContext.ShouldCache;

                            if (message != null)
                                message = EnrichMessage(message);
                        }
                        else
                        {
                            value = default;
                            message = CreateErrorMessage();
                        }

                        Message CreateErrorMessage() =>
                            new Message("Error while getting value for cache. Section: {SectionName}, Key: {Key}, ErrorMessage: {ErrorMessage}", MessageSeverity.Error)
                                .WithProperties(Properties(), PropertyAddMode.AddIfNotExists);

                        Message EnrichMessage(Message msg) => msg.WithProperties(Properties(), PropertyAddMode.AddIfNotExists);

                        KeyValuePair<string, object>[] Properties() =>
                            new[]
                            {
                                new KeyValuePair<string, object>("SectionName", SectionName),
                                new KeyValuePair<string, object>("Key", key),
                                new KeyValuePair<string, object>("Exception", e),
                                new KeyValuePair<string, object>("ErrorMessage", e.Message),
                            };
                    }

                    cacheItem = new CacheItem<TValue>(value, message, cacheEntryContext.Metadata);

                    var isSuccess = message == null || message.Severity != MessageSeverity.Error;
                    var isError = !isSuccess;
                    var shouldCacheFinal = shouldCache.GetValueOrDefault(isSuccess || (isError && Settings.CacheErrorValue));

                    if (shouldCacheFinal)
                    {
                        cacheEntry.SetValue(cacheItem);
                        _configureCacheEntry?.Invoke(cacheEntryContext);

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
                    cacheEntryContext.Metadata.SetValue(CacheResult.Elapsed, sw.Elapsed);
                }
            }

            CacheResult<TValue> cacheResult = new CacheResult<TValue>(
                _cacheSectionDescriptor,
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
            var cacheContext = new CacheEntryContext(_cacheSectionDescriptor, cacheEntry);

            var cacheItem = new CacheItem<TValue>(value, null, cacheContext.Metadata);
            cacheEntry.SetValue(cacheItem);
            _configureCacheEntry?.Invoke(cacheContext);

            cacheEntry.Dispose();
        }

        /// <inheritdoc/>
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            _keys.TryRemove(key, out _);
        }

        /// <inheritdoc />
        public Option<TValue> Get(string key)
        {
            if (_memoryCache.TryGetValue(key, out object cached))
            {
                CacheItem<TValue> cacheItem = (CacheItem<TValue>)cached;
                return cacheItem.Value;
            }

            _keys.TryRemove(key, out _);
            return Option<TValue>.None;
        }

        /// <inheritdoc />
        public CacheResult<TValue> GetCacheEntry(string key)
        {
            if (_memoryCache.TryGetValue(key, out object cached))
            {
                CacheItem<TValue> cacheItem = (CacheItem<TValue>)cached;
                return new CacheResult<TValue>(
                    cacheSection: _cacheSectionDescriptor,
                    key: key,
                    value: cacheItem.Value,
                    error: cacheItem.Error,
                    metadata: cacheItem.Metadata,
                    hitMiss: CacheHitMiss.Hit,
                    isCached: true);
            }

            _keys.TryRemove(key, out _);

            return CacheResult.Empty(_cacheSectionDescriptor, key);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (string key in Keys)
            {
                Remove(key);
            }
        }
    }
}
