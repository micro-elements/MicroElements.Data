// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MicroElements.Functional;
using MicroElements.Metadata;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Cached data container.
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
        /// Initializes a new instance of the <see cref="CacheItem{TValue}"/> class.
        /// </summary>
        /// <param name="value">Cached data.</param>
        /// <param name="error">Optional error or message.</param>
        /// <param name="metadata">Metadata associated with cache item.</param>
        public CacheItem(TValue value, Message? error, IPropertyContainer metadata)
        {
            Value = value;
            Error = error;
            Metadata = metadata;
        }
    }
}
