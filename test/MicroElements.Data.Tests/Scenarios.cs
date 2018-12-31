using FluentAssertions;
using MicroElements.Data.Filters;
using Xunit;

namespace MicroElements.Data.Tests
{
    public class Scenarios
    {
        [Fact]
        public void DataContainerScenario()
        {
            var dataContainer = DataContainer.FromText("sample content");

            DataFormat dataFormat = new DataFormat(new FormatName("SimpleTextFormat"), Filter.Empty);

            dataFormat.DataFilter.Matches(dataContainer).Should().BeTrue();

            //dataFormat.Parser.Parse(dataContainer);

            //IFormatRegistry formatRegistry;
            //formatRegistry.RegisterFormat(dataFormat, null);
        }
    }
}
