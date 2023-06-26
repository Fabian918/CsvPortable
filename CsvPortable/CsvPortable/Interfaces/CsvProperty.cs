using System.Reflection;
using CsvPortable.Attributes;

namespace CsvPortable.Interfaces;

public class CsvProperty
{
    /// <summary>
    /// Csv Property attribute.
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CsvProperty(PropertyInfo? propertyInfo)
    {
        PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

        var csvAttribute =
            (PropertyInfo.GetCustomAttributes(false).First(k => k.GetType() == typeof(CsvPropertyAttribute)) as
                CsvPropertyAttribute) ?? throw new ArgumentNullException($"Property '{propertyInfo.Name}' has no CsvPropertyAttribute.");
            
            
        Manipulations = (PropertyInfo.GetCustomAttributes(false).OfType<CsvManipulateAttribute>()).ToList();

        Index = csvAttribute!.Index;
        Name = csvAttribute.Name!;
    }

    public PropertyInfo PropertyInfo { get; init; }
    public int Index { get; init; }
    public string Name { get; init; }

    public string Documentation { get; init; } = string.Empty;

    public object? DefaultValue { get; set; } = null;

    public List<CsvManipulateAttribute> Manipulations { get; init; }

    public string PerformManipulations(object value)
    {
        string? ret = value?.ToString();

        foreach (var manipulation in Manipulations)
        {
            ret = manipulation.ManipulateValue(ret);
        }

        return ret ?? string.Empty;
    }
}