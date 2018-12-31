using System;
using FluentAssertions;
using Xunit;

namespace MicroElements.Data.Tests
{
    public class DataTests
    {
        [Fact]
        public void FormatNameChecks()
        {
            Action createInvalidObject = () => { new FormatName(null); };
            createInvalidObject.Should().Throw<ArgumentNullException>();

            new FormatName("format1").Should().BeEquivalentTo(new FormatName("format1"));
            new FormatName("format1").Should().NotBe(new FormatName("format2"));
            new FormatName("format1").Should().BeLessOrEqualTo(new FormatName("format2"));
        }

        [Fact]
        public void CreateNewDataContainerFromText()
        {
            var dataContainer = DataContainer.FromText("sample content");
            dataContainer.Should().NotBeNull();
            dataContainer.Attributes.Should().NotBeNull();
            dataContainer.Content.Should().NotBeNull();
            dataContainer.Content.GetContentText().Should().Be("sample content");
            dataContainer.Headers.Should().NotBeNull();
            dataContainer.Properties.Should().NotBeNull();
        }


    }
}
