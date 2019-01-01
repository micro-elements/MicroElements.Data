namespace MicroElements.Diagnostics
{
    using System.Collections.Generic;

    /// <summary>
    /// Message list.
    /// </summary>
    public interface IMessageList : IEnumerable<Message>
    {
        /// <summary>
        /// Adds message to list.
        /// </summary>
        /// <param name="message">Message.</param>
        void Add(Message message);
    }
}
