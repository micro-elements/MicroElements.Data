// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data.Content
{
    using System.IO;
    using System.Text;
    using JetBrains.Annotations;
    using MicroElements.CodeContracts;

    /// <summary>
    /// Represents text content.
    /// </summary>
    public class TextContent : IDataContent
    {
        private readonly string _text;
        private readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextContent"/> class.
        /// </summary>
        /// <param name="text">Content text.</param>
        /// <param name="encoding">Optional text encoding. By default <see cref="Encoding.UTF8"/> if not set.</param>
        public TextContent([NotNull] string text, Encoding encoding = null)
        {
            Requires.NotNull(text, nameof(text));
            _text = text;
            _encoding = encoding ?? Encoding.UTF8;
        }

        /// <inheritdoc />
        public int ContentLength => _encoding.GetByteCount(_text);

        /// <inheritdoc />
        public Encoding ContentEncoding => _encoding;

        /// <inheritdoc />
        public byte[] GetContentBytes() => _encoding.GetBytes(_text);

        /// <inheritdoc />
        public string GetContentText() => _text;

        /// <inheritdoc />
        public Stream GetContentStream() => new MemoryStream(_encoding.GetBytes(_text), writable: false);
    }
}
