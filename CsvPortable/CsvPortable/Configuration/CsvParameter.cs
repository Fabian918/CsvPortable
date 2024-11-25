// Copyright (c) 2024 Fabian Hering
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using CsvPortable.Interfaces;

namespace CsvPortable.Configuration
{
  /// <summary>
  /// CsvParameter object that specifies all parameters for working with csv payloads.
  /// - Configurations for objects
  /// - CsvDelimiter
  /// - PropertyMode
  /// -...
  /// </summary>
  public class CsvParameter
  {
    /// <summary>
    /// Default configuration for the CsvParameter.
    /// </summary>
    public static readonly CsvConfiguration? DefaultConfiguration = null;

    /// <summary>
    /// Default delimiter for the CsvParameter.
    /// </summary>
    public static readonly CsvDelimiter DefaultDelimiter = ";";

    /// <summary>
    /// Default value for CloseEnd.
    /// </summary>
    public static readonly bool DefaultCloseEnd = true;

    /// <summary>
    /// Default PropertyMode for the CsvParameter.
    /// </summary>
    public static readonly PropertyMode DefaultPropertyMode = PropertyMode.All;

    /// <summary>
    /// Default Specified Configurations for the CsvParameter.
    /// </summary>
    public static readonly List<(Type, CsvConfiguration)> DefaultSpecifiedConfigurations = new List<(Type, CsvConfiguration)>();


    /// <summary>
    /// Initializes a new instance of the <see cref="CsvParameter"/> class.
    /// </summary>
    /// <param name="configuration">configuration.</param>
    /// <param name="delimiter">delimiter.</param>
    /// <param name="closeEnd">closeEnd.</param>
    /// <param name="propertyMode">propertyMode.</param>
    /// <param name="specifiedConfigurations">specifiedConfigurations.</param>
    public CsvParameter(CsvConfiguration? configuration = null, CsvDelimiter? delimiter = null, bool? closeEnd = null, PropertyMode? propertyMode = null, List<(Type Type, CsvConfiguration Configuration)>? specifiedConfigurations = null)
    {
      this.Configuration = configuration ?? DefaultConfiguration;
      this.Delimiter = delimiter ?? DefaultDelimiter;
      this.CloseEnd = closeEnd ?? DefaultCloseEnd;
      this.PropertyMode = propertyMode ?? DefaultPropertyMode;
      this.SpecifiedConfigurations = specifiedConfigurations ?? DefaultSpecifiedConfigurations;
    }

    /// <summary>
    /// Gets the Default CsvParameter, no specific configuration.
    /// </summary>
    public static CsvParameter Default
    {
      get => (DefaultConfiguration, DefaultDelimiter);
    }

    /// <summary>
    /// Gets or Sets Configuration for the Type.
    /// </summary>
    public CsvConfiguration? Configuration { get; set; }

    /// <summary>
    /// Gets or Sets Delimiter --> default ";".
    /// </summary>
    public CsvDelimiter Delimiter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or Sets CloseEnd (The CSVLine gets closed(\r\n)) --> default "true".
    /// </summary>
    public bool CloseEnd { get; set; }

    /// <summary>
    /// Gets or sets the property mode.
    /// </summary>
    public PropertyMode PropertyMode { get; set; }

    /// <summary>
    /// Gets or Sets Specified Configurations.
    /// For Class X use that Specification.
    /// </summary>
    public List<(Type Type, CsvConfiguration Configuration)> SpecifiedConfigurations { get; set; }


    public static implicit operator CsvParameter((CsvConfiguration? Configuration, CsvDelimiter Delimiter) tupel)
    {
      return new CsvParameter(tupel.Configuration, tupel.Delimiter);
    }

    public static implicit operator CsvParameter((CsvConfiguration? Configuration, CsvDelimiter Delimiter, bool CloseEnd) tupel)
    {
      return new CsvParameter(tupel.Configuration, tupel.Delimiter, tupel.CloseEnd);
    }

    /// <summary>
    /// Get specified type parameter to use.
    /// </summary>
    /// <param name="type">type.</param>
    /// <param name="closeEnd">closeEnd.</param>
    /// <returns>CsvParameter.</returns>
    internal CsvParameter ParameterToUse(Type type, bool closeEnd)
    {
      bool MatchType((Type Type, CsvConfiguration Configuration) t)
      {
        return t.Type == type;
      }

      var configToUse = this.SpecifiedConfigurations.Exists(MatchType)
        ? this.SpecifiedConfigurations.FirstOrDefault(MatchType).Configuration
        : this.Configuration;

      return new CsvParameter(configToUse, this.Delimiter, closeEnd, this.PropertyMode, this.SpecifiedConfigurations);
    }
  }
}