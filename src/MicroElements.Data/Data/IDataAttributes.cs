namespace MicroElements.Data
{
    using System;

    /// <summary>
    /// Represents common data attributes.
    /// </summary>
    public interface IDataAttributes
    {
        /// <summary>
        /// The date and time of data created.
        /// </summary>
        DateTime DateCreated { get; }

        /// <summary>
        /// Data identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Data format.
        /// </summary>
        FormatName FormatName { get; }
    }
}
