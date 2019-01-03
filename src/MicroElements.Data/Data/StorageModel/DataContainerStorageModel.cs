namespace MicroElements.Data.StorageModel
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "DataContainer")]
    public class DataContainerStorageModel
    {
        /// <summary>
        /// Common data attributes.
        /// </summary>
        public DataAttributesStorageModel Attributes { get; set; }

        /// <summary>
        /// Data content.
        /// </summary>
        public DataContentStorageModel Content { get; set; }

        /// <summary>
        /// Data headers.
        /// </summary>
        IHeaders Headers { get; set; }

        /// <summary>
        /// Properties.
        /// </summary>
        IProperties Properties { get; set; }
    }

    /// <summary>
    /// StorageModel for <see cref="IDataAttributes"/>.
    /// </summary>
    public class DataAttributesStorageModel
    {
        public DateTime DateCreated { get; set; }
        public string Id { get; set; }
        public string FormatName { get; set; }
    }

    /// <summary>
    /// StorageModel for <see cref="IDataContent"/>.
    /// </summary>
    public class DataContentStorageModel
    {
        /// <summary>
        /// Encoding.
        /// </summary>
        public string Encoding { get; set; } = "utf-8";

        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; set; }
    }

    internal static class ConvertUtils
    {
        public static DateTime TrimToSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}
