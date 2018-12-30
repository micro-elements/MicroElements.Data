namespace MicroElements.Data
{
    using MicroElements.Data.Content;
    using MicroElements.Design.Annotations;

    /// <summary>
    /// Data container consist of common attributes, content, headers and properties.
    /// <para>It can be file, http, message or database content.</para>
    /// </summary>
    [Model(Convention = ModelConvention.DomainModel)]
    public class DataContainer : IDataContainer
    {
        /// <summary>
        /// Common data attributes.
        /// </summary>
        public IDataAttributes Attributes { get; }

        /// <summary>
        /// Data content.
        /// </summary>
        public IDataContent Content { get; }

        /// <summary>
        /// Data headers.
        /// </summary>
        public IHeaders Headers { get; }

        /// <summary>
        /// Properties.
        /// </summary>
        public IProperties Properties { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContainer"/> class.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="content"></param>
        /// <param name="headers"></param>
        /// <param name="properties"></param>
        public DataContainer(
            IDataAttributes attributes,
            IDataContent content,
            IHeaders headers,
            IProperties properties)
        {
            Attributes = attributes;
            Content = content;
            Headers = headers;
            Properties = properties;
        }

        public static IDataContainer FromText(string textContent)
        {
            return new DataContainer(
                new DataAttributes(),
                new TextContent(textContent),
                new Headers(),
                new Properties());
        }
    }
}
