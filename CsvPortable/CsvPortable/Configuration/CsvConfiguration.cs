// Copyright (c) 2024 Fabian Hering
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace CsvPortable.Configuration
{
  /// <summary>
  /// DTO that specifies custom behaviour for types.
  /// </summary>
  public class CsvConfiguration
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CsvConfiguration"/> class.
    /// </summary>
    /// <param name="type">type.</param>
    /// <param name="date">date.</param>
    public CsvConfiguration(int type, string date)
    {
      this.Type = type;
      this.Date = DateTime.Parse(date);
    }

    /// <summary>
    /// Gets or sets global list of configurations.
    /// </summary>
    public static List<CsvConfiguration> Configurations { get; set; } = new List<CsvConfiguration>();

    /// <summary>
    /// Gets the type the configuration applys to.
    /// </summary>
    public int Type { get; }

    /// <summary>
    /// Gets the date of the configuration.
    /// </summary>
    public DateTime Date { get; }

    public static implicit operator CsvConfiguration(int configurationType)
    {
      return Configurations.First(k => k.Type == configurationType);
    }

    public static bool operator !=(CsvConfiguration conf1, CsvConfiguration conf2)
    {
      return !(conf1 == conf2);
    }

    public static bool operator ==(CsvConfiguration? conf1, CsvConfiguration? conf2)
    {
      if (conf1 is null && conf2 is null)
      {
        return true;
      }

      if (conf1 is null || conf2 is null)
      {
        return false;
      }

      return
        conf1.Date == conf2.Date &&
        conf1.Type == conf2.Type;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (ReferenceEquals(obj, null))
      {
        return false;
      }

      throw new Exception();
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return
        this.Date.GetHashCode() *
        this.Type.GetHashCode();
    }
  }
}