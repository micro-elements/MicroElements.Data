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

            DataFormat dataFormat = new DataFormat(
                new FormatName("SimpleTextFormat"),
                Filter.Empty,
                new FormatParser(data => new ParseResult($"{data.Content.GetContentText()} parse result", null)));

            dataFormat.DataFilter.Matches(dataContainer).Should().BeTrue();

            IParseResult parseResult = dataFormat.Parser.Parse(dataContainer);

            parseResult.Should().NotBeNull();
            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Data.Should().NotBeNull();

            parseResult.Data.As<string>().Should().Be("sample content parse result");

        }
    }
}
