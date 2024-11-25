// Copyright (c) 2024 Fabian Hering
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using JetBrains.Annotations;

namespace CsvPortable.Attributes
{
    /// <summary>
    /// CsvPropertyAttribute that indicates the property is relevant for csv serialization and deserialization.
    /// The property can specify additional properties like an index, custom column name etc.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    [UsedImplicitly]
    public class CsvPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvPropertyAttribute"/> class.
        /// </summary>
        /// <param name="index">index.</param>
        /// <param name="name">name.</param>
        public CsvPropertyAttribute(int index, string? name = null, Func<object, string>? customTransfomer = null)
        {
            this.Index = index;
            this.Name = name;
            this.CustomTransformer = customTransfomer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvPropertyAttribute"/> class.
        /// </summary>
        /// <param name="name">name.</param>
        public CsvPropertyAttribute(string? name = null)
        {
            this.Index = IndexDefaultValue();
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets index of the property.(position in the csv row).
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets custom csv name for the property, null by default - takes the property name.
        /// </summary>
        public string? Name { get; set; }

        public Func<object, string>? CustomTransformer { get; set; }

        /// <summary>
        /// Default value for the property index.
        /// </summary>
        /// <returns>int as index.</returns>
        public static int IndexDefaultValue() => int.MaxValue - 1000;
    }
}