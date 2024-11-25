// Copyright (c) 2024 Fabian Hering
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
namespace CsvPortable.Interfaces;
using CsvPortable.Attributes;
using System.Reflection;

/// <summary>
/// Csp Property class.
/// Represents a property in a class that is relevant for csv (De)Serialization.
/// </summary>
public class CsvProperty
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CsvProperty"/> class.
  /// Csv Property attribute.
  /// </summary>
  /// <param name="propertyInfo">propertyInfo.</param>
  /// <exception cref="ArgumentNullException">ArgumentNullException.</exception>
  public CsvProperty(PropertyInfo? propertyInfo)
  {
    this.PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

    CsvPropertyAttribute? csvAttribute = this.PropertyInfo.GetCustomAttributes(false).FirstOrDefault(k => k.GetType() == typeof(CsvPropertyAttribute)) as CsvPropertyAttribute;


    this.Manipulations = this.PropertyInfo.GetCustomAttributes(false).OfType<CsvManipulateAttribute>().ToList();

    this.Index = csvAttribute?.Index ?? CsvPropertyAttribute.IndexDefaultValue();
    this.Name = csvAttribute?.Name ?? this.PropertyInfo.Name;
  }

  /// <summary>
  /// Gets propertyInfo of the property.
  /// </summary>
  public PropertyInfo PropertyInfo { get; init; }

  /// <summary>
  /// Gets index of the property (position in the row).
  /// </summary>
  public int Index { get; init; }

  /// <summary>
  /// Gets the Name of the property.
  /// </summary>
  public string Name { get; init; }

  /// <summary>
  /// Gets the documentation value of the property.
  /// </summary>
  public string Documentation { get; init; } = string.Empty;

  /// <summary>
  /// Gets or sets the default value of the property.
  /// </summary>
  public object? DefaultValue { get; set; } = null;

  /// <summary>
  /// Gets a list that stores all manipulations that should be performed on the value.
  /// </summary>
  public List<CsvManipulateAttribute> Manipulations { get; init; }

  /// <summary>
  /// Performs all manipulations on the value.
  /// </summary>
  /// <param name="value">value.</param>
  /// <returns>manipulated string value.</returns>
  public string PerformManipulations(object value)
  {
    string? ret = value?.ToString();

    foreach (var manipulation in this.Manipulations)
    {
      ret = manipulation.ManipulateValue(ret);
    }

    return ret ?? string.Empty;
  }
}