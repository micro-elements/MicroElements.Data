namespace MicroElements.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using MicroElements.Diagnostics;

    /// <summary>
    /// Parse result. Consists of data and diagnostics messages.
    /// </summary>
    public interface IParseResult
    {
        /// <summary>
        /// Returns <c>true</c> if parse was successful.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets parse result.
        /// </summary>
        object Data { get; }

        /// <summary>
        /// Gets diagnostics messages.
        /// </summary>
        IReadOnlyList<Message> Messages { get; }
    }

    /// <summary>
    /// Strong typed parse result.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    public interface IParseResult<T> : IParseResult
    {
        /// <summary>
        /// Parse result.
        /// </summary>
        new T Data { get; }
    }

    /// <summary>
    /// ParseResult.
    /// </summary>
    public class ParseResult : IParseResult
    {
        /// <inheritdoc />
        public bool IsSuccess { get; }

        /// <inheritdoc />
        public object Data { get; }

        /// <inheritdoc />
        public IReadOnlyList<Message> Messages { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseResult"/> class.
        /// </summary>
        /// <param name="data">Parse result. Can be null for error result.</param>
        /// <param name="messages">Messages list.</param>
        public ParseResult([CanBeNull] object data, IReadOnlyList<Message> messages)
        {
            Data = data;
            Messages = messages ?? Array.Empty<Message>();
            IsSuccess = Messages.All(message => message.Severity != Severity.Error);
            if (data == null && IsSuccess)
            {
                throw new ArgumentException("Parse result should be not null if no errors provided.");
            }
        }
    }

    /// <summary>
    /// Strong typed parse result.
    /// </summary>
    /// <typeparam name="T">Parse result type.</typeparam>
    public class ParseResult<T> : IParseResult<T>
    {
        /// <inheritdoc />
        public bool IsSuccess => Messages.Any(message => message.Severity < Severity.Error);

        /// <inheritdoc />
        public T Data { get; }

        /// <inheritdoc />
        object IParseResult.Data => Data;

        /// <inheritdoc />
        public IReadOnlyList<Message> Messages { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseResult{T}"/> class.
        /// </summary>
        /// <param name="data">Parse result. Can be null for error result.</param>
        /// <param name="messages">Messages list.</param>
        public ParseResult([CanBeNull] T data, IReadOnlyList<Message> messages)
        {
            Data = data;
            Messages = messages ?? Array.Empty<Message>();
            if (data == null && IsSuccess)
            {
                throw new ArgumentException("Parse result should be not null if no errors provided.");
            }
        }
    }
}
