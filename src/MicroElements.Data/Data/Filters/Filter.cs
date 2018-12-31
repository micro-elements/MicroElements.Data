namespace MicroElements.Data.Filters
{
    using MicroElements.CodeContracts;

    /// <summary>
    /// Generic filter that forward execution to external delegate.
    /// </summary>
    public class Filter : IDataFilter
    {
        /// <summary>
        /// Empty filter. Matches any data.
        /// </summary>
        public static readonly IDataFilter Empty = new Filter(data => true);

        private readonly FilterDelegate _filterDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="filterDelegate">Filter delegate.</param>
        public Filter(FilterDelegate filterDelegate)
        {
            Requires.NotNull(filterDelegate, nameof(filterDelegate));

            _filterDelegate = filterDelegate;
        }

        /// <inheritdoc />
        public bool Matches(IDataContainer data)
        {
            return _filterDelegate(data);
        }
    }
}
