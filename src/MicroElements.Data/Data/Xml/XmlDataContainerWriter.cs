namespace MicroElements.Data.Xml
{
    using System.IO;
    using System.Text;
    using System.Xml;

    public class XmlDataContainerWriter : IDataContainerWriter
    {
        /// <inheritdoc />
        public void Write(IDataContainer dataContainer, Stream stream, Encoding encoding)
        {
            var storageModel = StorageModelMapper.Instance.ToStorageModel(dataContainer);

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
}
