using System;
using MicroElements.Functional;

namespace MicroElements.Data.Caching
{
    public class CacheSettings<TValue> : ICacheSettings<TValue>
    {
        /// <inheritdoc />
        public string SectionName { get; set; }

        /// <inheritdoc />
        public Type ValueType => typeof(TValue);

        /// <inheritdoc />
        public string DataSource { get; set; } = "DataSource";

        /// <inheritdoc />
        public bool CacheErrorValue { get; set; } = false;

        /// <inheritdoc />
        public Func<TValue, Message>? Validate { get; set; }

        /// <inheritdoc/>
        public Func<Exception, Message>? HandleCreateError { get; set; }
    }
}
