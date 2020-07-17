// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Cache related exception.
    /// </summary>
    [Serializable]
    public class CacheException : Exception
    {
        /// <inheritdoc />
        public CacheException() { }

        /// <inheritdoc />
        protected CacheException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <inheritdoc />
        public CacheException(string message) : base(message) { }

        /// <inheritdoc />
        public CacheException(string message, Exception innerException) : base(message, innerException) { }
    }
}
