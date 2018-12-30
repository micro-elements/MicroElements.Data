namespace MicroElements.Data
{
    /// <summary>
    /// Data container consist of common attributes, content, headers and properties.
    /// <para>It can be file, http, message or database content.</para>
    /// </summary>
    public interface IDataContainer
    {
        /// <summary>
        /// Common data attributes.
        /// </summary>
        IDataAttributes Attributes { get; }

        /// <summary>
        /// Data content.
        /// </summary>
        IDataContent Content { get; }

        /// <summary>
        /// Data headers.
        /// </summary>
        IHeaders Headers { get; }

        /// <summary>
        /// Properties.
        /// </summary>
        IProperties Properties { get; }
    }
}
