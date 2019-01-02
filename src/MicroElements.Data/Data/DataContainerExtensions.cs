using System;

namespace MicroElements.Data
{
    using MicroElements.Data.Content;

    public static class DataContainerExtensions
    {
        public static IDataContainer ToDataContainer(this string textContent)
        {
            return new DataContainer(
                new DataAttributes(),
                new TextContent(textContent),
                new Headers(),
                new Properties());
        }

        public static IDataContainer WithFormat(this IDataContainer dataContainer, DataFormat dataFormat)
        {
            if (!dataFormat.DataFilter.Matches(dataContainer))
                throw new InvalidOperationException($"Data does not matches format {dataFormat.Name}");
            return new DataContainer(
                new DataAttributes(dataContainer.Attributes.Id, dataFormat.Name, DateTime.Now),
                dataContainer.Content,
                dataContainer.Headers,
                dataContainer.Properties);
        }
    }
}
