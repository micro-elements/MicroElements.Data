// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MicroElements.Metadata;
using Microsoft.Extensions.Caching.Memory;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// CacheContext uses for cache item customization.
    /// </summary>
    public interface ICacheContext
    {
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
    internal class CacheContext : ICacheContext
    {
        /// <inheritdoc />
        public ICacheEntry CacheEntry { get; }

        /// <inheritdoc />
        public IMutablePropertyContainer Metadata { get; }

        public CacheContext(ICacheEntry cacheEntry)
        {
            CacheEntry = cacheEntry;
            Metadata = new MutablePropertyContainer();
        }
    }
}
