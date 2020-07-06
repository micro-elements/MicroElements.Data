// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MicroElements.Functional;
using MicroElements.Metadata;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Cache value container.
    /// </summary>
    /// <typeparam name="TValue">Data type.</typeparam>
    internal class CacheItem<TValue>
    {
        /// <summary>
        /// Cached data.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Optional error.
        /// </summary>
        public Message? Error { get; }

        /// <summary>
        /// Metadata associated with cached item.
        /// </summary>
        public IPropertyContainer Metadata { get; }

        /// <summary>
        /// Cache context.
        /// </summary>
        public ICacheContext CacheContext { get; }

        public CacheItem(TValue value, Message? error, CacheContext cacheContext)
        {
            Value = value;
            Error = error;
            CacheContext = cacheContext;
            Metadata = cacheContext.Metadata;
        }
    }
}
