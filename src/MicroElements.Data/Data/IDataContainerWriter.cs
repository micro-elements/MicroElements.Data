// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System.IO;
    using System.Text;

    public interface IDataContainerWriter
    {
        void Write(IDataContainer dataContainer, Stream stream, Encoding encoding);
    }
}
