using System;
using System.IO;
using System.Text;
using FluentAssertions;
using MicroElements.Data.Filters;
using MicroElements.Data.Xml;
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

            var dataContainer = DataContainer
                .FromText("sample content")
                .WithFormat(dataFormat)
                .WithHeader("ContentType", "text");

            MemoryStream memoryStream = new MemoryStream();
            new XmlDataContainerWriter().Write(dataContainer, memoryStream, Encoding.UTF8);

            string text = Encoding.UTF8.GetString(memoryStream.ToArray());

            memoryStream.Position = 0;
            IDataContainer container = new XmlDataContainerReader().Read(memoryStream, Encoding.UTF8);

            dataContainer.Should().NotBeSameAs(container);

            memoryStream.Position = 0;
            new XmlDataContainerWriter().Write(container, memoryStream, Encoding.UTF8);
            string text2 = Encoding.UTF8.GetString(memoryStream.ToArray());

            text2.Should().Be(text, "objects after serialization and deserialization should be the same");

            /*
﻿<?xml version="1.0" encoding="utf-8"?>
<DataContainer xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
 <Attributes>
  <DateCreated>2019-01-04T00:22:35</DateCreated>
  <Id>df2e6536-fcb1-4c1c-a542-71c7cfcd765d</Id>
  <FormatName>SimpleTextFormat</FormatName>
 </Attributes>
 <Content>
  <Encoding>utf-8</Encoding>
  <Text>sample content</Text>
 </Content>
</DataContainer>
             */

        }
    }
}
