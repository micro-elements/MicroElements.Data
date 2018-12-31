namespace MicroElements.Data
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using MicroElements.CodeContracts;
    using MicroElements.Design.Annotations;
    using MicroElements.Functional;

    /// <summary>
    /// Format name.
    /// </summary>
    [Model(Convention = ModelConvention.ValueObject)]
    public class FormatName : ValueObject, IEquatableObject
    {
        /// <summary>
        /// Represents undefined format.
        /// </summary>
        public static readonly FormatName Undefined = new FormatName(string.Empty);

        /// <summary>
        /// Gets the format name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatName"/> class.
        /// </summary>
        /// <param name="name">The format name.</param>
        public FormatName([NotNull] string name)
        {
            Requires.NotNull(name, nameof(name));
            Name = name;
        }

        /// <inheritdoc />
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
