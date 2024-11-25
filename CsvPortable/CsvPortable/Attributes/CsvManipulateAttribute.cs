// Copyright (c) 2024 Fabian Hering
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace CsvPortable.Attributes
{
  /// <summary>
  /// Indicates that the property should be manipulated before deserializing it to csv.
  /// Attribute can be inherited to create custom manipulation.
  /// </summary>
  public abstract class CsvManipulateAttribute : Attribute
  {
    /// <summary>
    /// Gets Type of manipulation.
    /// </summary>
    public abstract int ManipulationType { get; }

    /// <summary>
    /// Gets the documentation value.
    /// </summary>
    public abstract string Documentation { get; }

    /// <summary>
    /// Function to manipulate the value.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>manipulated value.</returns>
    public abstract string ManipulateValue(object? value);
  }
}