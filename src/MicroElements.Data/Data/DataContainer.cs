// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using JetBrains.Annotations;
    using MicroElements.CodeContracts;
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
        /// <param name="attributes">Data attributes.</param>
        /// <param name="content">Data content.</param>
        /// <param name="headers">Headers.</param>
        /// <param name="properties">Properties.</param>
        public DataContainer(
            [NotNull] IDataAttributes attributes,
            [NotNull] IDataContent content,
            [NotNull] IHeaders headers,
            [NotNull] IProperties properties)
        {
            Requires.NotNull(attributes, nameof(attributes));
            Requires.NotNull(content, nameof(content));
            Requires.NotNull(headers, nameof(headers));
            Requires.NotNull(properties, nameof(properties));

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
