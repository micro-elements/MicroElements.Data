namespace MicroElements.Data
{
    using System.IO;

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
}
