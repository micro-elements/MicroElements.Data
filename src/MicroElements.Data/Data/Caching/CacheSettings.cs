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
        public static readonly ICacheSettings<TValue> Default = new CacheSettings<TValue>(CacheSettings.Default)
        {
            // No validation. (Any value is not error)
            Validate = null,
        };

        /// <inheritdoc />
        public Type ValueType => typeof(TValue);

        /// <inheritdoc />
        public bool CacheErrorValue { get; set; }

        /// <inheritdoc />
        public string DataSource { get; set; }

        /// <inheritdoc />
        public Func<Exception, Message>? HandleError { get; set; }

        /// <inheritdoc />
        public Func<TValue, Message>? Validate { get; set; }

        public CacheSettings()
        {
        }

        public CacheSettings(
            ICacheSettings @base,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Func<Exception, Message>? handleError = null,
            Func<TValue, Message>? validate = null)
        {
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleError = handleError ?? @base.HandleError;
            Validate = validate;
        }

        public CacheSettings(
            ICacheSettings<TValue> @base,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Func<Exception, Message>? handleError = null,
            Func<TValue, Message>? validate = null)
        {
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleError = handleError ?? @base.HandleError;
            Validate = validate ?? @base.Validate;
        }
    }

    /// <summary>
    /// Untyped cache settings.
    /// </summary>
    public class CacheSettings : ICacheSettings
    {
        /// <summary>
        /// Default cache settings.
        /// </summary>
        public static readonly ICacheSettings Default = new CacheSettings
        {
            // Do not cache errors by default
            CacheErrorValue = false,

            // Does not handle exceptions (rethrow)
            HandleError = null,

            // Unknown data source.
            DataSource = "Unknown",
        };

        /// <inheritdoc />
        public Type ValueType => typeof(object);

        /// <inheritdoc />
        public bool CacheErrorValue { get; private set; }

        /// <inheritdoc />
        public string DataSource { get; private set; }

        /// <inheritdoc />
        public Func<Exception, Message>? HandleError { get; private set; }

        public CacheSettings() { }

        public CacheSettings(
            ICacheSettings @base,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Func<Exception, Message>? handleError = null)
        {
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleError = handleError ?? @base.HandleError;
        }
    }

    public static class CacheSettingsExtensions
    {
        public static ICacheSettings SetCacheErrorValue(this ICacheSettings cacheSettings, bool cacheErrorValue = true)
        {
            return new CacheSettings(cacheSettings, cacheErrorValue: cacheErrorValue);
        }

        public static ICacheSettings<TValue> SetCacheErrorValue<TValue>(this ICacheSettings<TValue> cacheSettings, bool cacheErrorValue = true)
        {
            return new CacheSettings<TValue>(cacheSettings, cacheErrorValue: cacheErrorValue);
        }

        public static ICacheSettings SetHandleError(this ICacheSettings cacheSettings, Func<Exception, Message> handleError)
        {
            return new CacheSettings(cacheSettings, handleError: handleError);
        }

        public static ICacheSettings<TValue> SetHandleError<TValue>(this ICacheSettings<TValue> cacheSettings, Func<Exception, Message> handleError)
        {
            return new CacheSettings<TValue>(cacheSettings, handleError: handleError);
        }

        public static ICacheSettings<TValue> SetValidate<TValue>(this ICacheSettings<TValue> cacheSettings, Func<TValue, Message> validate)
        {
            return new CacheSettings<TValue>(cacheSettings, validate: validate);
        }

        public static ICacheSettings<TValue> Typed<TValue>(this ICacheSettings cacheSettings)
        {
            return new CacheSettings<TValue>(cacheSettings);
        }

        public static ICacheSectionDescriptor<TValue> CreateSectionDescriptor<TValue>(this ICacheSettings<TValue> cacheSettings, string sectionName)
        {
            return new CacheSectionDescriptor<TValue>(sectionName, cacheSettings);
        }
    }
}
