// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using MicroElements.Functional;

namespace MicroElements.Data.Experimental
{
    /// <summary>
    /// Represents Data with Metadata.
    /// </summary>
    /// <typeparam name="TData">Data type.</typeparam>
    /// <typeparam name="TMetadata">Metadata type.</typeparam>
    public interface IDataContainer<TData, TMetadata>
    {
        /// <summary>
        /// Data.
        /// </summary>
        TData Data { get; }

        /// <summary>
        /// Metadata associated with <see cref="Data"/>.
        /// </summary>
        TMetadata Metadata { get; }
    }

    public class DataContainer<TData, TMetadata> : IDataContainer<TData, TMetadata>
    {
        /// <inheritdoc />
        public TData Data { get; }

        /// <inheritdoc />
        public TMetadata Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContainer{TData, TMetadata}"/> class.
        /// </summary>
        /// <param name="data">Not null data.</param>
        /// <param name="metadata">Metadata.</param>
        public DataContainer([DisallowNull] TData data, TMetadata metadata = default)
        {
            Data = data.AssertArgumentNotNull(nameof(data));
            Metadata = metadata;
        }
    }
}
