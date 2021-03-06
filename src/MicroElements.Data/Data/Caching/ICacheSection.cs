﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroElements.Functional;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Represents base cache section abstraction.
    /// </summary>
    public interface ICacheSection
    {
        /// <summary>
        /// Gets section name.
        /// </summary>
        string SectionName { get; }

        /// <summary>
        /// Gets section options.
        /// </summary>
        ICacheSettings SettingsUntyped { get; }

        /// <summary>
        /// Gets all cache keys in section.
        /// </summary>
        IReadOnlyCollection<string> Keys { get; }

        /// <summary>
        /// Gets full cache entry info.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>Optional value.</returns>
        ICacheResult GetCacheEntryUntyped(string key);

        /// <summary>
        /// Clears section items.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Represents typed cache section.
    /// </summary>
    /// <typeparam name="TValue">Data type.</typeparam>
    public interface ICacheSection<TValue> : ICacheSection
    {
        /// <summary>
        /// Gets section settings.
        /// </summary>
        ICacheSettings<TValue> Settings { get; }

        /// <summary>
        /// Gets cached result or creates item with <paramref name="factory"/>.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <param name="factory">Factory method to create item on cache miss.</param>
        /// <returns>Cache result with extended info.</returns>
        Task<CacheResult<TValue>> GetOrCreateAsync(string key, Func<ICacheEntryContext, Task<TValue>> factory);

        /// <summary>
        /// Gets optional value by key.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>Optional value.</returns>
        Option<TValue> Get(string key);

        /// <summary>
        /// Gets full cache entry info.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>Optional value.</returns>
        CacheResult<TValue> GetCacheEntry(string key);

        /// <summary>
        /// Sets value for <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <param name="value">Value to cache.</param>
        /// <param name="configure">Optional cache item configure action.</param>
        void Set(string key, TValue value, Action<ICacheEntryContext>? configure = null);

        /// <summary>
        /// Removes value by key.
        /// </summary>
        /// <param name="key">Cache key.</param>
        void Remove(string key);
    }
}
