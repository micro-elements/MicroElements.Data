using System;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// Represents cache section description.
    /// </summary>
    public interface ICacheSectionInfo
    {
        /// <summary>
        /// Gets section name.
        /// </summary>
        string SectionName { get; }

        /// <summary>
        /// Gets value type that cache section holds.
        /// </summary>
        Type ValueType { get; }
    }
}
