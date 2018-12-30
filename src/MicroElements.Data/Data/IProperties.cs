namespace MicroElements.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents data typed properties.
    /// </summary>
    public interface IProperties
    {
        IProperty GetByName(string name);
        IProperty GetByType(Type type);
        IReadOnlyDictionary<string, IProperty> GetAll();
    }

    public class Properties : IProperties
    {
        /// <inheritdoc />
        public IProperty GetByName(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IProperty GetByType(Type type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IProperty> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
