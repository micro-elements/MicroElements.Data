// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System;

    public interface IDataAttrubutes
    {
        /// <summary>
        /// The date and time of message created.
        /// </summary>
        DateTime DateCreated { get; }

        /// <summary>
        /// Message identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Data format.
        /// </summary>
        string Format { get; }
    }

    /// <summary>
    /// Abstraction of message from different sources: File, Http, MessageQueue.
    /// </summary>
    public class MessageData : IDataAttrubutes
    {
        private readonly MessageData _parentData;

        /// <summary>
        /// The date and time of message created.
        /// </summary>
        public DateTime DateCreated { get; } = DateTime.Now;

        /// <summary>
        /// Message identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Data format.
        /// </summary>
        public string Format { get; }

        /// <summary>
        /// Message content.
        /// </summary>
        public IDataContent Content { get; }

        public MessageData(string format, IDataContent content)
        {
            Format = format;
            Content = content;
        }
    }
}
