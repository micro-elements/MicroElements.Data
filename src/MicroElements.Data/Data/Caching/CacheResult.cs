// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MicroElements.Functional;
using MicroElements.Metadata;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Represents cache result.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public readonly struct CacheResult<TValue> : IMetadataProvider
    {
        /// <summary>
        /// Section name.
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// Cache settings.
        /// </summary>
        public ICacheSettings Settings { get; }

        /// <summary>
        /// Cache key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Cached value if present and not error <see cref="Error"/>.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Optional error.
        /// </summary>
        public Message? Error { get; }

        /// <summary>
        /// Metadata associated with cache item.
        /// </summary>
        public IPropertyContainer Metadata { get; }

        /// <summary>
        /// Cache hit or miss.
        /// </summary>
        public CacheHitMiss HitMiss { get; }

        /// <summary>
        /// Returns true if item was cached.
        /// </summary>
        public bool IsCached { get; }

        public CacheResult(
            string sectionName,
            ICacheSettings settings,
            string key,
            TValue value,
            Message? error,
            IPropertyContainer metadata,
            CacheHitMiss hitMiss,
            bool isCached)
        {
            SectionName = sectionName;
            Settings = settings;
            Key = key;
            Value = value;
            Error = error;
            Metadata = metadata;
            HitMiss = hitMiss;
            IsCached = isCached;
        }

        public static implicit operator TValue(CacheResult<TValue> cacheResult) => cacheResult.Value;
    }
}
