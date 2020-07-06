// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Represents cache section description.
    /// </summary>
    public interface ICacheSectionDescriptor
    {
        /// <summary>
        /// Gets section name.
        /// </summary>
        string SectionName { get; }

        /// <summary>
        /// Gets untyped cache settings.
        /// </summary>
        ICacheSettings CacheSettingsUntyped { get; }
    }

    /// <summary>
    /// Represents cache section description: (Name, Type ans Settings).
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public interface ICacheSectionDescriptor<in TValue> : ICacheSectionDescriptor
    {
        /// <summary>
        /// Gets cache settings.
        /// </summary>
        ICacheSettings<TValue> CacheSettings { get; }
    }

    /// <summary>
    /// Represents cache section description: (Name, Type ans Settings).
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class CacheSectionDescriptor<TValue> : ICacheSectionDescriptor<TValue>
    {
        /// <inheritdoc />
        public string SectionName { get; }

        /// <inheritdoc />
        public ICacheSettings CacheSettingsUntyped => CacheSettings;

        /// <inheritdoc />
        public ICacheSettings<TValue> CacheSettings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheSectionDescriptor{TValue}"/> class.
        /// </summary>
        /// <param name="sectionName">Cache section name.</param>
        /// <param name="cacheSettings">Cache settings.</param>
        public CacheSectionDescriptor(string sectionName, ICacheSettings<TValue>? cacheSettings = null)
        {
            SectionName = sectionName;
            CacheSettings = cacheSettings ?? CacheSettings<TValue>.Default;
        }
    }
}
