using System.Text;

namespace MicroElements.Data
{
    public class DataFormat
    {
        public string Name { get; set; }
        public IDataFilter DataFilter { get; set; }
    }

    public interface IDataFilter
    {
        bool Matches(MessageData data);
    }

    public interface IDataHandler
    {
        MessageData Process(MessageData data);
    }

    public interface ISerializer
    {
    }



    public class DataObject
    {
    }

    //ParseResult
    //MessageList
    //ValidationResult

    public class Session
    {
        public DataContainer[] Data;

    }

    public interface IFormatRegistry
    {
        void RegisterFormat(DataFormat dataFormat, IDataHandler dataHandler);
        void RegisterFormatConverter(string sourceFormat, string targetFormat, IDataHandler dataHandler);
        void RegisterFormatParser(string dataFormat, IDataHandler dataHandler);
    }

    public class DataFormatConverter
    {
        public string SourceFormat { get; set; }
        public string TargetFormat { get; set; }
        public IDataHandler Converter { get; set; }
    }
}
