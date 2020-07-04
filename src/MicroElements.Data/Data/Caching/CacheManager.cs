// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroElements.Metadata;
using Microsoft.Extensions.Caching.Memory;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Wrapper over <see cref="IMemoryCache"/> that holds named, types cache sections <see cref="ICacheSection{TValue}"/>.
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// Time elapsed on getting value.
        /// </summary>
        public static readonly IProperty<TimeSpan> Elapsed = new Property<TimeSpan>("Elapsed").WithDescription("Time elapsed on getting value.");

        /// <summary>
        /// The source of value.
        /// </summary>
        public static readonly IProperty<string> Source = new Property<string>("Source").WithDescription("The source of value.");

        private readonly IMemoryCache _memoryCache;
        private readonly Action<ICacheContext> _configureCacheEntry;
        private readonly ConcurrentDictionary<string, ICacheSection> _sections = new ConcurrentDictionary<string, ICacheSection>();

        public CacheManager(IMemoryCache memoryCache = null, Action<ICacheContext> configureCacheEntry = null)
        {
            _memoryCache = memoryCache;
            _configureCacheEntry = configureCacheEntry;
        }

        public IReadOnlyCollection<ICacheSection> Sections => _sections.Values.ToList();

        public async Task<CacheResult<TValue>> GetOrCreateAsync<TValue>(string sectionName, string key, Func<ICacheContext, Task<TValue>> factory)
        {
            var cacheSection = GetSection(new CacheSettings<TValue>() { SectionName = sectionName });
            return await cacheSection.GetOrCreateAsync(key, factory);
        }

        public async Task<CacheResult<TValue>> GetOrCreateAsync<TValue>(ICacheSettings<TValue> cacheSettings, string key, Func<ICacheContext, Task<TValue>> factory)
        {
            var cacheSection = GetSection(cacheSettings);
            return await cacheSection.GetOrCreateAsync(key, factory);
        }

        public ICacheSection? GetSection(string sectionName)
        {
            _sections.TryGetValue(sectionName, out var cacheSection);
            return cacheSection;
        }

        public ICacheSection<TValue> GetSection<TValue>(ICacheSettings<TValue> cacheSettings)
        {
            CacheSection<TValue> CreateCacheSection(string name) => new CacheSection<TValue>(name, _memoryCache, _configureCacheEntry, cacheSettings);
            ICacheSection cacheSection = _sections.GetOrAdd(cacheSettings.SectionName, CreateCacheSection);
            return (ICacheSection<TValue>)cacheSection;
        }

        public void Clear()
        {
            foreach (var cacheSection in _sections)
            {
                cacheSection.Value.Clear();
            }
        }

        public void Freeze()
        {

        }
    }
}
