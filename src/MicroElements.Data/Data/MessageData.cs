// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System;

    /// <summary>
    /// Abstraction of message from different sources: File, Http, MessageQueue.
    /// </summary>
    public class MessageData
    {
        private DataContainer _dataContainer;
        private IMessageAttributes _messageAttributes;
    }
}
