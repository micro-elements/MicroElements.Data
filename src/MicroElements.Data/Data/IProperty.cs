using System;

namespace MicroElements.Data
{
    public interface IProperty
    {
        string Name { get; }
        Type Type { get; }
        object Value { get; }
    }


    public class Property : IProperty
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
    }

    public class PropertyStorageModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
