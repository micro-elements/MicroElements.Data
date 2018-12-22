namespace MicroElements.Data
{
    using System;
    using System.Collections.Generic;

    public class DataContainer : IDataAttrubutes
    {
        /// <inheritdoc />
        public DateTime DateCreated { get; set; }

        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Format { get; set; }

        public string Source;
        public string CorrelationId;

        /// <summary>
        /// Message content.
        /// </summary>
        public IDataContent Content { get; }

        /// <summary>
        /// Gets data
        /// </summary>
        public IHeaders Headers;
    }

    public interface IMessageAttributes
    {
        Uri SourceAddress { get; set; }
        Uri DestinationAddress { get; set; }
        Uri ResponseAddress { get; set; }
        Uri FaultAddress { get; set; }

        Guid? RequestId { get; set; }
        Guid? MessageId { get; set; }
        Guid? CorrelationId { get; set; }

        DateTime DateCreated { get; set; }
        TimeSpan? TimeToLive { get; set; }

    }

    public interface IHeaders : IDictionary<string, string>
    {
        //see https://github.com/MassTransit/MassTransit/blob/develop/src/MassTransit/Headers.cs
        IEnumerable<KeyValuePair<string, object>> GetAll();
    }

    public class Properties
    {

    }

    public class Property
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
    }

    public class PropertyStorageModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
