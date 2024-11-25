// Copyright (c) 2024 Fabian Hering
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace CsvPortable.Interfaces;

/// <summary>
/// Csv Delimiter, that specifies the delimiter used in the csv file.
/// </summary>
public class CsvDelimiter
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CsvDelimiter"/> class.
  /// </summary>
  /// <param name="value">value.</param>
  public CsvDelimiter(string value)
  {
    if (value is null || value.Length == 0)
    {
      throw new ArgumentException("Delimiter must not be null or empty", nameof(value));
    }

    if (value.Contains("\""))
    {
      throw new ArgumentException("Delimiter must not contain \"", nameof(value));
    }

    this.Value = value;
  }

  /// <summary>
  /// Gets delimiter value.
  /// </summary>
  public string Value { get; init; }

  /// <summary>
  /// Implicit conversion from <see cref="CsvDelimiter"/> to <see cref="string"/>.
  /// </summary>
  /// <param name="delimiter">delimiter.</param>
  /// <returns>delimiter as string.</returns>
  public static implicit operator string(CsvDelimiter delimiter) => delimiter.Value;

  /// <summary>
  /// Implicit conversion from <see cref="string"/> to <see cref="CsvDelimiter"/>.
  /// </summary>
  /// <param name="delimiter">delimiter.</param>
  /// <returns>new CsvDelimiter from string.</returns>
  public static implicit operator CsvDelimiter(string delimiter) => new CsvDelimiter(delimiter);
}