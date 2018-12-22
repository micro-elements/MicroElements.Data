namespace MicroElements.Data
{
    /// <summary>
    /// This is JMS message headers.
    /// </summary>
    public class JmsHeaders
    {
        /// <summary>
        /// The unique message ID. Note that this is not a required field and can be null.
        /// </summary>
        public string JMSMessageID;

        /// <summary>
        /// This header is set by the application for use by other applications.
        /// </summary>
        public string JMSCorrelationID;

        /// <summary>
        /// This header is set by the JMS provider and denotes the delivery mode.
        /// </summary>
        public int JMSDeliveryMode;

        /// <summary>
        ///  The priority of the message.
        /// </summary>
        public int JMSPriority;

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        public long JMSTimestamp;

        /// <summary>
        /// A value of zero means that the message does not expire.
        /// Any other value denotes the expiration time for when the message is removed from the queue.
        /// </summary>
        public long JMSExpiration;

        /// <summary>
        /// The type of message.
        /// </summary>
        public string JMSType;

        /// <summary>
        /// The queue/topic the sender expects replies to.
        /// When receiving a message this value holds the provider specific Destination interface object and is typically an internal Queue or Topic object.
        /// </summary>
        public string JMSReplyTo;
    }
}