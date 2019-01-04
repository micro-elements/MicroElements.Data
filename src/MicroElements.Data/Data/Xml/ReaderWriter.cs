// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data.Xml
{
    using System;
    using System.Xml.Serialization;
    using MicroElements.Data.Xml.StorageModel;

    public static class XmlDataContainerSerializer
    {
        private static readonly Lazy<XmlSerializer> LazyXmlSerializer = new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(DataContainerStorageModel)));

        public static XmlSerializer Instance => LazyXmlSerializer.Value;
    }
}
