// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MicroElements.Metadata;
using Microsoft.Extensions.Caching.Memory;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Use <see cref="ICacheEntryContext"/> to customize cache entry.
    /// </summary>
    public interface ICacheEntryContext
    {
        /// <summary>
        /// Cache section descriptor.
        /// </summary>
        public ICacheSectionDescriptor CacheSection { get; }

        /// <summary>
        /// Cache entry to configure.
        /// </summary>
        ICacheEntry CacheEntry { get; }

        /// <summary>
        /// Metadata assigned to cache entry.
        /// </summary>
        IMutablePropertyContainer Metadata { get; }

        // TODO: implement OnHit, OnMiss, OnExpiration
        // Action OnHit { get; set; }
        // Action OnMiss { get; set; }
        // Action OnExpiration { get; set; }
    }

    /// <summary>
    /// CacheContext uses for cache item customization.
    /// </summary>
    internal class CacheEntryContext : ICacheEntryContext
    {
        /// <summary>
        /// Cache section descriptor.
        /// </summary>
        public ICacheSectionDescriptor CacheSection { get; }

        /// <inheritdoc />
        public ICacheEntry CacheEntry { get; }

        /// <inheritdoc />
        public IMutablePropertyContainer Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntryContext"/> class.
        /// </summary>
        /// <param name="cacheSection">Cache section descriptor.</param>
        /// <param name="cacheEntry">Cache entry to configure.</param>
        public CacheEntryContext(ICacheSectionDescriptor cacheSection, ICacheEntry cacheEntry)
        {
            CacheSection = cacheSection;
            CacheEntry = cacheEntry;
            Metadata = new MutablePropertyContainer();
        }
    }
}
