namespace CsvPortable.Attributes
{
    public abstract class CsvManipulateAttribute : Attribute
    {
        public abstract int ManipulationType { get; }
        public abstract string Documentation { get; }
        public abstract string ManipulateValue(object value);
    }
}