using System.Reflection;
using CsvPortable.Attributes;

namespace CsvPortable.Interfaces;

public class CsvProperty
{
    public CsvProperty(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

        var csvAttribute =
            (PropertyInfo.GetCustomAttributes(false).First(k => k.GetType() == typeof(CsvPropertyAttribute)) as
                CsvPropertyAttribute);
            
            
        Manipulations = (PropertyInfo.GetCustomAttributes(false).OfType<CsvManipulateAttribute>()).ToList();

        Index = csvAttribute!.Index;
        Name = csvAttribute.Name;
        Enclosure = csvAttribute.Enclosure;
    }

    public PropertyInfo PropertyInfo { get; init; }
    public int Index { get; init; }
    public string Name { get; init; }
    public string Enclosure { get; init; }

    public string Documentation { get; init; }

    public object? DefaultValue { get; set; } = null;

    public List<CsvManipulateAttribute> Manipulations { get; init; }

    public string PerformManipulations(object value)
    {
        string ret = value?.ToString();

        foreach (var manipulation in Manipulations)
        {
            ret = manipulation.ManipulateValue(ret);
        }

        return ret;
    }
}