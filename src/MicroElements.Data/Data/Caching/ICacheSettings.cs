// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

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
        /// Can be overriden in cache item factory method.
        /// </summary>
        string DataSource { get; }

        /// <summary>
        /// Gets optional exception handler for handling exceptions while factory creation.
        /// </summary>
        public Action<ErrorHandleContext>? HandleErrorUntyped { get; }
    }

    /// <summary>
    /// Typed cache settings.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public interface ICacheSettings<TValue> : ICacheSettings
    {
        /// <summary>
        /// Gets optional value validation func to determine error.
        /// </summary>
        Action<ValidationContext<TValue>>? Validate { get; }

        /// <summary>
        /// Gets optional exception handler for handling exceptions while factory creation.
        /// </summary>
        Action<ErrorHandleContext<TValue>>? HandleError { get; }
    }
}
