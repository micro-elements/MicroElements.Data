// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using MicroElements.Data.StorageModel;

    public interface IDataContainerReader
    {
        IDataContainer Read(Stream stream, Encoding encoding);
    }

    public interface IDataContainerWriter
    {
        void Write(IDataContainer dataContainer, Stream stream, Encoding encoding);
    }

    public static class XmlDataContainerSerializer
    {
        private static readonly Lazy<XmlSerializer> LazyXmlSerializer = new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(DataContainerStorageModel)));

        public static XmlSerializer Instance => LazyXmlSerializer.Value;
    }

    public class XmlDataContainerWriter : IDataContainerWriter
    {
        /// <inheritdoc />
        public void Write(IDataContainer dataContainer, Stream stream, Encoding encoding)
        {
            var storageModel = StorageModelMapper.Instance.ToStorageMode(dataContainer);

            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = " ",
                Encoding = encoding,
                CloseOutput = true,
            };

            XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings);

            var xmlSerializer = XmlDataContainerSerializer.Instance;
            xmlSerializer.Serialize(xmlWriter, storageModel);
        }
    }

    public class XmlDataContainerReader : IDataContainerReader
    {
        /// <inheritdoc />
        public IDataContainer Read(Stream stream, Encoding encoding)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { };
            XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings);

            var xmlSerializer = XmlDataContainerSerializer.Instance;
            var containerStorageModel = (DataContainerStorageModel)xmlSerializer.Deserialize(xmlReader);

            IDataContainer dataContainer = StorageModelMapper.Instance.FromStorageMode(containerStorageModel);

            return dataContainer;
        }
    }
}
