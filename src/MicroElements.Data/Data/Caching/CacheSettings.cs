// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

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
            HandleErrorUntyped = null,

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
        public Action<ErrorHandleContext>? HandleErrorUntyped { get; set; }

        public CacheSettings() { }

        public CacheSettings(
            ICacheSettings? @base = null,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Action<ErrorHandleContext>? handleErrorUntyped = null)
        {
            @base ??= Default;
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleErrorUntyped = handleErrorUntyped ?? @base.HandleErrorUntyped;
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
        public Action<ErrorHandleContext>? HandleErrorUntyped { get; set; }

        /// <inheritdoc />
        public Action<ErrorHandleContext<TValue>>? HandleError { get; set; }

        /// <inheritdoc />
        public Action<ValidationContext<TValue>>? Validate { get; set; }

        public CacheSettings()
        {
        }

        public CacheSettings(
            ICacheSettings? @base = null,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Action<ErrorHandleContext>? handleErrorUntyped = null,
            Action<ErrorHandleContext<TValue>>? handleError = null,
            Action<ValidationContext<TValue>>? validate = null)
        {
            @base ??= Default;
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleErrorUntyped = handleErrorUntyped ?? @base.HandleErrorUntyped;
            HandleError = handleError;
            Validate = validate;
        }

        public CacheSettings(
            ICacheSettings<TValue>? @base = null,
            bool? cacheErrorValue = null,
            string? dataSource = null,
            Action<ErrorHandleContext>? handleErrorUntyped = null,
            Action<ErrorHandleContext<TValue>>? handleError = null,
            Action<ValidationContext<TValue>>? validate = null)
        {
            @base ??= Default;
            CacheErrorValue = cacheErrorValue ?? @base.CacheErrorValue;
            DataSource = dataSource ?? @base.DataSource;
            HandleErrorUntyped = handleErrorUntyped ?? @base.HandleErrorUntyped;
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
        public Action<ErrorHandleContext>? HandleErrorUntyped { get; }

        /// <inheritdoc />
        public Action<ErrorHandleContext<TValue>>? HandleError { get; }

        /// <inheritdoc />
        public Action<ValidationContext<TValue>>? Validate { get; }

        public ReadOnlyCacheSettings(
            bool cacheErrorValue,
            string dataSource,
            Action<ErrorHandleContext>? handleErrorUntyped,
            Action<ErrorHandleContext<TValue>>? handleError,
            Action<ValidationContext<TValue>>? validate)
        {
            CacheErrorValue = cacheErrorValue;
            DataSource = dataSource;
            HandleErrorUntyped = handleErrorUntyped;
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

        /// <summary>
        /// Cache does not process exceptions in factory.
        /// Use <see cref="CacheResult{TValue}.GetValueOrThrow"/> to rethrow catched exception.
        /// Sets <see cref="ICacheSettings.HandleErrorUntyped"/> to null.
        /// </summary>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <returns>New instance of <see cref="ICacheSettings"/>.</returns>
        public static ICacheSettings DoNotHandleErrors(this ICacheSettings cacheSettings)
        {
            return new CacheSettings(cacheSettings, handleErrorUntyped: null);
        }

        /// <summary>
        /// Cache does not process exceptions in factory.
        /// Use <see cref="CacheResult{TValue}.GetValueOrThrow"/> to rethrow catched exception.
        /// Sets <see cref="ICacheSettings.HandleErrorUntyped"/> and <see cref="ICacheSettings{TValue}.HandleError"/> to null.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <returns>New instance of <see cref="ICacheSettings{TValue}"/>.</returns>
        public static ICacheSettings<TValue> DoNotHandleErrors<TValue>(this ICacheSettings<TValue> cacheSettings)
        {
            return new CacheSettings<TValue>(cacheSettings, handleErrorUntyped: null, handleError: null);
        }

        /// <summary>
        /// Allows to override default error handling.
        /// </summary>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <param name="handleErrorUntyped">Action to handle exceptions.</param>
        /// <returns>New instance of <see cref="ICacheSettings"/>.</returns>
        public static ICacheSettings SetHandleErrorUntyped(this ICacheSettings cacheSettings, Action<ErrorHandleContext> handleErrorUntyped)
        {
            return new CacheSettings(cacheSettings, handleErrorUntyped: handleErrorUntyped);
        }

        /// <summary>
        /// Allows to override default error handling.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <param name="handleErrorUntyped">Action to handle exceptions.</param>
        /// <returns>New instance of <see cref="ICacheSettings"/>.</returns>
        public static ICacheSettings<TValue> SetHandleErrorUntyped<TValue>(this ICacheSettings<TValue> cacheSettings, Action<ErrorHandleContext> handleErrorUntyped)
        {
            return new CacheSettings<TValue>(cacheSettings, handleErrorUntyped: handleErrorUntyped);
        }

        /// <summary>
        /// Allows to override default error handling.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <param name="handleError">Action to handle exceptions.</param>
        /// <returns>New instance of <see cref="ICacheSettings{TValue}"/>.</returns>
        public static ICacheSettings<TValue> SetHandleError<TValue>(this ICacheSettings<TValue> cacheSettings, Action<ErrorHandleContext<TValue>>? handleError)
        {
            return new CacheSettings<TValue>(cacheSettings, handleError: handleError);
        }

        /// <summary>
        /// Allows to override default validation.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <param name="validate">Action to handle validation.</param>
        /// <returns>New instance of <see cref="ICacheSettings"/>.</returns>
        public static ICacheSettings<TValue> SetValidate<TValue>(this ICacheSettings<TValue> cacheSettings, Action<ValidationContext<TValue>> validate)
        {
            return new CacheSettings<TValue>(cacheSettings, validate: validate);
        }

        /// <summary>
        /// Converts to typed <see cref="ICacheSettings{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <returns>New instance of <see cref="ICacheSettings{TValue}"/>.</returns>
        public static ICacheSettings<TValue> Typed<TValue>(this ICacheSettings cacheSettings)
        {
            return new CacheSettings<TValue>(cacheSettings);
        }

        /// <summary>
        /// Creates new <see cref="ICacheSectionDescriptor{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <param name="sectionName">Cache section name.</param>
        /// <returns>New instance of <see cref="ICacheSectionDescriptor{TValue}"/>.</returns>
        public static ICacheSectionDescriptor<TValue> CreateSectionDescriptor<TValue>(this ICacheSettings<TValue> cacheSettings, string sectionName)
        {
            return new CacheSectionDescriptor<TValue>(sectionName, cacheSettings.AsReadOnly());
        }

        /// <summary>
        /// Creates readonly wrapper for <see cref="ICacheSettings{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="cacheSettings">Source cache settings.</param>
        /// <returns>New instance of <see cref="ICacheSettings{TValue}"/>.</returns>
        public static ICacheSettings<TValue> AsReadOnly<TValue>(this ICacheSettings<TValue> cacheSettings)
        {
            return new ReadOnlyCacheSettings<TValue>(
                cacheErrorValue: cacheSettings.CacheErrorValue,
                dataSource: cacheSettings.DataSource,
                handleErrorUntyped: cacheSettings.HandleErrorUntyped,
                handleError: cacheSettings.HandleError,
                validate: cacheSettings.Validate);
        }
    }
}
