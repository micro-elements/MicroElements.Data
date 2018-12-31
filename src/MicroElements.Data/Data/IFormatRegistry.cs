namespace MicroElements.Data
{
    public interface IFormatRegistry
    {
        void RegisterFormat(DataFormat dataFormat, IDataHandler dataHandler);
        void RegisterFormatConverter(string sourceFormat, string targetFormat, IDataHandler dataHandler);
        void RegisterFormatParser(string dataFormat, IDataHandler dataHandler);
    }
}
