// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
        /// Gets <see cref="Value"/>.
        /// </summary>
        /// <returns>Value.</returns>
        public TValue GetValue() => Value;

        /// <summary>
        /// Gets <see cref="Value"/> if <see cref="IsSuccess"/> or default if error.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value or default value if not success.</returns>
        public TValue GetValueOrDefault(TValue defaultValue = default) => IsSuccess ? Value : defaultValue;

        /// <summary>
        /// Gets <see cref="Value"/> if <see cref="IsSuccess"/> or throws <see cref="CacheException"/> if in error state.
        /// </summary>
        /// <returns>Value.</returns>
        public TValue GetValueOrThrow() => IsSuccess ? Value : throw new CacheException(Error.FormattedMessage, Error.GetException());
    }

    /// <summary>
    /// CacheResult extensions.
    /// </summary>
    public static class CacheResult
    {
        /// <summary>
        /// Time elapsed on getting value.
        /// </summary>
        public static readonly IProperty<TimeSpan> Elapsed = new Property<TimeSpan>("Elapsed").WithDescription("Time elapsed on getting value.");

        /// <summary>
        /// The source of value.
        /// </summary>
        public static readonly IProperty<string> DataSource = new Property<string>("DataSource").WithDescription("The source of value.");

        /// <summary>
        /// Gets <see cref="CacheResult.Elapsed"/> value.
        /// </summary>
        public static TimeSpan GetElapsed(this IMetadataProvider provider) => provider.Metadata.GetValue(CacheResult.Elapsed);

        /// <summary>
        /// Gets <see cref="CacheResult.DataSource"/> value.
        /// </summary>
        public static string GetDataSource(this IMetadataProvider provider) => provider.Metadata.GetValue(CacheResult.DataSource);

        /// <summary>
        /// Creates an empty result.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSection">Owner cache section.</param>
        /// <param name="key">Cache key.</param>
        /// <returns>Empty cache result.</returns>
        public static CacheResult<TValue> Empty<TValue>(
            ICacheSectionDescriptor<TValue> cacheSection,
            string key)
        {
            return new CacheResult<TValue>(
                cacheSection,
                key,
                value: default,
                error: null,
                metadata: null,
                hitMiss: CacheHitMiss.Miss,
                isCached: false);
        }
    }
}
