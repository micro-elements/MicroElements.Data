// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents content of <see cref="DataContainer"/>.
    /// <para>Provides methods for content retrieving.</para>
    /// </summary>
    public interface IDataContent
    {
        /// <summary>
        /// Gets content length in bytes.
        /// </summary>
        int ContentLength { get; }

        /// <summary>
        /// Gets content encoding.
        /// </summary>
        Encoding ContentEncoding { get; }

        /// <summary>
        /// Gets content as bytes.
        /// </summary>
        /// <returns>byte array.</returns>
        byte[] GetContentBytes();
        //TODO: ReadOnlySpan<byte> GetContentBytes();//net core app target

        /// <summary>
        /// Gets content as text.
        /// </summary>
        /// <returns><see cref="string"/>.</returns>
        string GetContentText();

        /// <summary>
        /// Gets content stream.
        /// </summary>
        /// <returns><see cref="Stream"/>.</returns>
        Stream GetContentStream();
    }
}
