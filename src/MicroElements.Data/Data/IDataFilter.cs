namespace MicroElements.Data
{
    /// <summary>
    /// Data filter.
    /// </summary>
    public interface IDataFilter
    {
        /// <summary>
        /// Returns true if data matches format.
        /// </summary>
        /// <param name="data">Data to check.</param>
        /// <returns><c>true</c> if data matches format.</returns>
        bool Matches(IDataContainer data);
    }

    /// <summary>
    /// Delegate for data filter.
    /// </summary>
    /// <param name="data">Data to check.</param>
    /// <returns><c>true</c> if data matches format.</returns>
    public delegate bool FilterDelegate(IDataContainer data);
}
