namespace MicroElements.Data
{
    //ParseResult
    //MessageList
    //ValidationResult

    public class Session
    {
        public DataContainer[] Data;

    }

    public class DataFormatConverter
    {
        public string SourceFormat { get; set; }
        public string TargetFormat { get; set; }
        public IDataHandler Converter { get; set; }
    }
}
