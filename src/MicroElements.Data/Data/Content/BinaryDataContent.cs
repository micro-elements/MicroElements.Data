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
}
