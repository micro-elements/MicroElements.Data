// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Diagnostics
{
    using System;

    /// <summary>
    /// Diagnostic message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Date and time of message created.
        /// </summary>
        public DateTime CreatedTime { get; }

        /// <summary>
        /// Message severity.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// Event name.
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// Message text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Exception associated with error message.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="severity">Message severity.</param>
        /// <param name="eventName">Event name.</param>
        /// <param name="text">Message text.</param>
        /// <param name="exception">Exception associated with error message.</param>
        /// <param name="createdTime">Date and time of message created.</param>
        public Message(
            Severity severity,
            string eventName,
            string text,
            Exception exception = null,
            DateTime? createdTime = null)
        {
            CreatedTime = createdTime ?? DateTime.Now;

            Severity = severity;
            EventName = eventName ?? string.Empty;
            Text = text ?? string.Empty;
            Exception = exception;
        }
    }

    /// <summary>
    /// Message severity.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Information message.
        /// </summary>
        Information,

        /// <summary>
        /// Warning.
        /// </summary>
        Warning,

        /// <summary>
        /// Error message.
        /// </summary>
        Error,
    }
}
