// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Provides methods to read <see cref="IDataContainer"/> from <see cref="Stream"/>.
    /// </summary>
    /// todo: IDataContainerReader: ReadAsync, Result<IDataContainer>
    public interface IDataContainerReader
    {
        /// <summary>
        /// Reads <see cref="IDataContainer"/> from <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Source stream.</param>
        /// <param name="encoding">Encoding.</param>
        /// <returns><see cref="IDataContainer"/>.</returns>
        IDataContainer Read(Stream stream, Encoding encoding);
    }
}
