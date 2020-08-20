// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MicroElements.Functional;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Extensions for cache.
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Gets all values from <see cref="ICacheSection{TValue}"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheSection">Cache section.</param>
        /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
        [return: NotNull]
        public static IEnumerable<T> GetAllValues<T>([NotNull] this ICacheSection<T> cacheSection)
        {
            cacheSection.AssertArgumentNotNull(nameof(cacheSection));

            var values = cacheSection
                .Keys
                .Select(key => cacheSection.Get(key))
                .Where(option => option.IsSome)
                .Select(option => option.GetValueOrDefault());

            return values;
        }

        /// <summary>
        /// Gets all cache entries from <see cref="ICacheSection{TValue}"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheSection">Cache section.</param>
        /// <returns>Enumerable of <see cref="CacheResult{TValue}"/>.</returns>
        [return: NotNull]
        public static IEnumerable<CacheResult<T>> GetAllEntries<T>([NotNull] this ICacheSection<T> cacheSection)
        {
            cacheSection.AssertArgumentNotNull(nameof(cacheSection));

            var values = cacheSection
                .Keys
                .Select(key => cacheSection.GetCacheEntry(key))
                .Where(cacheResult => !cacheResult.IsEmpty);

            return values;
        }

        /// <summary>
        /// Gets all cache entries from <see cref="ICacheSection{TValue}"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheSection">Cache section.</param>
        /// <returns>Enumerable of <see cref="CacheResult{TValue}"/>.</returns>
        [return: NotNull]
        public static IEnumerable<ICacheResult> GetAllEntriesUntyped([NotNull] this ICacheSection cacheSection)
        {
            cacheSection.AssertArgumentNotNull(nameof(cacheSection));

            var values = cacheSection
                .Keys
                .Select(key => cacheSection.GetCacheEntryUntyped(key))
                .Where(cacheResult => cacheResult != null);

            return values;
        }

        /// <summary>
        /// Gets all values from <paramref name="cacheManager"/> using <paramref name="sectionDescriptor"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheManager">Cache manager.</param>
        /// <param name="sectionDescriptor">Section descriptor to get or create cache section.</param>
        /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
        [return: NotNull]
        public static IEnumerable<T> GetAllValues<T>([NotNull] this CacheManager cacheManager, [NotNull] ICacheSectionDescriptor<T> sectionDescriptor)
        {
            cacheManager.AssertArgumentNotNull(nameof(cacheManager));
            sectionDescriptor.AssertArgumentNotNull(nameof(sectionDescriptor));

            var cacheSection = cacheManager.GetOrCreateSection(sectionDescriptor);
            return cacheSection.GetAllValues();
        }

        /// <summary>
        /// Gets all cache entries from <paramref name="cacheManager"/> using <paramref name="sectionDescriptor"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheManager">Cache manager.</param>
        /// <param name="sectionDescriptor">Section descriptor to get or create cache section.</param>
        /// <returns>Enumerable of <see cref="CacheResult{TValue}"/>.</returns>
        [return: NotNull]
        public static IEnumerable<CacheResult<T>> GetAllEntries<T>([NotNull] this CacheManager cacheManager, [NotNull] ICacheSectionDescriptor<T> sectionDescriptor)
        {
            cacheManager.AssertArgumentNotNull(nameof(cacheManager));
            sectionDescriptor.AssertArgumentNotNull(nameof(sectionDescriptor));

            var cacheSection = cacheManager.GetOrCreateSection(sectionDescriptor);
            return cacheSection.GetAllEntries();
        }

        /// <summary>
        /// Gets all values from <paramref name="cacheManager"/> using <paramref name="sectionName"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheManager">Cache manager.</param>
        /// <param name="sectionName">Section name to find cache section.</param>
        /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
        [return: NotNull]
        public static IEnumerable<T> GetAllValues<T>([NotNull] this CacheManager cacheManager, [NotNull] string sectionName)
        {
            cacheManager.AssertArgumentNotNull(nameof(cacheManager));
            sectionName.AssertArgumentNotNull(nameof(sectionName));

            var cacheSection = cacheManager.GetSection<T>(sectionName);
            if (cacheSection == null)
                return Array.Empty<T>();

            return cacheSection.GetAllValues();
        }

        /// <summary>
        /// Gets all cache entries from <paramref name="cacheManager"/> using <paramref name="sectionName"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheManager">Cache manager.</param>
        /// <param name="sectionName">Section name to find cache section.</param>
        /// <returns>Enumerable of <see cref="CacheResult{TValue}"/>.</returns>
        [return: NotNull]
        public static IEnumerable<CacheResult<T>> GetAllEntries<T>([NotNull] this CacheManager cacheManager, [NotNull] string sectionName)
        {
            cacheManager.AssertArgumentNotNull(nameof(cacheManager));
            sectionName.AssertArgumentNotNull(nameof(sectionName));

            var cacheSection = cacheManager.GetSection<T>(sectionName);
            if (cacheSection == null)
                return Array.Empty<CacheResult<T>>();

            return cacheSection.GetAllEntries();
        }

        /// <summary>
        /// Gets all key-value pairs from <see cref="ICacheSection{TValue}"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheSection">Cache section.</param>
        /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
        public static IEnumerable<(string Key, T Value)> GetAllKeyValues<T>([NotNull] this ICacheSection<T> cacheSection)
        {
            cacheSection.AssertArgumentNotNull(nameof(cacheSection));

            var values = cacheSection
                .Keys
                .Select(key => (key, cacheSection.Get(key)))
                .Where(kv => kv.Item2.IsSome)
                .Select(kv => (kv.key, kv.Item2.GetValueOrDefault()));

            return values;
        }

        /// <summary>
        /// Gets all key-value pairs from <paramref name="cacheManager"/> using <paramref name="sectionDescriptor"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheManager">Cache manager.</param>
        /// <param name="sectionDescriptor">Section descriptor to get or create cache section.</param>
        /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
        [return: NotNull]
        public static IEnumerable<(string Key, T Value)> GetAllKeyValues<T>([NotNull] this CacheManager cacheManager, [NotNull] ICacheSectionDescriptor<T> sectionDescriptor)
        {
            cacheManager.AssertArgumentNotNull(nameof(cacheManager));
            sectionDescriptor.AssertArgumentNotNull(nameof(sectionDescriptor));

            var cacheSection = cacheManager.GetOrCreateSection(sectionDescriptor);
            return cacheSection.GetAllKeyValues();
        }

        /// <summary>
        /// Gets all key-value pairs from <paramref name="cacheManager"/> using <paramref name="sectionName"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cacheManager">Cache manager.</param>
        /// <param name="sectionName">Section name to find cache section.</param>
        /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
        [return: NotNull]
        public static IEnumerable<(string Key, T Value)> GetAllKeyValues<T>([NotNull] this CacheManager cacheManager, [NotNull] string sectionName)
        {
            cacheManager.AssertArgumentNotNull(nameof(cacheManager));
            sectionName.AssertArgumentNotNull(nameof(sectionName));

            var cacheSection = cacheManager.GetSection<T>(sectionName);
            if (cacheSection == null)
                return Array.Empty<(string, T)>();

            return cacheSection.GetAllKeyValues();
        }
    }
}
