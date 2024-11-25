// Copyright (c) 2024 Fabian Hering
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace CsvPortable.Extensions
{
  /// <summary>
  /// Extensions for csv handling.
  /// </summary>
  internal static class CsvExtensions
  {
    private static List<(string ValueToReplace, string Replacement)> ReplaceAbles
    {
      get =>
        new List<(string ValueToReplace, string Replacement)>()
        {
          // Replacing " with 2 " for csv conformity
          ("\"", "\"\""),
        };
    }

    /// <summary>
    /// transforms a string to a csv conform string.
    /// </summary>
    /// <param name="valueAsString">valueAsString.</param>
    /// <returns>conform csv value.</returns>
    internal static string ToCsvConform(this string valueAsString)
    {
      foreach (var item in ReplaceAbles)
      {
        valueAsString = valueAsString.Replace(item.ValueToReplace, item.Replacement);
      }

      return valueAsString;
    }
  }
}