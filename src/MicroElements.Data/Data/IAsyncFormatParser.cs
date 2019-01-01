namespace MicroElements.Data
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    /// Async version of <see cref="IFormatParser"/>.
    /// </summary>
    public interface IAsyncFormatParser
    {
        /// <summary>
        /// Parse <see cref="IDataContainer"/>.
        /// </summary>
        /// <param name="dataContainer">Data to parse.</param>
        /// <returns>Parse result.</returns>
        [ItemNotNull] Task<IParseResult> ParseAsync([NotNull] IDataContainer dataContainer);
    }
}
