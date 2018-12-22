using JetBrains.Annotations;

namespace MicroElements.Data.Content
{
    using System.IO;
    using System.Text;

    public class BinaryDataContent : IDataContent
    {
        public byte[] Content { get; }
        public Encoding ContentEncoding { get; }
        public string ContentType { get; } = "text/xml";

        public BinaryDataContent(byte[] content, Encoding contentEncoding = null)
        {
            Content = content;
            ContentEncoding = contentEncoding ?? Encoding.UTF8;
        }

        /// <inheritdoc />
        public int ContentLength => Content.Length;

        /// <inheritdoc />
        public byte[] GetContentBytes() => Content;

        public string GetContentText()
        {
            return ContentLength > 0 ? new StreamReader(new MemoryStream(Content), ContentEncoding).ReadToEnd() : string.Empty;
        }

        public Stream GetContentStream()
        {
            return new MemoryStream(Content);
        }
    }

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
            _text = text;
            _encoding = encoding ?? Encoding.UTF8;
        }

        /// <inheritdoc />
        public int ContentLength => _encoding.GetByteCount(_text);

        /// <inheritdoc />
        public byte[] GetContentBytes() => _encoding.GetBytes(_text);

        /// <inheritdoc />
        public string GetContentText() => _text;

        /// <inheritdoc />
        public Stream GetContentStream() => new MemoryStream(_encoding.GetBytes(_text));
    }
}
