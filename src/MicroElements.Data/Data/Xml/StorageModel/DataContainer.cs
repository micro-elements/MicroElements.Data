// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data.Xml.StorageModel
{
    using System;
    using System.Xml.Serialization;
    using MicroElements.Design.Annotations;

    [Model(Convention = ModelConvention.StorageModel)]
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
        [XmlArrayItem(ElementName = "Header")]
        public HeaderStorageModel[] Headers { get; set; }

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

    public class HeadersStorageModel
    {
        public HeaderStorageModel[] Headers { get; set; }
    }

    public class HeaderStorageModel
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }

    internal static class ConvertUtils
    {
        public static DateTime TrimToSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}
