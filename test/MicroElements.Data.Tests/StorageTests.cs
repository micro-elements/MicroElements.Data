using System;
using System.IO;
using System.Text;
using FluentAssertions;
using MicroElements.Data.Filters;
using Xunit;

namespace MicroElements.Data.Tests
{
    public class StorageTests
    {
        [Fact]
        public void StorageModelMapping()
        {
            DataFormat dataFormat = new DataFormat(
                new FormatName("SimpleTextFormat"),
                Filter.Empty,
                new FormatParser(data => new ParseResult($"{data.Content.GetContentText()} parse result", null)));

            var dataContainer = DataContainer.FromText("sample content");
            dataContainer = dataContainer.WithFormat(dataFormat);

            IParseResult parseResult = dataFormat.Parser.Parse(dataContainer);

            parseResult.Should().NotBeNull();
            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Data.Should().NotBeNull();

            parseResult.Data.As<string>().Should().Be("sample content parse result");

            MemoryStream memoryStream = new MemoryStream();
            new XmlDataContainerWriter().Write(dataContainer, memoryStream, Encoding.UTF8);

            string text = Encoding.UTF8.GetString(memoryStream.ToArray());

            memoryStream.Position = 0;
            IDataContainer container = new XmlDataContainerReader().Read(memoryStream, Encoding.UTF8);

            Console.WriteLine(text);
            //IFormatRegistry formatRegistry;
            //formatRegistry.RegisterFormat(dataFormat, null);
        }
    }
}
