namespace MicroElements.Data
{
    using System;
    using JetBrains.Annotations;
    using MicroElements.CodeContracts;

    /// <summary>
    /// Format parser that delegates parse to external func.
    /// </summary>
    public class FormatParser : IFormatParser
    {
        private readonly Func<IDataContainer, IParseResult> _parseFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatParser"/> class.
        /// </summary>
        /// <param name="parseFunc">Parser func.</param>
        public FormatParser([NotNull] Func<IDataContainer, IParseResult> parseFunc)
        {
            Requires.NotNull(parseFunc, nameof(parseFunc));

            _parseFunc = parseFunc;
        }

        /// <inheritdoc />
        public IParseResult Parse(IDataContainer dataContainer)
        {
            return _parseFunc(dataContainer);
        }
    }
}
