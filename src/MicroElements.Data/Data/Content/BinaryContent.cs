namespace MicroElements.Data.Content
{
    using System.IO;
    using System.Text;
    using JetBrains.Annotations;
    using MicroElements.CodeContracts;

    /// <summary>
    /// Represents binary content from file, stream or other sources.
    /// </summary>
    public class BinaryContent : IDataContent
    {
        private readonly byte[] _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryContent"/> class.
        /// </summary>
        /// <param name="content">Byte content.</param>
        /// <param name="contentEncoding">Content encoding for text extracting.</param>
        public BinaryContent([NotNull] byte[] content, Encoding contentEncoding = null)
        {
            Requires.NotNull(content, nameof(content));
            _content = CloneContent(content);
            ContentEncoding = contentEncoding ?? Encoding.UTF8;
        }

        /// <inheritdoc />
        public int ContentLength => _content.Length;

        /// <inheritdoc />
        public Encoding ContentEncoding { get; }

        /// <inheritdoc />
        public byte[] GetContentBytes() => CloneContent(_content);

        /// <inheritdoc />
        public string GetContentText()
        {
            return ContentLength > 0 ? new StreamReader(GetContentStream(), ContentEncoding).ReadToEnd() : string.Empty;
        }

        /// <inheritdoc />
        public Stream GetContentStream()
        {
            return new MemoryStream(_content, writable: false);
        }

        /// <summary>
        /// Protects content from modifying.
        /// </summary>
        private byte[] CloneContent(byte[] content) => (byte[])content.Clone();
    }
}
