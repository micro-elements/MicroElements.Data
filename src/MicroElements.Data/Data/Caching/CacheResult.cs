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
    public interface ICacheResult : IManualMetadataProvider
    {
        /// <summary>
        /// Section name.
        /// </summary>
        string SectionName { get; }

        /// <summary>
        /// Cache settings.
        /// </summary>
        ICacheSettings Settings { get; }

        /// <summary>
        /// Cache key.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Cached value if present and is not error <see cref="Error"/>.
        /// </summary>
        object? ValueUntyped { get; }

        /// <summary>
        /// Optional error.
        /// </summary>
        Message? Error { get; }

        /// <summary>
        /// Cache hit or miss.
        /// </summary>
        CacheHitMiss HitMiss { get; }

        /// <summary>
        /// Returns true if item was cached.
        /// </summary>
        bool IsCached { get; }
    }

    /// <summary>
    /// Represents typed cache result.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public interface ICacheResult<TValue> : ICacheResult
    {
        /// <summary>
        /// Cached value if present and is not error <see cref="ICacheResult.Error"/>.
        /// </summary>
        TValue Value { get; }
    }

    /// <summary>
    /// Represents typed cache result.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public readonly struct CacheResult<TValue> : ICacheResult<TValue>
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
        /// Cached value if present and is not error <see cref="Error"/>.
        /// </summary>
        public object? ValueUntyped => Value;

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
        /// Gets <see cref="Value"/>.
        /// </summary>
        /// <returns>Value.</returns>
        public TValue GetValue() => Value;

        /// <summary>
        /// Gets <see cref="Value"/> if <see cref="CacheResult.IsSuccess"/> or default if error.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value or default value if not success.</returns>
        public TValue GetValueOrDefault(TValue defaultValue = default) => this.IsSuccess() ? Value : defaultValue;

        /// <summary>
        /// Gets <see cref="Value"/> if <see cref="CacheResult.IsSuccess"/> or throws <see cref="CacheException"/> if in error state.
        /// </summary>
        /// <returns>Value.</returns>
        public TValue GetValueOrThrow() => this.IsSuccess() ? Value : throw new CacheException(Error.FormattedMessage, Error.GetException());
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
        public static TimeSpan GetElapsed(this ICacheResult provider) => provider.Metadata.GetValue(CacheResult.Elapsed);

        /// <summary>
        /// Gets <see cref="CacheResult.DataSource"/> value.
        /// </summary>
        public static string? GetDataSource(this ICacheResult provider) => provider.Metadata.GetValue(CacheResult.DataSource);

        /// <summary>
        /// True if result is in success state.
        /// </summary>
        public static bool IsSuccess(this ICacheResult result) => result.Error == null || result.Error.Severity != MessageSeverity.Error;

        /// <summary>
        /// True if result is in success state.
        /// </summary>
        public static bool IsSuccess<T>(this in CacheResult<T> result) => result.Error == null || result.Error.Severity != MessageSeverity.Error;

        /// <summary>
        /// Result is empty (Not found for example).
        /// </summary>
        public static bool IsEmpty(this ICacheResult result) => result.Error == null && !result.HasMetadata() && result.HitMiss == CacheHitMiss.Miss && result.IsCached == false;

        /// <summary>
        /// Result is empty (Not found for example).
        /// </summary>
        public static bool IsEmpty<T>(this in CacheResult<T> result) => result.Error == null && !result.HasMetadata() && result.HitMiss == CacheHitMiss.Miss && result.IsCached == false;

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
                metadata: PropertyContainer.Empty,
                hitMiss: CacheHitMiss.Miss,
                isCached: false);
        }
    }
}
