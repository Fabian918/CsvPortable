using JetBrains.Annotations;

namespace CsvPortable.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
[UsedImplicitly]
public class CsvIgnoreAttribute : Attribute
{
   
}