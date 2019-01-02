using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using MicroElements.Data.StorageModel;

namespace MicroElements.Data
{
    using System.IO;
    using System.Text;

    public interface IDataContainerReader
    {
        IDataContainer Read(Stream stream, Encoding encoding);
    }

    public interface IDataContainerWriter
    {
        void Write(IDataContainer dataContainer, Stream stream, Encoding encoding);
    }

    public class XmlDataContainerWriter : IDataContainerWriter
    {
        public static Lazy<XmlSerializer> LazyXmlSerializer = new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(DataContainerStorageModel)));

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

            var xmlSerializer = LazyXmlSerializer.Value;
            xmlSerializer.Serialize(xmlWriter, storageModel);
        }
    }

    public class XmlDataContainerReader : IDataContainerReader
    {
        /// <inheritdoc />
        public IDataContainer Read(Stream stream, Encoding encoding)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                
            };
            XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings);

            var deserialize = (DataContainerStorageModel)XmlDataContainerWriter.LazyXmlSerializer.Value.Deserialize(xmlReader);

            return null;
        }
    }

    public static class XmlExtensions
    {
        public static XDocument ToXDocument(this IDataContainer dataContainer)
        {
            return new XDocument(
                new XElement("DataContainer",
                    new XElement("CreatedTime", dataContainer.Attributes.DateCreated)));
        }
    }

    public static class SerializationUtil
    {
        public static T Deserialize<T>(XDocument doc)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = doc.Root.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        public static XDocument Serialize<T>(T value)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            XDocument doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, value);
            }

            return doc;
        }
    }
}
