using System;

namespace MicroElements.Data
{
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
}
