// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using MicroElements.Functional;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Typed cache settings.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class CacheSettings<TValue> : ICacheSettings<TValue>
    {
        /// <summary>
        /// Default cache settings.
        /// </summary>
        public static readonly ICacheSettings<TValue> Default = new CacheSettings<TValue>
        {
            // Do not cache errors by default
            CacheErrorValue = false,

            // Does not handle exceptions (rethrow)
            HandleCreateError = null,

            // No validation. (Any value is not error)
            Validate = null,

            // Unknown data source.
            DataSource = "Unknown",
        };

        /// <inheritdoc />
        public Type ValueType => typeof(TValue);

        /// <inheritdoc />
        public string DataSource { get; set; }

        /// <inheritdoc />
        public bool CacheErrorValue { get; set; }

        /// <inheritdoc />
        public Func<TValue, Message>? Validate { get; set; }

        /// <inheritdoc/>
        public Func<Exception, Message>? HandleCreateError { get; set; }
    }

    public static class CacheSection
    {
        public static CacheSettingsBuilder<TValue> ForType<TValue>()
        {
            return new CacheSettingsBuilder<TValue>();
        }
    }

    public class CacheSettingsBuilder<TValue>
    {
        private CacheSettings<TValue> _cacheSettings;

        public CacheSettingsBuilder<TValue> CacheErrorValue(bool cacheErrorValue = true)
        {
            _cacheSettings.CacheErrorValue = cacheErrorValue;
            return this;
        }
    }
}
