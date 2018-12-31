// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Format parser parses <see cref="IDataContainer"/> to <see cref="ParseResult"/>.
    /// </summary>
    public interface IFormatParser
    {
        ParseResult Parse(DataContainer dataContainer);
    }

    public class ParseResult
    {
        public object Data;
        public List<DiagnosticMessage> Messages;
    }

    public class DiagnosticMessage
    {
        public DateTime CreatedTime { get; }
        public string Message { get; }
        public Severity Severity { get; }
    }

    public enum Severity
    {
        Info,
        Warning,
        Error
    }
}
