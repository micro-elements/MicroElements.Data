namespace MicroElements.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Headers are key-value collection.
    /// <para>Abstracts http headers, messaging headers.</para>
    /// <para>Should be used for transport or storage layer properties.</para>
    /// <para>For some types of data can be presented as typed attributes. For example: <see cref="IMessageAttributes"/></para>
    /// </summary>
    public interface IHeaders
    {
        //void Add(string name, string value);
        //string Get(string name, string defaultValue);

        IReadOnlyDictionary<string, string> GetAll();
    }

    public class Headers : IHeaders
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> GetAll()
        {
            return new Dictionary<string, string>();
        }
    }
}
