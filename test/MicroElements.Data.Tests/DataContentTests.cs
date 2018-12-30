using System;
using System.IO;
using System.Text;
using FluentAssertions;
using MicroElements.Data.Content;
using Xunit;

namespace MicroElements.Data.Tests
{
    public class DataContentTests
    {
        [Fact]
        public void TextContent_BorderCases()
        {
            AssertionExtensions.Should(() => new TextContent(null)).Throw<ArgumentNullException>();
            AssertionExtensions.Should(() => new TextContent("")).NotThrow();
            new TextContent("text").ContentEncoding.Should().Be(Encoding.UTF8);
            new TextContent("text", Encoding.Unicode).ContentEncoding.Should().Be(Encoding.Unicode);
        }

        [Fact]
        public void TextContentTest()
        {
            string text = "sample content";
            var content = new TextContent(text);
            content.ContentLength.Should().Be(text.Length);
            content.ContentEncoding.Should().Be(Encoding.UTF8);
            content.GetContentText().Should().Be(text);
            content.GetContentBytes().Should().BeEquivalentTo(Encoding.UTF8.GetBytes(text));

            content.GetContentStream().Should().NotBeNull();
            content.GetContentStream().Should().NotBeSameAs(content.GetContentStream());
            content.GetContentStream().CanRead.Should().BeTrue();
            content.GetContentStream().CanWrite.Should().BeFalse();
            new StreamReader(content.GetContentStream()).ReadToEnd().Should().Be(text);
        }

        [Fact]
        public void BinaryContent_BorderCases()
        {
            AssertionExtensions.Should(() => new BinaryContent(null)).Throw<ArgumentNullException>();
            AssertionExtensions.Should(() => new BinaryContent(new byte[0])).NotThrow();
            new BinaryContent(new byte[0]).ContentEncoding.Should().Be(Encoding.UTF8);
            new BinaryContent(new byte[0], Encoding.Unicode).ContentEncoding.Should().Be(Encoding.Unicode);
        }

        [Fact]
        public void BinaryContentTest()
        {
            string text = "sample content";
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            var content = new BinaryContent(bytes);
            content.ContentLength.Should().Be(text.Length);
            content.ContentEncoding.Should().Be(Encoding.UTF8);
            content.GetContentText().Should().Be(text);
            content.GetContentBytes().Should().BeEquivalentTo(bytes);

            content.GetContentStream().Should().NotBeNull();
            content.GetContentStream().Should().NotBeSameAs(content.GetContentStream());
            content.GetContentStream().CanRead.Should().BeTrue();
            content.GetContentStream().CanWrite.Should().BeFalse();
            new StreamReader(content.GetContentStream()).ReadToEnd().Should().Be(text);
        }

        [Fact]
        public void BinaryContentWithEncoding()
        {
            string text = "Русский текст";
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            var content = new BinaryContent(bytes, Encoding.UTF8);
            content.GetContentText().Should().NotBe(text);
        }
    }
}
