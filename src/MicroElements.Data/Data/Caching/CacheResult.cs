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
        /// Cached value if present and is not error <see cref="Error"/>.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheResult{TValue}"/> struct.
        /// </summary>
        public CacheResult(
            ICacheSectionDescriptor<TValue> cacheSection,
            string key,
            TValue value,
            Message? error,
            IPropertyContainer metadata,
            CacheHitMiss hitMiss,
            bool isCached)
        {
            SectionName = cacheSection.SectionName;
            Settings = cacheSection.CacheSettings;
            Key = key;
            Value = value;
            Error = error;
            Metadata = metadata;
            HitMiss = hitMiss;
            IsCached = isCached;
        }

        public CacheResult(
            ICacheSectionDescriptor<TValue> cacheSection,
            string key)
        {
            SectionName = cacheSection.SectionName;
            Settings = cacheSection.CacheSettings;
            Key = key;

            Value = default;
            Error = null;
            Metadata = null;
            HitMiss = CacheHitMiss.Miss;
            IsCached = false;
        }

        /// <summary>
        /// Converts to base type.
        /// </summary>
        /// <param name="cacheResult">Cache result.</param>
        /// <returns>Value if no error or exception.</returns>
        /// <exception cref="CacheException">Cache error.</exception>
        public static implicit operator TValue(CacheResult<TValue> cacheResult) => cacheResult.GetValueOrThrow();

        /// <summary>
        /// True if result is in success state.
        /// </summary>
        public bool IsSuccess => Error == null || Error.Severity != MessageSeverity.Error;

        /// <summary>
        /// Result is empty (Not found for example).
        /// </summary>
        public bool IsEmpty => Error == null && Metadata == null && HitMiss == CacheHitMiss.Miss && IsCached == false;

        /// <summary>
        /// Gets value or default.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value or default value if not success.</returns>
        public TValue GetValueOrDefault(TValue defaultValue = default) => IsSuccess ? Value : defaultValue;

        /// <summary>
        /// Gets value or throws <see cref="CacheException"/> if in error state.
        /// </summary>
        /// <returns>Value.</returns>
        public TValue GetValueOrThrow() => IsSuccess ? Value : throw new CacheException(Error.FormattedMessage, Error.GetException());
    }
}
