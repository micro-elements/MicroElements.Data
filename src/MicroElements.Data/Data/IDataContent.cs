using System.IO;
using System.Text;

namespace MicroElements.Data
{
    /// <summary>
    /// Represents content of <see cref="MessageData"/>.
    /// <para>Provides methods for content retrieving.</para>
    /// </summary>
    public interface IDataContent
    {
        /// <summary>
        /// Gets content length in bytes.
        /// </summary>
        int ContentLength { get; }

        /// <summary>
        /// Gets content as bytes.
        /// </summary>
        /// <returns>byte array.</returns>
        byte[] GetContentBytes();

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
