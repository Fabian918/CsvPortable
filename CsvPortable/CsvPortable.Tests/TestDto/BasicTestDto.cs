using CsvPortable.Attributes;
using CsvPortable.Dtos;
using CsvPortable.Interfaces;

namespace CsvPortable.Tests.TestDto;

public class BasicTestDto
{
    [CsvProperty(enclosure: "\"")] public string? String { get; set; }
    [CsvProperty(enclosure: "\"")] public char? Char { get; set; }

    [CsvProperty()] public bool? Bool { get; set; }

    [CsvProperty()] public byte? Byte { get; set; }

    [CsvProperty()] public int? Int { get; set; }

    [CsvProperty()] public double? Double { get; set; }
}