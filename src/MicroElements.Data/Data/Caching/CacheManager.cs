// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MicroElements.Functional;
using Microsoft.Extensions.Caching.Memory;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Wrapper over <see cref="IMemoryCache"/> that holds named, types cache sections <see cref="ICacheSection{TValue}"/>.
    /// </summary>
    public class CacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Action<ICacheEntryContext>? _configureCacheEntry;
        private readonly ConcurrentDictionary<string, ICacheSection> _sections = new ConcurrentDictionary<string, ICacheSection>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="memoryCache">Provided memory cache.</param>
        /// <param name="configureCacheEntry">Optional cache configure method for all sections.</param>
        public CacheManager([NotNull] IMemoryCache memoryCache, Action<ICacheEntryContext>? configureCacheEntry = null)
        {
            _memoryCache = memoryCache.AssertArgumentNotNull(nameof(memoryCache));
            _configureCacheEntry = configureCacheEntry;
        }

        /// <summary>
        /// Gets section list.
        /// </summary>
        public IReadOnlyCollection<ICacheSection> Sections => _sections.Values.ToList();

        /// <summary>
        /// Gets or creates value according default cache behavior <see cref="CacheSettings{TValue}.Default"/>.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="sectionName">Section name.</param>
        /// <param name="key">Cache key.</param>
        /// <param name="factory">Factory method to create and customize cache item.</param>
        /// <returns><see cref="CacheResult{TValue}"/>.</returns>
        public async Task<CacheResult<TValue>> GetOrCreateAsync<TValue>(string sectionName, string key, Func<ICacheEntryContext, Task<TValue>> factory)
        {
            ICacheSection<TValue> cacheSection = GetOrCreateSection(sectionName, CacheSettings<TValue>.Default);
            return await cacheSection.GetOrCreateAsync(key, factory);
        }

        /// <summary>
        /// Gets or creates value according cache section settings.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="sectionDescriptor">Section descriptor.</param>
        /// <param name="key">Cache key.</param>
        /// <param name="factory">Factory method to create and customize cache item.</param>
        /// <returns><see cref="CacheResult{TValue}"/>.</returns>
        public async Task<CacheResult<TValue>> GetOrCreateAsync<TValue>(ICacheSectionDescriptor<TValue> sectionDescriptor, string key, Func<ICacheEntryContext, Task<TValue>> factory)
        {
            var cacheSection = GetOrCreateSection(sectionDescriptor);
            return await cacheSection.GetOrCreateAsync(key, factory);
        }

        /// <summary>
        /// Gets optional section by name.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <returns>Optional section.</returns>
        public ICacheSection? GetSectionUntyped(string sectionName)
        {
            _sections.TryGetValue(sectionName, out var cacheSection);
            return cacheSection;
        }

        /// <summary>
        /// Gets optional section by name.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="sectionName">Section name.</param>
        /// <returns>Optional section.</returns>
        public ICacheSection<TValue>? GetSection<TValue>(string sectionName)
        {
            _sections.TryGetValue(sectionName, out var cacheSection);
            return (ICacheSection<TValue>)cacheSection;
        }

        /// <summary>
        /// Gets or creates cache section by provided settings.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="sectionDescriptor">CacheSectionDescriptor.</param>
        /// <returns>Cache section.</returns>
        public ICacheSection<TValue> GetOrCreateSection<TValue>(ICacheSectionDescriptor<TValue> sectionDescriptor)
        {
            ICacheSection cacheSection = _sections.GetOrAdd(sectionDescriptor.SectionName, sectionName => CreateCacheSection(sectionName, sectionDescriptor.CacheSettings));
            return (ICacheSection<TValue>)cacheSection;
        }

        /// <summary>
        /// Gets or creates cache section by provided settings.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="sectionName">Section name.</param>
        /// <param name="cacheSettings">Cache settings.</param>
        /// <returns>Cache section.</returns>
        public ICacheSection<TValue> GetOrCreateSection<TValue>(string sectionName, ICacheSettings<TValue> cacheSettings)
        {
            ICacheSection cacheSection = _sections.GetOrAdd(sectionName, name => CreateCacheSection(name, cacheSettings));
            return (ICacheSection<TValue>)cacheSection;
        }

        /// <summary>
        /// Clears data in all sections.
        /// </summary>
        public void Clear()
        {
            foreach (var cacheSection in _sections)
            {
                cacheSection.Value.Clear();
            }
        }

        /// <summary>
        /// Freezes data in all sections.
        /// </summary>
        public void Freeze()
        {

        }

        private ICacheSection<TValue> CreateCacheSection<TValue>(string sectionName, ICacheSettings<TValue> cacheSettings)
        {
            return new CacheSection<TValue>(sectionName, _memoryCache, _configureCacheEntry, cacheSettings);
        }
    }
}
