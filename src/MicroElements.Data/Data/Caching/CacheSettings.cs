// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using MicroElements.Functional;

namespace MicroElements.Data.Caching
{
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
        public bool CacheErrorValue { get; set; }

        /// <inheritdoc />
        public string DataSource { get; set; }

        /// <inheritdoc />
        public Func<Exception, Message>? HandleError { get; set; }

        public CacheSettings() { }

        public CacheSettings(
            ICacheSettings? @base = null,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Func<Exception, Message>? handleError = null)
        {
            @base ??= Default;
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleError = handleError ?? @base.HandleError;
        }
    }

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
            ICacheSettings? @base = null,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Func<Exception, Message>? handleError = null,
            Func<TValue, Message>? validate = null)
        {
            @base ??= Default;
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleError = handleError ?? @base.HandleError;
            Validate = validate;
        }

        public CacheSettings(
            ICacheSettings<TValue>? @base = null,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Func<Exception, Message>? handleError = null,
            Func<TValue, Message>? validate = null)
        {
            @base ??= Default;
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleError = handleError ?? @base.HandleError;
            Validate = validate ?? @base.Validate;
        }
    }

    /// <summary>
    /// ReadOnly Typed cache settings.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class ReadOnlyCacheSettings<TValue> : ICacheSettings<TValue>
    {
        /// <inheritdoc />
        public Type ValueType => typeof(TValue);

        /// <inheritdoc />
        public bool CacheErrorValue { get; }

        /// <inheritdoc />
        public string DataSource { get; }

        /// <inheritdoc />
        public Func<Exception, Message>? HandleError { get; }

        /// <inheritdoc />
        public Func<TValue, Message>? Validate { get; }

        public ReadOnlyCacheSettings(
            bool cacheErrorValue,
            string dataSource,
            Func<Exception, Message>? handleError,
            Func<TValue, Message>? validate)
        {
            CacheErrorValue = cacheErrorValue;
            DataSource = dataSource;
            HandleError = handleError;
            Validate = validate;
        }
    }

    /// <summary>
    /// CacheSettings extensions.
    /// </summary>
    public static class CacheSettingsExtensions
    {
        public static ICacheSettings<TValue> Configure<TValue>(this ICacheSettings<TValue> cacheSettings, Action<CacheSettings<TValue>> configure)
        {
            var settings = new CacheSettings<TValue>(cacheSettings);
            configure(settings);
            return settings;
        }

        public static ICacheSettings SetCacheErrorValue(this ICacheSettings cacheSettings, bool cacheErrorValue = true)
        {
            return new CacheSettings(cacheSettings, cacheErrorValue: cacheErrorValue);
        }

        public static ICacheSettings<TValue> SetCacheErrorValue<TValue>(this ICacheSettings<TValue> cacheSettings, bool cacheErrorValue = true)
        {
            return new CacheSettings<TValue>(cacheSettings, cacheErrorValue: cacheErrorValue);
        }

        public static ICacheSettings SetDataSource(this ICacheSettings cacheSettings, string dataSource)
        {
            return new CacheSettings(cacheSettings, dataSource: dataSource);
        }

        public static ICacheSettings<TValue> SetDataSource<TValue>(this ICacheSettings<TValue> cacheSettings, string dataSource)
        {
            return new CacheSettings<TValue>(cacheSettings, dataSource: dataSource);
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
            return new CacheSectionDescriptor<TValue>(sectionName, cacheSettings.AsReadOnly());
        }

        public static ICacheSettings<TValue> AsReadOnly<TValue>(this ICacheSettings<TValue> cacheSettings)
        {
            return new ReadOnlyCacheSettings<TValue>(
                cacheErrorValue: cacheSettings.CacheErrorValue,
                dataSource: cacheSettings.DataSource,
                handleError: cacheSettings.HandleError,
                validate: cacheSettings.Validate);
        }
    }
}
