// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MicroElements.Functional;
using MicroElements.Metadata;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Context for value validation.
    /// Allows to override default validation.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class ValidationContext<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContext{TValue}"/> class.
        /// </summary>
        /// <param name="cacheSection">Cache section descriptor.</param>
        /// <param name="value">Value to check.</param>
        /// <param name="metadata">Metadata.</param>
        public ValidationContext(ICacheSectionDescriptor<TValue> cacheSection, TValue value, IMutablePropertyContainer metadata)
        {
            CacheSection = cacheSection;
            Value = value;
            Metadata = metadata;
            ShouldCache = true;
        }

        /// <summary>
        /// Cache section descriptor.
        /// </summary>
        public ICacheSectionDescriptor<TValue> CacheSection { get; }

        /// <summary>
        /// Value to check.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Allows to set metadata for value.
        /// </summary>
        public IMutablePropertyContainer Metadata { get; }

        /// <summary>
        /// Set error message if <see cref="Value"/> is bad.
        /// </summary>
        public Message? Error { get; set; }

        /// <summary>
        /// Cache or not to cache. Set true if you want to cache value (good or bad).
        /// </summary>
        public bool? ShouldCache { get; set; }
    }
}
