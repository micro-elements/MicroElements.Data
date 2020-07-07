// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using MicroElements.Functional;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Represents cache behavior.
    /// </summary>
    public interface ICacheSettings
    {
        /// <summary>
        /// Gets value type that cache section holds.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Gets a value indicating whether cache should cache error values.
        /// </summary>
        bool CacheErrorValue { get; }

        /// <summary>
        /// Gets default DataSource for cache items.
        /// Can be overriden in cache item factory method. TODO: check usage
        /// </summary>
        string DataSource { get; }

        /// <summary>
        /// Gets optional exception handler for handling exceptions while factory creation.
        /// </summary>
        Func<Exception, Message>? HandleError { get; }
    }

    /// <summary>
    /// Typed cache settings.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public interface ICacheSettings<in TValue> : ICacheSettings
    {
        /// <summary>
        /// Gets optional value validation func to determine error.
        /// </summary>
        Func<TValue, Message>? Validate { get; }
    }
}
