using JetBrains.Annotations;

namespace CsvPortable.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    [UsedImplicitly]
    public class CsvPropertyAttribute : Attribute
    {
        public int Index { get; set; }
        public string? Name { get; set; }
        public string Enclosure { get; set; }
        static int IndexDefaultValue () => Int32.MaxValue - 1000;

        public CsvPropertyAttribute(int index, string? name = null, string enclosure = "")
        {
            Index = index;
            Name = name;
            Enclosure = enclosure;
        }
        
        public CsvPropertyAttribute(string? name = null, string enclosure = "")
        {
            Index = IndexDefaultValue();
            Name = name;
            Enclosure = enclosure;
        }
    }
}