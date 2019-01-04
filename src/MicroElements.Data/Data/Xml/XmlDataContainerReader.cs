namespace MicroElements.Data.Xml
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using MicroElements.Data.Xml.StorageModel;

    public class XmlDataContainerReader : IDataContainerReader
    {
        /// <inheritdoc />
        public IDataContainer Read(Stream stream, Encoding encoding)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { };
            XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings);

            var xmlSerializer = XmlDataContainerSerializer.Instance;
            var containerStorageModel = (DataContainerStorageModel)xmlSerializer.Deserialize(xmlReader);

            IDataContainer dataContainer = StorageModelMapper.Instance.FromStorageModel(containerStorageModel);

            return dataContainer;
        }
    }
}
