using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using MicroElements.CodeContracts;
using MicroElements.Functional;

namespace MicroElements.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Headers are key-value collection.
    /// <para>Abstracts http headers, messaging headers.</para>
    /// <para>Should be used for transport or storage layer properties.</para>
    /// <para>For some types of data can be presented as typed attributes. For example: <see cref="IMessageAttributes"/></para>
    /// </summary>
    public interface IHeaders : IEnumerable<Header>
    {
    }

    public class Headers : IHeaders
    {
        public static readonly IHeaders Empty = new Headers();

        private readonly Dictionary<string, Header> _headersDic = new Dictionary<string, Header>(StringComparer.OrdinalIgnoreCase);

        private void AddHeaders(IEnumerable<Header> headers)
        {
            foreach (var header in headers)
            {
                _headersDic.Add(header.Name, header);
            }
        }

        public Headers()
        {
        }

        public Headers(IEnumerable<Header> headers)
        {
            AddHeaders(headers);
        }

        public Headers(IDictionary<string, string> headers)
        {
            AddHeaders(headers.Select(pair => new Header(pair.Key, pair.Value)));
        }

        /// <inheritdoc />
        public IEnumerator<Header> GetEnumerator()
        {
            return _headersDic.Values.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Header is string name-value pair.
    /// Name case is ignored.
    /// </summary>
    public class Header : ValueObject
    {
        /// <summary>
        /// Header name.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Header value.
        /// </summary>
        [NotNull]
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        public Header([NotNull] string name, [NotNull] string value)
        {
            Requires.NotNull(name, nameof(name));
            Requires.NotNull(value, nameof(value));

            Name = name;
            Value = value;
        }

        /// <inheritdoc />
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name.ToUpperInvariant();
            yield return Value;
        }
    }
}
