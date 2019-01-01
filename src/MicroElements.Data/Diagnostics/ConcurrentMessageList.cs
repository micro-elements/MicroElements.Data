// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Diagnostics
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// Simple concurrent message list.
    /// </summary>
    public class ConcurrentMessageList : IMessageList
    {
        private readonly ConcurrentQueue<Message> _messages = new ConcurrentQueue<Message>();

        /// <inheritdoc />
        public void Add(Message message) => _messages.Enqueue(message);

        /// <inheritdoc />
        public IEnumerator<Message> GetEnumerator() => _messages.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
