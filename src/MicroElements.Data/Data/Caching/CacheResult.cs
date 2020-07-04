// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MicroElements.Functional;
using MicroElements.Metadata;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Represents cache result.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct CacheResult<TValue> : IMetadataProvider
    {
        public ICacheSettings Settings { get; }
        public string Key { get; }
        public TValue Value { get; }
        public Message Error { get; }
        public IPropertyContainer Metadata { get; }
        public CacheHitMiss HitMiss { get; }
        public bool IsCached { get; }

        public string SectionName => Settings.SectionName;

        public CacheResult(
            ICacheSettings settings,
            string key,
            TValue value,
            Message error,
            IPropertyContainer metadata,
            CacheHitMiss hitMiss,
            bool isCached)
        {
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
