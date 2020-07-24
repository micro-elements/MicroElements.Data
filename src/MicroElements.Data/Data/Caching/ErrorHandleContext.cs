// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using MicroElements.Functional;
using MicroElements.Metadata;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Context for error processing.
    /// Allows to override default error handling.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class ErrorHandleContext<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandleContext{TValue}"/> class.
        /// </summary>
        /// <param name="cacheSection">Cache section descriptor.</param>
        /// <param name="exception">Exception raised in factory method.</param>
        /// <param name="metadata">Allows to set metadata for cache entry.</param>
        public ErrorHandleContext(ICacheSectionDescriptor<TValue> cacheSection, Exception exception, IMutablePropertyContainer metadata)
        {
            CacheSection = cacheSection;
            Exception = exception;
            Metadata = metadata;
        }

        /// <summary>
        /// Cache section descriptor.
        /// </summary>
        public ICacheSectionDescriptor<TValue> CacheSection { get; }

        /// <summary>
        /// Exception raised in factory method.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Allows to set metadata for cache entry.
        /// </summary>
        public IMutablePropertyContainer Metadata { get; }

        /// <summary>
        /// Override error message. Can skip error.
        /// </summary>
        public Message? Error { get; set; }

        /// <summary>
        /// Set value if you want to handle error and return smth valuable.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Cache or not to cache. Set true if you want to cache value (good or bad).
        /// </summary>
        public bool? ShouldCache { get; set; }
    }

    /// <summary>
    /// Context for error processing.
    /// Allows to override default error handling.
    /// </summary>
    public class ErrorHandleContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandleContext"/> class.
        /// </summary>
        /// <param name="cacheSection">Cache section descriptor.</param>
        /// <param name="exception">Exception raised in factory method.</param>
        /// <param name="metadata">Allows to set metadata for cache entry.</param>
        public ErrorHandleContext(ICacheSectionDescriptor cacheSection, Exception exception, IMutablePropertyContainer metadata)
        {
            CacheSection = cacheSection;
            Exception = exception;
            Metadata = metadata;
        }

        /// <summary>
        /// Cache section descriptor.
        /// </summary>
        public ICacheSectionDescriptor CacheSection { get; }

        /// <summary>
        /// Exception raised in factory method.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Allows to set metadata for cache entry.
        /// </summary>
        public IMutablePropertyContainer Metadata { get; }

        /// <summary>
        /// Override error message. Can skip error.
        /// </summary>
        public Message? Error { get; set; }

        /// <summary>
        /// Cache or not to cache. Set true if you want to cache value (good or bad).
        /// </summary>
        public bool? ShouldCache { get; set; }
    }
}
