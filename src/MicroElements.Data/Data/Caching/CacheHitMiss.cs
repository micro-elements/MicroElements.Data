// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Represents cache hit or miss.
    /// </summary>
    public enum CacheHitMiss
    {
        /// <summary>
        /// Cache miss. Item was not found in cache.
        /// </summary>
        Miss,

        /// <summary>
        /// Cache hit. Item was found in cache.
        /// </summary>
        Hit,
    }
}
