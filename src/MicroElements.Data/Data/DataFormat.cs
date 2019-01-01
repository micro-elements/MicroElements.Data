namespace MicroElements.Data
{
    /// <summary>
    /// Data format.
    /// </summary>
    public class DataFormat
    {
        /// <summary>
        /// The format name.
        /// </summary>
        public FormatName Name { get; }

        /// <summary>
        /// Filter that checks data matches format.
        /// </summary>
        public IDataFilter DataFilter { get; }

        public IFormatValidator FormatValidator { get; }

        /// <summary>
        /// Parser that can parse format.
        /// </summary>
        public IFormatParser Parser { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFormat"/> class.
        /// </summary>
        /// <param name="name">The format name.</param>
        /// <param name="dataFilter">Filter that checks data matches format.</param>
        /// <param name="parser">Format parser.</param>
        public DataFormat(FormatName name, IDataFilter dataFilter, IFormatParser parser)
        {
            Name = name;
            DataFilter = dataFilter;
            Parser = parser;
        }
    }
}
