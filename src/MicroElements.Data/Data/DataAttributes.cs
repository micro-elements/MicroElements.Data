// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System;

    /// <summary>
    /// Common data attributes.
    /// </summary>
    public class DataAttributes : IDataAttributes
    {
        /// <inheritdoc />
        public DateTime DateCreated { get; }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public FormatName FormatName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAttributes"/> class.
        /// </summary>
        /// <param name="dateCreated">The date and time of data created.</param>
        /// <param name="id">Data identifier.</param>
        /// <param name="formatName">Data format.</param>
        public DataAttributes(string id = null, string formatName = null, DateTime? dateCreated = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
            FormatName = formatName != null ? new FormatName(formatName) : FormatName.Undefined;
            DateCreated = dateCreated ?? DateTime.Now;
        }
    }
}
