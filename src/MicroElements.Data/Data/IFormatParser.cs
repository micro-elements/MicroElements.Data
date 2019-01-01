// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using MicroElements.Diagnostics;

    /// <summary>
    /// Format parser parses <see cref="IDataContainer"/> to <see cref="IParseResult"/>.
    /// </summary>
    public interface IFormatParser
    {
        /// <summary>
        /// Parse <see cref="IDataContainer"/>.
        /// </summary>
        /// <param name="dataContainer">Data to parse.</param>
        /// <returns>Parse result.</returns>
        [NotNull] IParseResult Parse([NotNull] IDataContainer dataContainer);
    }
}
